using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_Jump : BON_State
{
    /*
    *  CLASS METHODS
    */

    public override void Enter()
    {
        //setbool true in controller
        //start anim
        //disable jump action ( sauter au sol 1fois, pas plus)
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
