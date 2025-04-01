using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class BON_Elevator : BON_Actionnable
{
    [SerializeField] GameObject _elevatorPosition;
    [SerializeField] Canvas _levelHUD;
    [SerializeField] float _playerSpeed;
    List<Button> _buttonsInHUD;
    GameObject _player;
    bool _isPlayerMoving;
    bool _isElevatorMoving;
    void Start()
    {
        _buttonsInHUD.Add(_levelHUD.GetComponentInChildren<Button>());
        _player = GameObject.FindFirstObjectByType<Player>().gameObject;
        _isPlayerMoving = false;
        _isElevatorMoving = false;
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
    }
}
