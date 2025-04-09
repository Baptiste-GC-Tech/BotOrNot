using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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
        MonoBehaviour.print("check state PR");
        if (newState ==  States.Jump)
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
            //SetState(state);
            MonoBehaviour.print("changement effectué");
            _currentState = state;
        }
        else
        {
            MonoBehaviour.print("changement non validé");
        }
    }

    /*// Update is called once per frame
    void Update()
    {
        if (BON_GameManager.Instance().IsPlayingNut == true) //test current state
        {
            if (!CheckStatePossible(_currentState))
            {
                InitState(States.Idle);
            }
        }
    }*/
/*
    void Start()
    {
        // Lier chaque état à sa classe spécifique
        _stateDict = new Dictionary<States, BON_State>
        {
            { States.Idle, new BON_SIdle() },
            { States.Moving, new BON_SMoving() },
            { States.Jump, new BON_SJump() },
            { States.ThrowingCable, new BON_SThrowingCable() },
            { States.ControllingMachine, new BON_SControllingMachine() },
            { States.Elevator, new BON_SElevator() },
        };

    }*/
}
