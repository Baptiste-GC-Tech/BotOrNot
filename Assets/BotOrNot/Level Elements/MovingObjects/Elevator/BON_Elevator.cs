using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.RuleTile.TilingRuleOutput;


public class BON_Elevator : BON_Actionnable
{
    /*
     *  FIELDS
    */
    [SerializeField] bool _elevatorStatus;                  //Set the Actionnable of the elevator button

    [SerializeField] bool _triggerTheElevator;              //Set the Actionnable of the actual elevator (moving platform)

    [SerializeField] GameObject _elevator;                  //Gamrobject of the elevator
    [SerializeField] GameObject _elevatorPosition;          //The position that the player needs to reach to be on the elevator
    [SerializeField] GameObject _outTargetPosition;   //The positions that the player needs to reach to get out of the elevator 
    [SerializeField] Canvas _levelHUD;                      //A reference to the canvas to be able to disable the controls of the player
    [SerializeField] float _playerSpeed;                    //Refecrence to the player speed (right now it's set in the editor but might need to get the speed from the player's script)         
    GameObject _player;                     
    bool _isPlayerMoving;                                   //is the player moving to the elevator
    bool _isElevatorMoving;                                 //is the elevator movnig to it's final destination
    bool _IsMovingByPlayerOut;                                      //is the player moving out of the elevator
    bool _previousStatus;                                   //allows us to change the status of the actionable without calling it every frame
    bool _elevatorHasAlredyMoved;                           //check if the elevator has reach the final destination
    bool _elevatorNeedsToChangePosition;                    //if the elevator is not on the same level as the actionnable, changes it's position
    bool _elevatorHasFinishedChangingPosition;              //check if the elevator has reach the destination of the actionnable
    Animator _playerAnimator;


    /*
    *  METHODS
    */

    public override void On()
    {
        //disable the controls
        _levelHUD.GetComponentInChildren<BON_COMPJoystick>().ComponentToggle();
        _levelHUD.GetComponentInChildren<BON_COMPPlayerButtons>().ComponentToggle();
        //if the elevator is not on the same floor as the actionable changes it's position
        if (_elevator.transform.position.y - 1 > gameObject.transform.position.y || _elevator.transform.position.y + 1 < gameObject.transform.position.y)
        {
            _elevatorNeedsToChangePosition = true;
        }
        //otherwise starts to move the player to the elevator
        else
        {
            _isPlayerMoving = true;
        }
    }

    public override void Off()
    {
        //enable the controls
        _levelHUD.GetComponentInChildren<BON_COMPJoystick>().ComponentToggle();
        _levelHUD.GetComponentInChildren<BON_COMPPlayerButtons>().ComponentToggle();
    }


    /*
     *  UNITY METHODS
     */
    void Start()
    {
        _player = GameObject.FindFirstObjectByType<BON_CCPlayer>().gameObject;
        _isPlayerMoving = false;
        _isElevatorMoving = false;
        _IsMovingByPlayerOut = false;
        _previousStatus = Status;
        _elevatorHasAlredyMoved = false;
        _elevatorNeedsToChangePosition = false;
        _elevatorHasFinishedChangingPosition = false;
        _playerAnimator = _player.GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        //check for a status chang
        if (_previousStatus != _elevatorStatus)
        { 
            Status = _elevatorStatus;
            _previousStatus = _elevatorStatus;
        }

        //change the status of the elevator
        if (_triggerTheElevator)
        {
            _elevator.GetComponent<BON_MovObj_ListBased>().Toggle();
            _triggerTheElevator = false;
        }

        //if the elevator is not on the same level as the actionnable, changes it's position
        if (_elevatorNeedsToChangePosition)
        {
            _elevator.GetComponent<BON_MovObj_ListBased>().Toggle();
            _elevatorNeedsToChangePosition = false;
            _elevatorHasFinishedChangingPosition = true;
        }
        //check if the elevator is done moving
        if (!_elevator.GetComponent<BON_MovObj_ListBased>().Status && _elevatorHasFinishedChangingPosition)
        {
            _isPlayerMoving = true;
            _elevatorHasFinishedChangingPosition = false;

            _player.transform.eulerAngles = (_player.transform.position.x > _elevatorPosition.transform.position.x)? new Vector3(0, -90, 0): new Vector3(0, 90, 0);

            _playerAnimator.SetBool("IsIdle", false);
            _playerAnimator.SetBool("IsMoving", true);
        }

        //moves the player to the elevator
        if (_isPlayerMoving)
        {
            float step = _playerSpeed * Time.deltaTime;
            _player.transform.position = Vector3.MoveTowards(_player.transform.position, _elevatorPosition.transform.position, step);
            //if the player is close enough, they stop moving
            if (Vector3.Distance(_player.transform.position, _elevatorPosition.transform.position) < 0.5f)
            {
                _isPlayerMoving = false;
                _isElevatorMoving = true;
                _player.transform.parent = _elevator.transform;
                _playerAnimator.SetBool("IsIdle", true);
                _playerAnimator.SetBool("IsMoving", false);
            }
        }
        if (_isElevatorMoving)
        {
            //move the elevator
            _elevator.GetComponent<BON_MovObj_ListBased>().Toggle();
            _isElevatorMoving = false;
            _elevatorHasAlredyMoved = true;
        }
        //stop the motion of the elevator and start the motion of the player
        if (!_elevator.GetComponent<BON_MovObj_ListBased>().Status && _elevatorHasAlredyMoved)
        {
            _IsMovingByPlayerOut = true;
            _elevatorHasAlredyMoved = false;
            _player.transform.parent = null;


            _playerAnimator.SetBool("IsIdle", false);
            _playerAnimator.SetBool("IsMoving", true);
        }
        //moves the player out of the elevator
        if (_IsMovingByPlayerOut)
        {
            float step = _playerSpeed * Time.deltaTime;
            //if the elevator cycling of the elevator is false (i.e. the elevator is up), player moves to the second location (count = 1)
            _player.transform.eulerAngles = (_player.transform.position.x > _outTargetPosition.transform.position.x) ? new Vector3(0, -90, 0) : new Vector3(0, 90, 0);
            //if the player is close to the final postion, stops their motion and changes the status of the Bon_Elevator)
            _player.transform.position = Vector3.MoveTowards(_player.transform.position, _outTargetPosition.transform.position, step);
            if (Vector3.Distance(_player.transform.position, _outTargetPosition.transform.position) < 0.5f)
            {
                _IsMovingByPlayerOut = false;
                _elevatorStatus = false;
                _playerAnimator.SetBool("IsIdle", true);
                _playerAnimator.SetBool("IsMoving", false);
                base.Toggle();
            }
        }
    }
}
