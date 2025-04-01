using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class BON_Elevator : BON_Actionnable
{
    [SerializeField] GameObject _elevator;
    [SerializeField] GameObject _elevatorPosition;
    [SerializeField] Canvas _levelHUD;
    [SerializeField] float _playerSpeed;
    Button[] _buttonsInHUD;
    GameObject _player;
    bool _isPlayerMoving;
    bool _isElevatorMoving;
    void Start()
    {
        _buttonsInHUD = _levelHUD.GetComponentsInChildren<Button>();
        _player = GameObject.FindFirstObjectByType<Player>().gameObject;
        _isPlayerMoving = false;
        _isElevatorMoving = false;
        base.Status = true;
    }

    public override void On()
    {
        foreach (var button in _buttonsInHUD)
        {
            button.interactable = false;
        }
        _isPlayerMoving = true;
    }

    public override void Off()
    {
        foreach (var button in _buttonsInHUD)
        {
            button.interactable = true;
        }
    }
    private void Update()
    {
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
            Debug.Log(_elevator.GetComponent<BON_MovObj_ListBased>().Status);
            _elevator.GetComponent<BON_MovObj_ListBased>().Toggle();
            Debug.Log(_elevator.GetComponent<BON_MovObj_ListBased>().Status);
            _isElevatorMoving = false;
            base.Toggle();
        }
    }
}
