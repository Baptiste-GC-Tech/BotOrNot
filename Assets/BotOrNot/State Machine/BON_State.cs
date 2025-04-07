using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BON_State : ScriptableObject
{
    /*
     *  FIELDS
     */

    //avatarState reference
    protected BON_AvatarState _AvatarState;


    /*
     *  CLASS METHODS
     */

    public virtual void Enter()
    {
        //setbool true in controller
        //start anim
    }

    public virtual void Update()
    {
        //update state
    }

    public virtual void Exit()
    {
        //setbool false in controller
    }


    /*
     *  UNITY METHODS
     */
    void Start()
    {
        _AvatarState = this.GetComponent<BON_AvatarState>();
        if (_AvatarState != null)
        {
            MonoBehaviour.print("oui");
        }
    }
}
