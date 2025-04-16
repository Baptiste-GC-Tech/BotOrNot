using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_GroundSnapPR : MonoBehaviour
{
    /*
    *  FIELDS
    */
    /* References */
    private BON_CCPlayer _CC;
    private BON_MovePR _moveComp;

    /* Raycast offsets */
    private Vector3 _farLeftRayOffset = new Vector3(-0.5f, -0.5f, 0.0f);
    private Vector3 _leftRayOffset = new Vector3(-0.25f, -0.75f, 0.0f);
    private Vector3 _middleRayOffset = new Vector3(0.0f, -1.0f, 0.0f);
    private Vector3 _rightRayOffset = new Vector3(0.25f, -0.75f, 0.0f);
    private Vector3 _farRightRayOffset = new Vector3(0.5f, -0.5f, 0.0f);


    /*
     *  UNITY METHODS
     */
    private void Start()
    {
        _CC = GetComponent<BON_CCPlayer>();
        _moveComp = GetComponent<BON_MovePR>();
    }

    private void Update()
    {
        Debug.DrawRay(transform.position + _farLeftRayOffset * transform.localScale.y, Vector3.down * 0.05f, Color.red);
        Debug.DrawRay(transform.position + _leftRayOffset * transform.localScale.y, Vector3.down * 0.05f, Color.red);
        Debug.DrawRay(transform.position + _middleRayOffset * transform.localScale.y, Vector3.down * 0.05f, Color.red);
        Debug.DrawRay(transform.position + _rightRayOffset * transform.localScale.y, Vector3.down * 0.05f, Color.red);
        Debug.DrawRay(transform.position + _farRightRayOffset * transform.localScale.y, Vector3.down * 0.05f, Color.red);
        //// TODO: add a ground layer mask here
        //if (Physics.Raycast(transform.position + _leftRayOffset, Vector3.down, 0.1f) ||
        //    Physics.Raycast(transform.position + _middleRayOffset, Vector3.down, 0.1f) ||
        //    Physics.Raycast(transform.position + _rightRayOffset, Vector3.down, 0.1f)
        //    )
        //{

        //}
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("Trigger enter !");

    //    // Ignores collisions not involving floor
    //    if (other.tag != "Floor") return;

    //    // PR is in the air now and wasn't a frame ago ?
    //    // If so, try snap to the ground when the snapping sphere hits floors (for an adherence illusion)
    //    if (!_CC.AvatarState.IsGrounded && _CC.AvatarState.WasGroundedLastFrame)
    //    {
    //        Debug.Log("snap");
    //    }
    //}
}
