using UnityEngine;
public class BON_DetectionWallPR : MonoBehaviour
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
            _otherPos = other.ClosestPoint(transform.position); //contact point
            _normal = transform.position - _otherPos;

            if (Mathf.Abs(_normal.x) > Mathf.Abs(_normal.y)) //collide on X => wall
            {
                if (_CCPlayer.GetComponent<BON_MovePR>().MoveXAxisDir == 1)
                {
                    _CCPlayer.AvatarState.IsAgainstWallRight = true;
                }
                else if (_CCPlayer.GetComponent<BON_MovePR>().MoveXAxisDir == -1)
                {
                    _CCPlayer.AvatarState.IsAgainstWallLeft = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Terrain")) //wall
        {
            _CCPlayer.AvatarState.IsAgainstWallRight = false;
            _CCPlayer.AvatarState.IsAgainstWallLeft = false;
        }
    }
}
