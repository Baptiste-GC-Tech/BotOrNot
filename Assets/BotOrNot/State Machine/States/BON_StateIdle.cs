using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_Idle : BON_State
{
    /*
    *  CLASS METHODS
    */

    public override void Enter()
    {
        //setbool true in controller
        //start anim
    }

    public override void Update()
    {
        //si pas de sol => tombe
    }
    public override void Exit()
    {
        //setbool false in controller
    }
}
