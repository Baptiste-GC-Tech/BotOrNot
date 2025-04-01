using UnityEngine;
using Cinemachine;
using System.Collections.Generic;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraFollowMode : MonoBehaviour
{
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

    private CinemachineTransposer transposer;
    private CinemachineComposer composer;

    private enum Direction { None, Left, Right }
    private Direction currentDirection = Direction.Right;

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
        transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();
        composer = vcam.GetCinemachineComponent<CinemachineComposer>();
    }

    private void Update()
    {
        if (followTarget == null || player == null) return;

        currentTriggerData = activeTriggerTargets.Count > 0 ? activeTriggerTargets[^1] : null;

        Vector3 desiredFollowPos = followTarget.position;
        Vector3 desiredOffset = Vector3.zero;

        switch (currentMode)
        {
            case FollowMode.Player:
                desiredFollowPos = player.position;
                desiredOffset = GetOffsetFromDirection();
                break;

            case FollowMode.OtherObject:
                if (otherTarget != null)
                {
                    desiredFollowPos = otherTarget.position;
                    desiredOffset = new Vector3(0f, 0f, offsetZ);
                }
                break;

            case FollowMode.Barycenter:
                if (otherTarget != null)
                {
                    desiredFollowPos = (player.position + otherTarget.position) / 2f;
                    desiredOffset = new Vector3(0f, 0f, offsetZ);
                }
                break;

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

        if (transposer != null)
            transposer.m_FollowOffset = Vector3.Lerp(transposer.m_FollowOffset, desiredOffset, Time.deltaTime * offsetLerpSpeed);

        if (composer != null)
        {
            Vector3 targetTrackedOffset = new Vector3(desiredOffset.x, 0f, 0f);
            composer.m_TrackedObjectOffset = Vector3.Lerp(composer.m_TrackedObjectOffset, targetTrackedOffset, Time.deltaTime * offsetLerpSpeed);
        }

        // !!! ATTENTION A MODIFIER ABSOLUMENT POUR LES NOUVEAUX INPUTS !!!
        if (Input.GetKeyDown(KeyCode.A))
            currentDirection = Direction.Left;
        else if (Input.GetKeyDown(KeyCode.D))
            currentDirection = Direction.Right;
    }

    private Vector3 GetOffsetFromDirection()
    {
        return currentDirection == Direction.Left
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
