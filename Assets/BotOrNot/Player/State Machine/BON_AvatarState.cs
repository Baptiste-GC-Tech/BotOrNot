using System.Collections.Generic;
using Unity.Burst.Intrinsics;
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
     *  Booleans for Common State (both PR and DR)
     */

    bool _isGrounded = false;  //1 if touch the ground 
    public bool IsGrounded //for jump -> need to setup
    {
        get { return _isGrounded; }
        set { _isGrounded = value; }
    }
    bool _isAgainstWall = false; //1 if touch a wall
    public bool IsAgainstWall //  -> need to setup
    {
        get { return _isAgainstWall; }
        set { _isAgainstWall = value; }
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
        set { _hasCableOut = value; }
    }

    //bool _isNearItem = false; //<-- Truc pour l’inventaire 
    //public bool IsNearItem
    //{
    //    get { return _isNearItem; }
    //    set { _isNearItem = value; }
    //}


    /*
     *  Booleans exclusive to PR
     */

    bool _isNearHumanoidObject = false; //<-- (eg.échelle, ...) <- useless now ?
    public bool IsNearHumanoidObject
    {
        get { return _isNearHumanoidObject; }
        set { _isNearHumanoidObject = value; }
    }

    //bool _isJumping = false; //1 if in air - for state
    //public bool IsJumping
    //{
    //    get { return _isJumping; }
    //    set { _isJumping = value; }
    //}


    protected State _currentState;
    public State CurrentState
    {
        get { return _currentState; }
    }


    // player reference
    private BON_CCPlayer _player;
    protected BON_State _currentStateAsset;

    private Collider _playerCollider;

    //for isgrounded
    float distToGround;
    //for isagainstWall
    float distToWall;

    public enum State
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

    protected void SetState(State state)
    {
        _currentStateAsset?.Exit();      // Quitter l’ancien état
        _currentState = state;
        _currentStateAsset = _stateDict[state];
        _currentStateAsset?.InitPlayer(_player); //set le player
        _currentStateAsset?.Enter();    // Entrer dans le nouveau
    }

    protected bool CheckStatePossible(State newState) // <-- (eg robot cannot Jump, Dame robot cannot use cable)
    {
        if (BON_GameManager.Instance().CurrentCharacterPlayed == 0) //PR
        {
            if (newState == State.Jump)
            {
                Debug.Log("état pas possible pour le robot");
                return false;
            }
        }
        else if (BON_GameManager.Instance().CurrentCharacterPlayed == 1) //DR
        {
            if (newState == State.ControllingMachine || newState == State.ThrowingCable)
            {
                MonoBehaviour.print("état pas possible pour la dame robot");
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
            Debug.Log("changement non validé");
        }
    }

    public void Init()
    {
        _stateDict.Add(State.Idle,new BON_SIdle());
        _stateDict.Add(State.Moving,new BON_SMoving());
        _stateDict.Add(State.Jump,new BON_SJump());
        _stateDict.Add(State.ControllingMachine,new BON_SControllingMachine());
        _stateDict.Add(State.ThrowingCable,new BON_SThrowingCable());
        _stateDict.Add(State.Elevator,new BON_SElevator());

        _player = GameObject.FindFirstObjectByType<BON_CCPlayer>();
        _playerCollider = _player.GetComponent<Collider>();
        distToGround = _playerCollider.bounds.extents.y;
        distToWall = _playerCollider.bounds.extents.x;

        _currentState =  BON_AvatarState.State.Idle;
        _currentStateAsset = _stateDict[BON_AvatarState.State.Idle];
        _currentStateAsset?.InitPlayer(_player); //set le player
        _currentStateAsset?.Enter();
    }

    public void UpdateState() //update state and mains bools
    {
        _currentStateAsset?.UpState();
        UpdateGroundedBool();
        UpdateAgainstWallBool();
    }

    public void UpdateGroundedBool() //check if there ground under
    {
        //Debug.DrawRay(_player.transform.position, -Vector3.up, Color.red);

        _isGrounded = Physics.Raycast(_player.transform.position, -Vector3.up, distToGround + 0.5f);
    }

    private void UpdateAgainstWallBool() //check if there wall forward
    {
        //Debug.DrawRay(_player.transform.position, Vector3.left, Color.blue);
        //Debug.DrawRay(_player.transform.position, Vector3.right, Color.blue);

        _isAgainstWall = (Physics.Raycast(_player.transform.position, Vector3.right, distToWall + 0.2f) || Physics.Raycast(_player.transform.position, Vector3.left, distToWall + 0.2f));
    }
}
