using System;
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

    bool _isGrounded = false;  //1 if touch the ground
    public bool IssGrounded
    {
        get { return _isGrounded; }
        set { _isGrounded = value; }
    }
    bool _isAgainstWall = false; //1 if touch a wall
    public bool IsAgainstWall
    {
        get { return _isAgainstWall; }
        set { _isAgainstWall = value; }
    }
    bool _isMoving = false; //1 if move, 0 if idle
    public bool IsMoving
    {
        get { return _isMoving; }
        set { _isMoving = value; }
    }
    bool _isInElevator = false; //1 if in elevator
    public bool IsInElevator
    {
        get { return _isInElevator; }
        set { _isInElevator = value; }
    }

    //bool PR
    bool _isNearIOMInteractible = false; //<-- Terminaux, grue, ...
    public bool IsNearIOMInteractible
    {
        get { return _isNearIOMInteractible; }
        set { _isNearIOMInteractible = value; }
    }


    bool _isConstrollingMachine = false; //1 if using machine, 0 if not 
    public bool IsConstrollingMachine
    {
        get { return _isConstrollingMachine; }
        set { _isConstrollingMachine = value; }
    }

    bool _isNearCableInteractible = false;
    public bool IsNearCableInteractible
    {
        get { return _isNearCableInteractible; }
        set { _isNearCableInteractible = value; }
    }

    bool _isthrowingCable = false; //1 if using cable, 0 if not 
    public bool IsthrowingCable
    {
        get { return _isthrowingCable; }
        set { _isthrowingCable = value; }
    }

    bool _isNearItem = false; //<-- Truc pour l’inventaire 
    public bool IsNearItem
    {
        get { return _isNearItem; }
        set { _isNearItem = value; }
    }

    //bool DR
    bool _isNearHumanoidObject = false; //<-- (eg.échelle, ...) 
    public bool IsNearHumanoidObject
    {
        get { return _isNearHumanoidObject; }
        set { _isNearHumanoidObject = value; }
    }
    bool _isJumping = false; //1 if in air 
    public bool IsJumping
    {
        get { return _isJumping; }
        set { _isJumping = value; }
    }

    protected States _currentState;

    public enum States
    {
        Idle,
        Moving, //moving in ground
        Jump, // in air 
        Elevator, 
        ControllingMachine,
        ThrowingCable,
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
}
