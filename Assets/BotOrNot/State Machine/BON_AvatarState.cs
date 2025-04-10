using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State/AvatarState")]
public abstract class BON_AvatarState : ScriptableObject
{
    /*
    *  FIELDS
    */

    //link States class and enum
    protected Dictionary<States, BON_State> _stateDict = new();

    /* bool for states */

    bool _isGrounded = false;  //1 if touch the ground
    public bool IsGrounded
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
    public States CurrentState
    {
        get { return _currentState; }
    }

    protected BON_State _currentStateAsset;

    // player reference
    private BON_CCPlayer _player;

    public enum States
    {
        Idle,
        Moving,
        Jump, 
        Elevator, 
        ControllingMachine,
        ThrowingCable,
    };

    /*
    *  CLASS METHODS
    */


    protected void SetState(States state)
    {
        _currentStateAsset?.Exit();      // Quitter l’ancien état
        _currentState = state;
        _currentStateAsset = _stateDict[state];
        _currentStateAsset?.InitPlayer(_player); //set le player
        _currentStateAsset?.Enter();    // Entrer dans le nouveau
    }

    protected abstract bool CheckStatePossible(States currentState); // <-- (eg robot cannot Jump, Dame robot cannot use cable)
                                                                     // see childre
    public abstract void ChangeState(States state);

    public void Init()
    {
        _stateDict.Add(States.Idle,new BON_SIdle());
        _stateDict.Add(States.Moving,new BON_SMoving());
        _stateDict.Add(States.Jump,new BON_SJump());
        _stateDict.Add(States.ControllingMachine,new BON_SControllingMachine());
        _stateDict.Add(States.ThrowingCable,new BON_SThrowingCable());
        _stateDict.Add(States.Elevator,new BON_SElevator());

        _player = GameObject.FindFirstObjectByType<BON_CCPlayer>();

        _currentState =  BON_AvatarState.States.Idle;
        _currentStateAsset = _stateDict[BON_AvatarState.States.Idle];
        _currentStateAsset?.InitPlayer(_player); //set le player
        _currentStateAsset?.Enter();
    }

    public void UpdateState()
    {
        //MonoBehaviour.print("update state");
        _currentStateAsset?.UpState();
    }
}
