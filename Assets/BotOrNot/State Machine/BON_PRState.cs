using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using static BON_GameManager;
using static UnityEditor.Experimental.GraphView.GraphView;

[CreateAssetMenu(menuName = "State/PRState")]
public class BON_PRState : BON_AvatarState
{
    /*
    *  CLASS METHODS
    */

    protected override bool CheckStatePossible(States newState) // <-- (eg robot cannot Jump, Dame robot cannot use cable)
    {
        if (newState == States.Jump)
        {
            MonoBehaviour.print("état pas possible pour le robot");
            return false;
        }
        return true;
    }

    public override void ChangeState(States state)
    {
        //MonoBehaviour.print("current state =" + _currentState);
        if (_currentState != state && CheckStatePossible(state))
        {
            //MonoBehaviour.print("changement effectué");
            SetState(state);
        }
        else
        {
            MonoBehaviour.print("changement non validé");
        }
    }
}
