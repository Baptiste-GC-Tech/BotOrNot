using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State/AvatarState")]
public class BON_AvatarState : ScriptableObject
{
    /*
    *  FIELDS
    */

    //link States enum to States class
    protected Dictionary<States, BON_State> _stateDict = new();

    /* bool for states */

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
    bool _isMoving = false; //1 if move, 0 if idle
    public bool IsMoving //for state
    {
        get { return _isMoving; }
        set { _isMoving = value; }
    }
    bool _isInElevator = false; //1 if in elevator
    public bool IsInElevator //for state -> need to setup
    {
        get { return _isInElevator; }
        set { _isInElevator = value; }
    }
    

    //bool PR
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

    bool _isthrowingCable = false; //1 if using cable, 0 if not for state
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
    bool _isNearHumanoidObject = false; //<-- (eg.échelle, ...) <- useless now ?
    public bool IsNearHumanoidObject
    {
        get { return _isNearHumanoidObject; }
        set { _isNearHumanoidObject = value; }
    }

    bool _isJumping = false; //1 if in air - for state
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

    private Collider _collider;

    //for isgrounded
    float distToGround;
    //for isagainstWall
    float distToWall;

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

    //protected abstract bool CheckStatePossible(States currentState); // <-- (eg robot cannot Jump, Dame robot cannot use cable)
    // see childre

    protected bool CheckStatePossible(States newState) // <-- (eg robot cannot Jump, Dame robot cannot use cable)
    {
        if (BON_GameManager.Instance().CurrentCharacterPlayed == 0) //PR
        {
            if (newState == States.Jump)
            {
                Debug.Log("état pas possible pour le robot");
                return false;
            }
        }
        else if (BON_GameManager.Instance().CurrentCharacterPlayed == 1) //DR
        {
            if (newState == States.ControllingMachine || newState == States.ThrowingCable)
            {
                MonoBehaviour.print("état pas possible pour la dame robot");
                return false;
            }
        }
        return true;
    }

    //public abstract void ChangeState(States state);

    public void ChangeState(States state)
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
        _stateDict.Add(States.Idle,new BON_SIdle());
        _stateDict.Add(States.Moving,new BON_SMoving());
        _stateDict.Add(States.Jump,new BON_SJump());
        _stateDict.Add(States.ControllingMachine,new BON_SControllingMachine());
        _stateDict.Add(States.ThrowingCable,new BON_SThrowingCable());
        _stateDict.Add(States.Elevator,new BON_SElevator());

        _player = GameObject.FindFirstObjectByType<BON_CCPlayer>();
        _collider = _player.GetComponent<Collider>();
        distToGround = _collider.bounds.extents.y;
        distToWall = _collider.bounds.extents.x;

        _currentState =  BON_AvatarState.States.Idle;
        _currentStateAsset = _stateDict[BON_AvatarState.States.Idle];
        _currentStateAsset?.InitPlayer(_player); //set le player
        _currentStateAsset?.Enter();
    }

    public void UpdateState() //update state and mains bools
    {
        _currentStateAsset?.UpState();
        Isgrounded();
        IsagainstWall();
    }

    public void Isgrounded() //check if there ground under
    {
        //Debug.DrawRay(_player.transform.position, -Vector3.up, Color.red);

        _isGrounded = Physics.Raycast(_player.transform.position, -Vector3.up, distToGround + 0.5f);
    }

    private void IsagainstWall() //check if there wall forward
    {
        //Debug.DrawRay(_player.transform.position, Vector3.left, Color.blue);
        //Debug.DrawRay(_player.transform.position, Vector3.right, Color.blue);

        _isAgainstWall = (Physics.Raycast(_player.transform.position, Vector3.right, distToWall + 0.2f) || Physics.Raycast(_player.transform.position, Vector3.left, distToWall + 0.2f));
    }
}
