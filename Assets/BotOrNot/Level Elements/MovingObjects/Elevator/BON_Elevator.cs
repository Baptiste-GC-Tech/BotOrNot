using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class BON_Elevator : BON_Actionnable
{
    /*
     *  FIELDS
    */
    [SerializeField] GameObject _elevator;
    [SerializeField] GameObject _elevatorPosition;
    [SerializeField] List<GameObject> _outTargetPosition;
    [SerializeField] Canvas _levelHUD;
    [SerializeField] float _playerSpeed;
    Button[] _buttonsInHUD;
    GameObject _player;
    bool _isPlayerMoving;
    bool _isElevatorMoving;
    bool _isMovingOut;
    bool _previousStatus;
    bool _elevatorHasAlredyMoved;
    bool _elevatorNeedsToChangePosition;
    bool _elevatorHasFinishedChangingPosition;



    /*
     *  UNITY METHODS
     */
    void Start()
    {
        _buttonsInHUD = _levelHUD.GetComponentsInChildren<Button>();
        _player = GameObject.FindFirstObjectByType<BON_CCPlayer>().gameObject;
        _isPlayerMoving = false;
        _isElevatorMoving = false;
        _isMovingOut = false;
        _previousStatus = Status;
        _elevatorHasAlredyMoved = false;
        _elevatorNeedsToChangePosition = false;
        _elevatorHasFinishedChangingPosition = false;
    }
    private void Update()
    {
        if (_previousStatus != base.Status)
        {
            
            Status = base.Status;
            _previousStatus = base.Status;
        }
        if (_elevatorNeedsToChangePosition)
        {
            _elevator.GetComponent<BON_MovObj_ListBased>().Toggle();
            _elevatorNeedsToChangePosition = false;
            _elevatorHasFinishedChangingPosition = true;
        }
        if (!_elevator.GetComponent<BON_MovObj_ListBased>().Status && _elevatorHasFinishedChangingPosition)
        {
            _isPlayerMoving = true;
            _elevatorHasFinishedChangingPosition = false;
        }
        if (_isPlayerMoving)
        {
            float step = _playerSpeed * Time.deltaTime;
            _player.transform.position = Vector3.MoveTowards(_player.transform.position, _elevatorPosition.transform.position, step);

            if (Vector3.Distance(_player.transform.position, _elevatorPosition.transform.position) < 0.5f)
            {
                _isPlayerMoving = false;
                _isElevatorMoving = true;
            }
        }
        if (_isElevatorMoving)
        {
            //move the elevator
            _elevator.GetComponent<BON_MovObj_ListBased>().Toggle();
            _isElevatorMoving = false;
            _elevatorHasAlredyMoved = true;
        }
        if (!_elevator.GetComponent<BON_MovObj_ListBased>().Status && _elevatorHasAlredyMoved)
        {
            _isMovingOut = true;
            _elevatorHasAlredyMoved = false;
        }
        if (_isMovingOut)
        {
            float step = _playerSpeed * Time.deltaTime;
            int count = 1;
            if (_elevator.GetComponent<BON_MovObj_ListBased>().IsCyclingPositive)
            {
                count = 0;
            }
            _player.transform.position = Vector3.MoveTowards(_player.transform.position, _outTargetPosition[count].transform.position, step);
            if (Vector3.Distance(_player.transform.position, _outTargetPosition[count].transform.position) < 0.5f)
            {
                _isMovingOut = false;
                base.Toggle();
            }

        }
    }

    /*
    *  METHODS
    */

    public override void On()
    {
        Debug.Log("Enter On");
        foreach (var button in _buttonsInHUD)
        {
            button.interactable = false;
        }
        if (_elevator.transform.position.y - 1 > gameObject.transform.position.y || _elevator.transform.position.y + 1 < gameObject.transform.position.y)
        {
            _elevatorNeedsToChangePosition = true;
        }
        else
        {        
            _isPlayerMoving = true;
        }
    }

    public override void Off()
    {
        foreach (var button in _buttonsInHUD)
        {
            button.interactable = true;
        }
    }

}
