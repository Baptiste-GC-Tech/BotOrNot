using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_DetectionWallPR : MonoBehaviour
{
    private BON_CCPlayer _CCPlayer;
    private Vector3 _wallPos;

    private void Start()
    {
        _CCPlayer = GameObject.FindFirstObjectByType<BON_CCPlayer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter, also...");

        if (other.gameObject.CompareTag("Wall")) //wall
        {
            _wallPos = other.transform.position;
            if( transform.position.x < _wallPos.x) //wall on right
            {
                print("wall on right");
                _CCPlayer.AvatarState.IsAgainstWallRight = true;
            }
            else
            {
                print("wall on left");
                _CCPlayer.AvatarState.IsAgainstWallLeft = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Wall")) //wall
        {
            print("no wall");
            _wallPos = other.transform.position;
            if (transform.position.x < _wallPos.x) //wall on right
            {
                _CCPlayer.AvatarState.IsAgainstWallRight = false;
            }
            else
            {
                _CCPlayer.AvatarState.IsAgainstWallLeft = false;
            }
        }
    }
}
