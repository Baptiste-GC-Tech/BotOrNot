using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_GroundSnapPR : MonoBehaviour
{
    /*
     *  FIELDS
     */
    private BON_AvatarState _avatarState;
    private BON_Move _moveComp;

    [SerializeField] Collider _groundSnapCollider;


    /*
     *  UNITY METHODS
     */
    private void Start()
    {
        /* Fields init */
        _avatarState = GetComponent<BON_AvatarState>();
        _moveComp = GetComponent<BON_Move>();
    }

    private void Update()
    {
        
    }
}
