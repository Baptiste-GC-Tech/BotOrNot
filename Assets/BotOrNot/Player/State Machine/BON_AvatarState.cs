using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "State/AvatarState")]
public class BON_AvatarState : ScriptableObject
{
    /*
    *  FIELDS
    */


    //link State enum to State class
    protected Dictionary<State, BON_State> _stateDict = new();

    /*  
    *  Animator Controller
    */

    private Animator _animator;

    /*
     *  Booleans for Common State (both PR and DR)
     */

    bool _wasGroundedLastFrame;
    public bool WasGroundedLastFrame
    {
        get { return _wasGroundedLastFrame; }
    }
    bool _isGrounded = false;  //1 if touch the ground 
    public bool IsGrounded
    {
        get { return _isGrounded; }
        set
        {
            _wasGroundedLastFrame = _isGrounded;
            _isGrounded = value;
        }
    }

    bool _isAgainstWallLeft = false; //1 if touch a wall on left
    public bool IsAgainstWallLeft
    {
        get { return _isAgainstWallLeft; }
        set { _isAgainstWallLeft = value; }
    }

    bool _isAgainstWallRight = false; //1 if touch a wall on left
    public bool IsAgainstWallRight
    {
        get { return _isAgainstWallRight; }
        set { _isAgainstWallRight = value; }
    }


    // New boolean to avoid some unwanted transitions
    bool _isIdling = true;
    public bool IsIdling
    {
        get { return _isIdling; }
        set { _isIdling = value; }
    }


    bool _isMovingByPlayer = false; //1 if move, 0 if idle
    public bool IsMovingByPlayer //for state
    {
        get { return _isMovingByPlayer; }
        set { _isMovingByPlayer = value; }
    }
    bool _isInElevator = false; //1 if in elevator
    public bool IsInElevator //for state -> need to setup
    {
        get { return _isInElevator; }
        set { _isInElevator = value; }
    }
    bool _isDrifting = false;
    public bool IsDrifting
    {
        get { return _isDrifting; }
        set { _isDrifting = value; }

    }
    bool _isNearElevator = false; //1 if is near elevator
    public bool IsNearElevator 
    {
        get { return _isNearElevator; }
        set { _isNearElevator = value; }
    }


    /*
     *  Booleans exclusive to PR
     */

    bool _isNearIOMInteractible = false; //<-- Terminaux, grue, ... for switching player/machine
    public bool IsNearIOMInteractible
    {
        get { return _isNearIOMInteractible; }
        set { _isNearIOMInteractible = value; }
    }

    bool _isConstrollingMachine = false; //1 if using machine, 0 if not -> for machine & state
    public bool IsConstrollingMachine
    {
        get { return _isConstrollingMachine; }
        set { _isConstrollingMachine = value; }
    }

    bool _hasCableOut = false; //1 if using cable, 0 if not for state
    public bool HasCableOut
    {
        get { return _hasCableOut; }
        set
        {
            _hasCableOut = value;
        }
    }


    /*
     *  Booleans exclusive to DR
     */

    bool _isNearHumanoidObject = false; //<-- (eg.�chelle, ...) <- useless now ?
    public bool IsNearHumanoidObject
    {
        get { return _isNearHumanoidObject; }
        set { _isNearHumanoidObject = value; }
    }

    protected State _currentState;
    public State CurrentState
    {
        get { return _currentState; }
    }

    // player reference
    private BON_CCPlayer _player;
    protected BON_State _currentStateAsset;

    public enum State
    {
        Idle,
        Moving,
        Jump, 
        Elevator, 
        ControllingMachine,
        ThrowingCable,
        Drift
        //Grounded
    };
    

    /*
     *  CLASS METHODS
     */

    protected void SetState(State state)
    {
        _currentStateAsset?.Exit();      // Quitter l�ancien �tat
        _currentState = state;
        _currentStateAsset = _stateDict[state];
        _currentStateAsset?.InitPlayer(_player); //set le player
        _currentStateAsset?.Enter();    // Entrer dans le nouveau
        UpdateAnimator(state);
    }

    private void UpdateAnimator(State state)
    {
        if (_animator == null) return;

        _animator.SetBool("IsIdle", state == State.Idle);
        _animator.SetBool("IsMoving", state == State.Moving);
        _animator.SetBool("IsJumping", state == State.Jump);
        _animator.SetBool("IsControllingMachine", state == State.ControllingMachine);
        _animator.SetBool("IsInElevator", state == State.Elevator);
        _animator.SetBool("IsThrowingCable", state == State.ThrowingCable);
        _animator.SetBool("StoppedAbruptly", state == State.Drift);
        _animator.SetBool("DirectionChangedQuickly", state == State.Drift);

        _animator.SetBool("IsGrounded", _isGrounded);
        _animator.SetBool("IsAgainstWallLeft", _isAgainstWallLeft);
        _animator.SetBool("IsAgainstWallRight", _isAgainstWallRight);
        _animator.SetBool("IsNearIOMInteractible", _isNearIOMInteractible);
        _animator.SetBool("IsInElevatorZone", _isInElevator);
    }



    protected bool CheckStatePossible(State newState) // <-- (eg robot cannot Jump, Dame robot cannot use cable)
    {
        if (BON_GameManager.Instance().CurrentCharacterPlayed == 0) //PR
        {
            if (newState == State.Jump)
            {
                Debug.Log("�tat pas possible pour le robot");
                return false;
            }
        }
        else if (BON_GameManager.Instance().CurrentCharacterPlayed == 1) //DR
        {
            if (newState == State.ControllingMachine || newState == State.ThrowingCable)
            {
                MonoBehaviour.print("�tat pas possible pour la dame robot");
                return false;
            }
        }
        return true;
    }

    public void ChangeState(State state)
    {
        if (_currentState != state && CheckStatePossible(state))
        {
            SetState(state);
        }
        else
        {
            Debug.Log("changement non valid�");
        }
    }

    public void Init()
    {
        _player = GameObject.FindFirstObjectByType<BON_CCPlayer>();

        if (_player == null)
        {
            Debug.LogError("BON_AvatarState.Init() : BON_CCPlayer introuvable !");
            return;
        }

        // find Animator in children
        _animator = _player.GetComponentInChildren<Animator>();
        if (_animator == null)
        {
            Debug.LogWarning("BON_AvatarState.Init() : Animator non trouv� dans les enfants de BON_CCPlayer.");
        }


        // Initialize states
        _stateDict.Add(State.Idle, new BON_SIdle());
        _stateDict.Add(State.Moving, new BON_SMoving());
        _stateDict.Add(State.Jump, new BON_SJump());
        _stateDict.Add(State.ControllingMachine, new BON_SControllingMachine());
        _stateDict.Add(State.ThrowingCable, new BON_SThrowingCable());
        _stateDict.Add(State.Elevator, new BON_SElevator());
        _stateDict.Add(State.Drift, new BON_SDrift());

        // initial state
        _currentState = State.Idle;
        _currentStateAsset = _stateDict[_currentState];
        _currentStateAsset?.InitPlayer(_player);
        _currentStateAsset?.Enter();
    }


    public void UpdateState() //update state and mains bools
    {
        _currentStateAsset?.UpState();
    }
}
