using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public abstract class BON_AvatarState : ScriptableSingleton<BON_AvatarState>
{
    /*
    *  FIELDS
    */

    // * = dame robot
    // ° = petit robot

    /* bool for states */
    bool isNut = false; //1 if nut, 0 if Dame robot => maybe in game manager ?

    bool _isGrounded = false;  //1 if touch the ground
    bool _isJumping = false; //1 if in air *
    bool _isAgainstWall = false; //1 if touch a wall
    bool _isMoving = false; //1 if move, 0 if idle
    bool _isInElevator = false; //1 if in elevator

    bool _isNearHumanoidObject = false; //<-- (eg.échelle, ...) *
    public bool IsNearHumanoidObject
    {
        get { return _isNearHumanoidObject; }
        set { _isNearHumanoidObject = value; }
    }

    bool _isNearIOMInteractible = false; //<-- Terminaux, grue, ... °
    public bool IsNearIOMInteractible
    {
        get { return _isNearIOMInteractible; }
        set { _isNearIOMInteractible = value; }
    }
    bool _isConstrollingMachine = false; //1 if using machine, 0 if not °
    public bool IsConstrollingMachine
    {
        get { return _isConstrollingMachine; }
        set { _isConstrollingMachine = value; }
    }
    bool _isNearCableInteractible = false; // °
    public bool IsNearCableInteractible
    {
        get { return _isNearCableInteractible; }
        set { _isNearCableInteractible = value; }
    }
    bool _isthrowingCable = false; //1 if using cable, 0 if not °
    public bool IsthrowingCable
    {
        get { return _isthrowingCable; }
        set { _isthrowingCable = value; }
    }

    bool _isNearItem = false; //<-- Truc pour l’inventaire °
    public bool IsNearItem
    {
        get { return _isNearIOMInteractible; }
        set { _isNearItem = value; }
    }

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
    public static BON_AvatarState Instance()
    {
        return instance;
    }

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
            if (OnStateValueChanged(state))
            {
                MonoBehaviour.print("changement effectué");
                _currentState = state;
            }
            else
            {
                MonoBehaviour.print("changement non validé");
            }
        }
        else
        {
            MonoBehaviour.print("changement non validé");
        }
    }

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
    }

    /*
     *  UNITY METHODS
     */

    private void Awake()
    {
        ScriptableObject relut = CreateInstance("BON_AvatarState");
        MonoBehaviour.print(relut);
    }
}
