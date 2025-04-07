using UnityEngine;
using Cinemachine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Threading;
using System.Collections;



[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraFollowMode : MonoBehaviour
{
    /*
     *  FIELDS
     */

    InputAction _moveAction;

    public enum FollowMode
    {
        Player,
        OtherObject,
        Barycenter,
        Auto,
        Shake
    }

    [Header("Cibles de suivi")]
    [Tooltip("Transform du joueur contrôlé.")]
    public Transform Player;

    [Tooltip("Transform d’un objet secondaire à suivre.")]
    public Transform OtherTarget;

    [Tooltip("Transform intermédiaire que la Cinemachine suit réellement.")]
    public Transform FollowTarget;

    [Header("Offset par défaut")]
    [Tooltip("Décalage horizontal appliqué à la caméra quand elle suit le joueur. Est opposé en fonction du sens du joueur.")]
    public float OffsetX = 6f;

    [Tooltip("Décalage vertical appliqué à la caméra.")]
    public float OffsetY = 0f;

    [Tooltip("Décalage en profondeur appliqué à la caméra.")]
    public float OffsetZ = 6f;

    [Header("Interpolation")]
    [Tooltip("Vitesse de transition de position du FollowTarget.")]
    [Range(0.1f, 20f)] public float FollowLerpSpeed = 5f;

    [Tooltip("Vitesse de transition pour les offsets caméra.")]
    [Range(0.1f, 20f)] public float OffsetLerpSpeed = 4f;

    
    [Header("Shake")]
    [Tooltip("Intensité de shake de la caméra.")]
    public float Intensity = 0f;
    [Tooltip("Temps de shake de la caméra.")]
    public float Shaketime = 0f;
    

    [HideInInspector] public FollowMode currentMode = FollowMode.Auto;

    private CinemachineFramingTransposer _framingTransposer;
    private CinemachineComposer _composer;
    private CinemachineVirtualCamera _vcam;
    private CinemachineBasicMultiChannelPerlin _perlinNoise;


    private enum Direction { _None, _Left, _Right } // REF
    private Direction _currentDirection = Direction._Right; // REF

    private class TriggerTargetData
    {
        public Transform Target;
        public bool OverrideOffset;
        public float OffsetX;
        public float OffsetY;
        public float OffsetZ;
        public bool FocusOnly;
    }

    private List<TriggerTargetData> _activeTriggerTargets = new List<TriggerTargetData>();
    private TriggerTargetData _currentTriggerData = null;



    /*
    *  CLASS METHODS
    */

    private Vector3 GetOffsetFromDirection()
    {
        float x = _currentDirection == Direction._Left ? -OffsetX : OffsetX;
        return new Vector3(x, OffsetY, OffsetZ);
    }

    public void RegisterTriggerTarget(Transform target, bool overrideOffset, float customX, float customY, float customZ, bool focusOnly)
    {
        if (_activeTriggerTargets.Exists(t => t.Target == target)) return;

        var data = new TriggerTargetData
        {
            Target = target,
            OverrideOffset = overrideOffset,
            OffsetX = customX,
            OffsetY = customY,
            OffsetZ = customZ,
            FocusOnly = focusOnly
        };

        _activeTriggerTargets.Add(data);
    }

    public void UnregisterTriggerTarget(Transform target)
    {
        _activeTriggerTargets.RemoveAll(t => t.Target == target);
    }

    /* Utilisation du script Cinemachine Collision Impulse Source sur le gameobject BON_PR > Petit_Robot */
    /*
    public void ShakeCamera(float intensity, float shaketime)
    {
        print("Shake Triggered");
        perlinNoise.m_AmplitudeGain = intensity;
        StartCoroutine(WaitTime(shaketime));
    }

    IEnumerator WaitTime(float shaketime)
    {
        yield return new WaitForSeconds(shaketime);
        ResetIntensity();
    }

    void ResetIntensity()
    {
        perlinNoise.m_AmplitudeGain = 0;
    }
    */

    /*
     
     */


    private void Start()
    {
        /* New Inputs */
        // MoveAction = InputSystem.actions.FindAction("Player/Move");

        _vcam = GetComponent<CinemachineVirtualCamera>();
        _framingTransposer = _vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        _composer = _vcam.GetCinemachineComponent<CinemachineComposer>();
        _perlinNoise = _vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        /* -> Appliquer l'offset au Start
         * if (_framingTransposer != null)
         *      _framingTransposer.m_TrackedObjectOffset = new Vector3(offsetX, 0f, 0f);
         */

    }

    private void Update()
    {
        /* New Inputs */
        // print(MoveAction.ReadValue<Vector2>());
        _moveAction = InputSystem.actions.FindAction("ActionsMapPR/Move");

        if (FollowTarget == null || Player == null) return;

        _currentTriggerData = _activeTriggerTargets.Count > 0 ? _activeTriggerTargets[^1] : null;

        Vector3 desiredFollowPos = FollowTarget.position;
        Vector3 desiredOffset = Vector3.zero;

        switch (currentMode)
        {
            /* Follow Player */
            case FollowMode.Player:
                desiredFollowPos = Player.position;
                desiredOffset = GetOffsetFromDirection();
                break;
            /* Follow Object Spécifique */
            case FollowMode.OtherObject:
                if (OtherTarget != null)
                {
                    desiredFollowPos = OtherTarget.position;
                    desiredOffset = new Vector3(0f, OffsetY, OffsetZ);
                }
                break;

            /* Follow Barycentre entre Player et Objet Spécifique */
            case FollowMode.Barycenter:
                if (OtherTarget != null)
                {
                    desiredFollowPos = (Player.position + OtherTarget.position) / 2f;
                    desiredOffset = new Vector3(0f, OffsetY, OffsetZ);
                }
                break;

            /* Follow Automatique :
             * Follow Player de base (si non trigger zone) + Direction avec Offsets appliqués (offsetX et offsetZ)
             */
            case FollowMode.Auto:
                if (_currentTriggerData != null)
                {
                    desiredFollowPos = _currentTriggerData.FocusOnly
                        ? _currentTriggerData.Target.position
                        : (Player.position + _currentTriggerData.Target.position) / 2f;

                    desiredOffset = _currentTriggerData.OverrideOffset
                        ? new Vector3(_currentTriggerData.OffsetX, _currentTriggerData.OffsetY, _currentTriggerData.OffsetZ)
                        : new Vector3(OffsetX, OffsetY, OffsetZ);
                }
                else
                {
                    desiredFollowPos = Player.position;
                    desiredOffset = GetOffsetFromDirection();
                }
                break;
        }

        FollowTarget.position = Vector3.Lerp(FollowTarget.position, desiredFollowPos, Time.deltaTime * FollowLerpSpeed);

        /* Appliquation des offsets désirés sur le component CinemachineVirtualCamera > Body > Tracked Object Offset */
        if (_framingTransposer != null)
            _framingTransposer.m_TrackedObjectOffset = Vector3.Lerp(_framingTransposer.m_TrackedObjectOffset, new Vector3(desiredOffset.x, desiredOffset.y, desiredOffset.z), Time.deltaTime * OffsetLerpSpeed);

        if (_vcam != null && _vcam.m_Lens.Orthographic)
        {
            var lens = _vcam.m_Lens;
            lens.OrthographicSize = Mathf.Lerp(
                lens.OrthographicSize,
                desiredOffset.z,
                Time.deltaTime * OffsetLerpSpeed);
            _vcam.m_Lens = lens;
        }

        /*
         * Non utilisé car on n'utilise plus l'Aim
         * 
        if (_composer != null)
        {
            Vector3 targetTrackedOffset = new Vector3(desiredOffset.x, 0f, desiredOffset.z);
            _composer.m_TrackedObjectOffset = Vector3.Lerp(_composer.m_TrackedObjectOffset, targetTrackedOffset, Time.deltaTime * offsetLerpSpeed);
        }
        */

        /* !!! ATTENTION A MODIFIER ABSOLUMENT POUR LES NOUVEAUX INPUTS !!! */

        //print(MoveAction.ReadValue<Vector2>().x);

        if (_moveAction.ReadValue<Vector2>().x == 1)
            _currentDirection = Direction._Left;
        else if (_moveAction.ReadValue<Vector2>().x == -1)
            _currentDirection = Direction._Right;
        

        /*
        if (Input.GetKey(KeyCode.A))
            _currentDirection = Direction._Left;
        else if (Input.GetKey(KeyCode.A))
            _currentDirection = Direction._Right;
        */
    }

}
