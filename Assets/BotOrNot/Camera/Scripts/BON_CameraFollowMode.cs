using UnityEngine;
using Cinemachine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class BON_CameraFollowMode : MonoBehaviour
{
    /*
     *  FIELDS
     */

    private InputAction _moveAction;
    [SerializeField] private FollowMode _currentMode = FollowMode.Auto;

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
    [SerializeField] private Transform _player;

    [Tooltip("Transform d’un objet secondaire à suivre.")]
    [SerializeField] private Transform _otherTarget;

    [Tooltip("Transform intermédiaire que la Cinemachine suit réellement.")]
    [SerializeField] private Transform _followTarget;

    [Header("Offset par défaut")]
    [Tooltip("Décalage horizontal appliqué à la caméra quand elle suit le joueur. Est opposé en fonction du sens du joueur.")]
    [SerializeField] private float _offsetX = 6f;

    [Tooltip("Décalage vertical appliqué à la caméra.")]
    [SerializeField] private float _offsetY = 0f;

    [Tooltip("Décalage en profondeur appliqué à la caméra.")]
    [SerializeField] private float _offsetZ = 6f;

    [Header("Interpolation")]
    [Tooltip("Vitesse de transition de position du FollowTarget.")]
    [Range(0.1f, 20f)][SerializeField] private float _followLerpSpeed = 5f;

    [Tooltip("Vitesse de transition pour les offsets caméra.")]
    [Range(0.1f, 20f)][SerializeField] private float _offsetLerpSpeed = 4f;

    public FollowMode CurrentMode { get; private set; } = FollowMode.Auto;

    private CinemachineFramingTransposer _framingTransposer;
    private CinemachineComposer _composer;
    private CinemachineVirtualCamera _vcam;
    private CinemachineBasicMultiChannelPerlin _perlinNoise;

    private enum Direction { None, Left, Right }
    private Direction _currentDirection = Direction.Right;

    private class TriggerTargetData
    {
        public Transform Target;
        public bool OverrideOffset;
        public float OffsetX;
        public float OffsetY;
        public float OffsetZ;
        public bool FocusOnly;
    }

    private readonly List<TriggerTargetData> _activeTriggerTargets = new();
    private TriggerTargetData _currentTriggerData = null;

    /*
     *  CLASS METHODS
     */

    private Vector3 PRIVGetOffsetFromDirection()
    {
        float x = _currentDirection == Direction.Left ? -_offsetX : _offsetX;
        return new Vector3(x, _offsetY, _offsetZ);
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

    /*
     *  UNITY METHODS
     */

    private void Start()
    {
        _vcam = GetComponent<CinemachineVirtualCamera>();
        _framingTransposer = _vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        _composer = _vcam.GetCinemachineComponent<CinemachineComposer>();
        _perlinNoise = _vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        _moveAction = InputSystem.actions.FindAction("ActionsMapPR/Move");

        if (_followTarget == null || _player == null) return;

        _currentTriggerData = _activeTriggerTargets.Count > 0 ? _activeTriggerTargets[^1] : null;

        Vector3 desiredFollowPos = _followTarget.position;
        Vector3 desiredOffset = Vector3.zero;

        switch (_currentMode)
        {
            case FollowMode.Player:
                desiredFollowPos = _player.position;
                desiredOffset = PRIVGetOffsetFromDirection();
                break;

            case FollowMode.OtherObject:
                if (_otherTarget != null)
                {
                    desiredFollowPos = _otherTarget.position;
                    desiredOffset = new Vector3(0f, _offsetY, _offsetZ);
                }
                break;

            case FollowMode.Barycenter:
                if (_otherTarget != null)
                {
                    desiredFollowPos = (_player.position + _otherTarget.position) / 2f;
                    desiredOffset = new Vector3(0f, _offsetY, _offsetZ);
                }
                break;

            case FollowMode.Auto:
                if (_currentTriggerData != null)
                {
                    desiredFollowPos = _currentTriggerData.FocusOnly
                        ? _currentTriggerData.Target.position
                        : (_player.position + _currentTriggerData.Target.position) / 2f;

                    desiredOffset = _currentTriggerData.OverrideOffset
                        ? new Vector3(_currentTriggerData.OffsetX, _currentTriggerData.OffsetY, _currentTriggerData.OffsetZ)
                        : new Vector3(_offsetX, _offsetY, _offsetZ);
                }
                else
                {
                    desiredFollowPos = _player.position;
                    desiredOffset = PRIVGetOffsetFromDirection();
                }
                break;
        }

        _followTarget.position = Vector3.Lerp(_followTarget.position, desiredFollowPos, Time.deltaTime * _followLerpSpeed);

        if (_framingTransposer != null)
        {
            _framingTransposer.m_TrackedObjectOffset = Vector3.Lerp(
                _framingTransposer.m_TrackedObjectOffset,
                new Vector3(desiredOffset.x, desiredOffset.y, desiredOffset.z),
                Time.deltaTime * _offsetLerpSpeed);
        }

        if (_vcam != null && _vcam.m_Lens.Orthographic)
        {
            var lens = _vcam.m_Lens;
            lens.OrthographicSize = Mathf.Lerp(lens.OrthographicSize, desiredOffset.z, Time.deltaTime * _offsetLerpSpeed);
            _vcam.m_Lens = lens;
        }

        Vector2 moveValue = _moveAction.ReadValue<Vector2>();
        if (moveValue.x == 1)
            _currentDirection = Direction.Left;
        else if (moveValue.x == -1)
            _currentDirection = Direction.Right;
    }
}
