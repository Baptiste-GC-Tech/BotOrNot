using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class BON_FreeMovementCrane : BON_Actionnable
{
    /*
     *  FIELDS
     */
    private bool _isMoving;
    [SerializeField] float _speedMax;
    private float _speed;
    private float _acceleration;
    private Vector3 _direction;


    /*
     *  CLASS METHODS
     */
    public override void On()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerInput>().SwitchCurrentActionMap("MachineControl");
    }

    public override void Off()
    {
        _isMoving = false;
        GameObject.FindWithTag("Player").GetComponent<PlayerInput>().SwitchCurrentActionMap("ActionsMapPR");
    }

    public void Up()
    {
        if (_direction == new Vector3(0, 1, 0) || _direction == Vector3.zero)
        {
            _isMoving = true;
            _direction = new Vector2(0, 1);
        }
        else
        {
            Stop();
        }
    }

    public void Down() 
    {
        if (_direction != new Vector3(0, -1, 0) || _direction == Vector3.zero)
        {
            _isMoving = true;
            _direction = new Vector2(0, -1);
        }
        else
        {
            Stop();
        }
    }

    public void Left()
    {
        if (_direction != new Vector3(-1, 0, 0) || _direction == Vector3.zero)
        {
            _isMoving = true;
            _direction = new Vector2(-1, 0);
        }
        else
        {
            Stop();
        }
    }

    public void Right()
    {
        if (_direction != new Vector3(1, 0, 0) || _direction == Vector3.zero)
        {
            _isMoving = true;
            _direction = new Vector2(1, 0);
        }
        else
        {
            Stop();
        }
    }

    public void Stop()
    {
        _isMoving = false;
        _direction = Vector2.zero;
    }



    /*
     *  UNITY METHODS
     */

    private void Start()
    {
        _direction = Vector2.zero;
        _speed = 0;
        _acceleration = _speedMax / 5;
    }
    private void FixedUpdate()
    {
        if (_isMoving || _speed > 0)
        {
            gameObject.transform.position += _direction * _speed * Time.deltaTime;
        }
        if (_speed < _speedMax)
        {
            _speed += _acceleration * Time.deltaTime;
        }
        else if(_speed > 0)
        {
            _speed -= _acceleration * Time.deltaTime;
        }
    }
}
