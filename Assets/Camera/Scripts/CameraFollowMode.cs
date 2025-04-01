using UnityEngine;
using Cinemachine;
using System.Collections.Generic;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraFollowMode : MonoBehaviour
{
    /*
     *  FIELDS
     */

    public enum FollowMode
    {
        Player,
        OtherObject,
        Barycenter,
        Auto
    }

    [Header("Cibles de suivi")]
    [Tooltip("Transform du joueur contrôlé.")]
    public Transform player;

    [Tooltip("Transform d’un objet secondaire à suivre.")]
    public Transform otherTarget;

    [Tooltip("Transform intermédiaire que la Cinemachine suit réellement.")]
    public Transform followTarget;

    [Header("Offset par défaut")]
    [Tooltip("Décalage horizontal appliqué à la caméra quand elle suit le joueur.")]
    public float offsetX = -3f;

    [Tooltip("Décalage en profondeur appliqué à la caméra.")]
    public float offsetZ = 9f;

    [Header("Interpolation")]
    [Tooltip("Vitesse de transition de position du FollowTarget.")]
    [Range(0.1f, 20f)] public float followLerpSpeed = 5f;

    [Tooltip("Vitesse de transition pour les offsets caméra.")]
    [Range(0.1f, 20f)] public float offsetLerpSpeed = 5f;

    [HideInInspector] public FollowMode currentMode = FollowMode.Auto;

    private CinemachineFramingTransposer _framingTransposer;
    private CinemachineComposer _composer;

    private enum Direction { _None, _Left, _Right }
    private Direction _currentDirection = Direction._Right;

    private class TriggerTargetData
    {
        public Transform target;
        public bool overrideOffset;
        public float offsetX;
        public float offsetZ;
        public bool focusOnly;
    }

    private List<TriggerTargetData> activeTriggerTargets = new List<TriggerTargetData>();
    private TriggerTargetData currentTriggerData = null;

    private void Start()
    {
        var vcam = GetComponent<CinemachineVirtualCamera>();
        _framingTransposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        _composer = vcam.GetCinemachineComponent<CinemachineComposer>();

        /* -> Appliquer l'offset au Start
         * if (_framingTransposer != null)
         *      _framingTransposer.m_TrackedObjectOffset = new Vector3(offsetX, 0f, 0f);
         */    
            
    }

    private void Update()
    {
        if (followTarget == null || player == null) return;

        currentTriggerData = activeTriggerTargets.Count > 0 ? activeTriggerTargets[^1] : null;

        Vector3 desiredFollowPos = followTarget.position;
        Vector3 desiredOffset = Vector3.zero;

        switch (currentMode)
        {
            /* Follow Player */
            case FollowMode.Player:
                desiredFollowPos = player.position;
                desiredOffset = GetOffsetFromDirection();
                break;
            /* Follow Object Spécifique */
            case FollowMode.OtherObject:
                if (otherTarget != null)
                {
                    desiredFollowPos = otherTarget.position;
                    desiredOffset = new Vector3(0f, 0f, offsetZ);
                }
                break;

            /* Follow Barycentre entre Player et Objet Spécifique */
            case FollowMode.Barycenter:
                if (otherTarget != null)
                {
                    desiredFollowPos = (player.position + otherTarget.position) / 2f;
                    desiredOffset = new Vector3(0f, 0f, offsetZ);
                }
                break;

            /* Follow Automatique :
             * Follow Player de base (si non trigger zone) + Direction avec Offsets appliqués (offsetX et offsetZ)
             */
            case FollowMode.Auto:
                if (currentTriggerData != null)
                {
                    desiredFollowPos = currentTriggerData.focusOnly
                        ? currentTriggerData.target.position
                        : (player.position + currentTriggerData.target.position) / 2f;

                    desiredOffset = currentTriggerData.overrideOffset
                        ? new Vector3(currentTriggerData.offsetX, 0f, currentTriggerData.offsetZ)
                        : new Vector3(0f, 0f, offsetZ);
                }
                else
                {
                    desiredFollowPos = player.position;
                    desiredOffset = GetOffsetFromDirection();
                }
                break;

        }

        followTarget.position = Vector3.Lerp(followTarget.position, desiredFollowPos, Time.deltaTime * followLerpSpeed);

        /* Appliquation des offsets désirés sur le component CinemachineVirtualCamera > Body > Tracked Object Offset */
        if (_framingTransposer != null)
            _framingTransposer.m_TrackedObjectOffset = Vector3.Lerp(_framingTransposer.m_TrackedObjectOffset, new Vector3(desiredOffset.x, 0f, desiredOffset.z), Time.deltaTime * offsetLerpSpeed);
        
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

        if (Input.GetKeyDown(KeyCode.A))
            _currentDirection = Direction._Left;
        else if (Input.GetKeyDown(KeyCode.D))
            _currentDirection = Direction._Right;
    }

     /*
     *  CLASS METHODS
     */

    private Vector3 GetOffsetFromDirection()
    {
        return _currentDirection == Direction._Left
            ? new Vector3(-offsetX, 0f, offsetZ)
            : new Vector3(offsetX, 0f, offsetZ);
    }

    public void RegisterTriggerTarget(Transform target, bool overrideOffset, float customX, float customZ, bool focusOnly)
    {
        if (activeTriggerTargets.Exists(t => t.target == target)) return;

        var data = new TriggerTargetData
        {
            target = target,
            overrideOffset = overrideOffset,
            offsetX = customX,
            offsetZ = customZ,
            focusOnly = focusOnly
        };

        activeTriggerTargets.Add(data);
    }


    public void UnregisterTriggerTarget(Transform target)
    {
        activeTriggerTargets.RemoveAll(t => t.target == target);
    }
}
