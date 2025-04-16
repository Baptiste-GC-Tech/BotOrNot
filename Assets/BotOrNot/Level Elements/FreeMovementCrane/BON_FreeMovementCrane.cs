using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.Rendering;

public class BON_FreeMovementCrane : BON_Controllable
{
    /*
     *  FIELDS
     */

    private bool _IsMovingByPlayer;
    [SerializeField] float _speedMax;
    private float _speed;
    private float _acceleration;
    private Vector3 _direction;
    [SerializeField] List<Vector4> _boundaries;
    private bool _isBlocked;



    /*
     *  CLASS METHODS
     */

    public override void Off()
    {
        base.Off();
        Stop();
    }

    public override void ProcessInput(Vector2 Input)
    {
        print(Input);
        if (System.Math.Abs(Input.x) > 0.1f || System.Math.Abs(Input.y) > 0.1f)
        {
            if (System.Math.Abs(Input.y) > System.Math.Abs(Input.x))
            {
                if (Input.y > 0)
                {
                    Up();
                }
                else
                {
                    Down();
                }
            }
            else
            {
                if (Input.x > 0)
                {
                    Right();
                }
                else
                {
                    Left();
                }
            }
        }
        else
        {
            Stop();
        }
    }
    public void Up()
    {
        if (_direction == new Vector3(0, 1, 0) || _direction == Vector3.zero)
        {
            _IsMovingByPlayer = true;
            _direction = new Vector2(0, 1);
        }
        else
        {
            Stop();
        }
    }

    public void Down() 
    {
        if (_direction == new Vector3(0, -1, 0) || _direction == Vector3.zero)
        {
            _IsMovingByPlayer = true;
            _direction = new Vector2(0, -1);
        }
        else
        {
            Stop();
        }
    }

    public void Left()
    {
        if (_direction == new Vector3(-1, 0, 0) || _direction == Vector3.zero)
        {
            _IsMovingByPlayer = true;
            _direction = new Vector2(-1, 0);
        }
        else
        {
            Stop();
        }
    }

    public void Right()
    {
        if (_direction == new Vector3(1, 0, 0) || _direction == Vector3.zero)
        {
            _IsMovingByPlayer = true;
            _direction = new Vector2(1, 0);
        }
        else
        {
            Stop();
        }
    }

    public void Stop()
    {
        _IsMovingByPlayer = false;
    }



    /*
     *  UNITY METHODS
     */

    private void Start()
    {
        _direction = Vector2.zero;
        _speed = 0;
        _acceleration = _speedMax / 3;
        _isBlocked = false;
    }
    private void FixedUpdate()
    {
        if (_IsMovingByPlayer || _speed > 0)
        {
            Vector3 nextPos = gameObject.transform.position + _direction * _speed * Time.deltaTime;
            foreach (Vector4 Box in _boundaries)
            {
                if (Box[0] <= nextPos.x && nextPos.x <= Box[1] && Box[2] <= nextPos.y && nextPos.y <= Box[3])
                {
                    gameObject.transform.position += _direction * _speed * Time.deltaTime;
                    _isBlocked = false;
                    break;
                }
                else
                {
                    _isBlocked = true;
                }
            }
        }

        float oneAcceleration = _acceleration * Time.deltaTime;

        if (_IsMovingByPlayer && _speed + oneAcceleration < _speedMax)
        {
            _speed += oneAcceleration;
        }
        else if(!_IsMovingByPlayer && _speed - oneAcceleration > 0)
        {
            _speed -= oneAcceleration;
        }
        else if (!_IsMovingByPlayer || _isBlocked)
        {
            _speed = 0;
            _direction = Vector2.zero;
        }
    }
}
