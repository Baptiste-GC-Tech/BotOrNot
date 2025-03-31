using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraFollowMode : MonoBehaviour
{
    public enum FollowMode
    {
        Player,
        OtherObject,
        Barycenter
    }

    [Header("Cibles de suivi")]
    public Transform player;
    public Transform otherTarget;
    public Transform followTarget;

    [Header("Paramètres d’offset")]
    public float offsetX = 3f;
    public float offsetZ = 9f;

    [Header("Interpolation (smoothness)")]
    [Range(0.1f, 20f)] public float followLerpSpeed = 5f;
    [Range(0.1f, 20f)] public float offsetLerpSpeed = 5f;

    [HideInInspector] public FollowMode currentMode = FollowMode.Player;

    private CinemachineTransposer transposer;
    private CinemachineComposer composer;

    private enum Direction { None, Left, Right }
    private Direction currentDirection = Direction.Right;

    private void Start()
    {
        var vcam = GetComponent<CinemachineVirtualCamera>();
        transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();
        composer = vcam.GetCinemachineComponent<CinemachineComposer>();
    }

    private void Update()
    {
        if (followTarget == null || player == null)
            return;

        Vector3 desiredFollowPos = followTarget.position;
        Vector3 desiredOffset = Vector3.zero;

        switch (currentMode)
        {
            case FollowMode.Player:
                desiredFollowPos = player.position;

                if (Input.GetKeyDown(KeyCode.A))
                    currentDirection = Direction.Left;
                else if (Input.GetKeyDown(KeyCode.D))
                    currentDirection = Direction.Right;

                if (currentDirection == Direction.Left)
                    desiredOffset = new Vector3(-offsetX, 0f, offsetZ);
                else if (currentDirection == Direction.Right)
                    desiredOffset = new Vector3(offsetX, 0f, offsetZ);
                break;

            case FollowMode.OtherObject:
                if (otherTarget != null)
                    desiredFollowPos = otherTarget.position;

                desiredOffset = new Vector3(0f, 0f, offsetZ);
                break;

            case FollowMode.Barycenter:
                if (otherTarget != null)
                    desiredFollowPos = (player.position + otherTarget.position) / 2f;

                desiredOffset = new Vector3(0f, 0f, offsetZ);
                break;
        }

        followTarget.position = Vector3.Lerp(followTarget.position, desiredFollowPos, Time.deltaTime * followLerpSpeed);

        if (transposer != null)
        {
            Vector3 currentOffset = transposer.m_FollowOffset;
            transposer.m_FollowOffset = Vector3.Lerp(currentOffset, desiredOffset, Time.deltaTime * offsetLerpSpeed);
        }

        if (composer != null)
        {
            Vector3 currentTrackedOffset = composer.m_TrackedObjectOffset;
            Vector3 targetTrackedOffset = new Vector3(desiredOffset.x, 0f, 0f);
            composer.m_TrackedObjectOffset = Vector3.Lerp(currentTrackedOffset, targetTrackedOffset, Time.deltaTime * offsetLerpSpeed);
        }
    }
}
