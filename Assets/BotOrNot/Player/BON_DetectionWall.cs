using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_DetectionWall : MonoBehaviour
{
    private BON_CCPlayer _CCPlayer;

    private void Start()
    {
        _CCPlayer = GameObject.FindFirstObjectByType<BON_CCPlayer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall") //wall
        {
            //TO DO stop move
            //bool againt wall true
            _CCPlayer.AvatarState.IsAgainstWall = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Wall") //wall
        {
            //TO DO regive move
            _CCPlayer.AvatarState.IsAgainstWall = false;
        }
    }
}
