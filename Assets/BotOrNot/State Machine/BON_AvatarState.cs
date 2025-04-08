using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "State/AvatarState")]
public abstract class BON_AvatarState : ScriptableObject
{
    /*
    *  FIELDS
    */

    /* bool for states */
    //bool isNut = false; //1 if nut, 0 if Dame robot => maybe in game manager ?

    bool _isGrounded = false;  //1 if touch the ground
    bool _isAgainstWall = false; //1 if touch a wall
    bool _isMoving = false; //1 if move, 0 if idle
    bool _isInElevator = false; //1 if in elevator

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

    protected abstract bool CheckStatePossible(States currentState); // <-- (eg robot cannot Jump, Dame robot cannot use cable)
                                                                    // see children

    public void ChangeState(States state)
    {
        if (_currentState != state && CheckStatePossible(state))
        {
            MonoBehaviour.print("changement effectué");
            _currentState = state;
        }
        else
        {
            MonoBehaviour.print("changement non validé");
        }
    }
/*
    private bool OnStateValueChanged(States state) //change bool
    {
        switch (state)
        {
            case States.Idle:
                _isJumping = false;
                _isMoving = false;
                return true;

            case States.Moving:
                _isMoving = true;
                return true;

            case States.Jump:
                _isJumping = true;
                return true;

            case States.Elevator:
                _isInElevator = true;
                return true;

            case States.ControllingMachine:
                if (_isNearIOMInteractible)
                {
                    _isJumping = false;
                    _isMoving = false;
                    _isGrounded = true;
                    return true;
                }
                else
                {
                    return false;
                }

            case States.ThrowingCable:
                if (_isNearCableInteractible)
                {
                    _isthrowingCable = true;
                    return true;
                }
                else
                {
                    return false;
                }

            default:
                return false;
        }
    }*/

    /*
     *  UNITY METHODS
     */
}
