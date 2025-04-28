using UnityEngine;
public class BON_DetectionGroundPR : MonoBehaviour
{
    private BON_CCPlayer _CCPlayer;
    private Vector3 _otherPos;
    private Vector3 _normal;

    private void Start()
    {
        _CCPlayer = GameObject.FindFirstObjectByType<BON_CCPlayer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            _CCPlayer.AvatarState.IsGrounded = true;       
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            _CCPlayer.AvatarState.IsGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            _CCPlayer.AvatarState.IsGrounded = false;
        }
    }
}