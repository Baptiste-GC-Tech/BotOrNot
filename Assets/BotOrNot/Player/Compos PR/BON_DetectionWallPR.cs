using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_DetectionWallPR : MonoBehaviour
{
    private BON_CCPlayer _CCPlayer;

    private void Start()
    {
        _CCPlayer = GameObject.FindFirstObjectByType<BON_CCPlayer>();
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter, also...");

        if (other.gameObject.layer == LayerMask.NameToLayer("Wall")) //wall
        {
            if (_CCPlayer.GetComponent<BON_MovePR>().MoveXAxisDir == 1) //wall on right
            {
                _CCPlayer.AvatarState.IsAgainstWallRight = true;
                print(_CCPlayer.AvatarState.IsAgainstWallRight);
            }
            else if (_CCPlayer.GetComponent<BON_MovePR>().MoveXAxisDir == -1)
            {
                _CCPlayer.AvatarState.IsAgainstWallLeft = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall")) //wall
        {
            _CCPlayer.AvatarState.IsAgainstWallRight = false;
            _CCPlayer.AvatarState.IsAgainstWallLeft = false;            
        }
    }
}
