using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_DetectionWallPR : MonoBehaviour
{
    private BON_CCPlayer _CCPlayer;
    private Vector3 otherPos;
    private Vector3 delta;

    private void Start()
    {
        _CCPlayer = GameObject.FindFirstObjectByType<BON_CCPlayer>();
    }

    private void OnTriggerEnter(Collider other)
    {        
        if (other.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            otherPos = other.ClosestPoint(transform.position); //contact point
            delta = otherPos - transform.position;

            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y)) //collide on X => wall
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
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y)) //collide on X => wall
            {
                _CCPlayer.AvatarState.IsAgainstWallRight = false;
                _CCPlayer.AvatarState.IsAgainstWallLeft = false;
            }
        }
    }
}
