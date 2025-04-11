using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BON_Controllable : BON_Actionnable
{
    /*
     *  CLASS METHODS
     */

    public override void On()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerInput>().SwitchCurrentActionMap("MachineControl");
    }

    public override void Off()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerInput>().SwitchCurrentActionMap("ActionsMapPR");
    }

    public virtual void ProcessInput(Vector2 Input)
    {

    }
}
