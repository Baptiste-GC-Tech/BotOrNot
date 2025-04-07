using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BON_AvatarState
{
    /*
    *  FIELDS
    */

    // * = dame robot
    // ° = petit robot

    /* bool for states */
    bool isNut; //1 if nut, 0 if Dame robot => maybe in game manager ?

    bool isGrounded;  //1 if touch the ground
    bool isJumping; //1 if in air *
    bool isAgainstWall; //1 if touch a wall
    bool isMoving; //1 if move, 0 if idle
    bool isInElevator; //1 if in elevator

    bool isNearHumanoidObject; //<-- (eg.échelle, ...) *

    bool isNearIOMInteractible; //<-- Terminaux, grue, ... °
    bool isConstrollingMachine; //1 if using machine, 0 if not °
    bool isNearCableInteractible; // °
    bool isthrowingCable; //1 if using cable, 0 if not °

    bool isNearItem; //<-- Truc pour l’inventaire °


    protected States _currentState;

    public enum States
    {
        Idle,
        Moving, //moving in ground
        Jump, // in air *
        Elevator, 
        ControllingMachine, // °
        ThrowingCable, // °
    };

    /*
    *  CLASS METHODS
    */

    public void InitState(States currentState)
    {
        _currentState = currentState;
    }

    protected abstract bool CheckStatePossible(States currentState); // <-- (eg robot cannot Jump, Dame robot cannot use cable) see children

    /*
    protected virtual void CheckSwitchState(States currentState) 
    {
         * 
         * get player speed
         * if speed != 0
         * state = moving
         * 
         * NON => plutot l'inverse, au moment de l'input -> switch state (evite de check chaque frame + appeller 500 fonctions et vars
         * 
    }
    */

    public void ChangeState(States state)
    {
        if (_currentState != state && CheckStatePossible(state))
        {
            _currentState = state;
            MonoBehaviour.print("changement effectué");
        }
        else
        {
            MonoBehaviour.print("changement non validé");
        }
    }

    /*
    private void OnStateValueChanged(States state)
    {
        switch (state)
        {
            case States.Idle:
                //update ?
                break;
            case States.Grounded:
               //
                break;
            case States.Jump:
                //
                break;
            case States.Elevator:
                //
                break;
            case States.ControllingMachine:
                //
                break;
            case States.ThrowingCable:
                //
                break;
        }
    }*/
}
