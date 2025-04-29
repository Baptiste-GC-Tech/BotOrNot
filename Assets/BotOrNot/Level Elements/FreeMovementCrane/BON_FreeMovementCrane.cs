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

    /* physics related */
    private bool _isCollideDown;
    private bool _isCollideUp;
    private bool _isCollideLeft;
    private bool _isCollideRight;

    private Rigidbody _rigidbody;
    private Collider _collider;

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
        if ((_direction == new Vector3(0, 1, 0) || _direction == Vector3.zero) && !_isCollideUp)
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
        if ((_direction == new Vector3(0, -1, 0) || _direction == Vector3.zero) && !_isCollideDown)
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
        if ((_direction == new Vector3(-1, 0, 0) || _direction == Vector3.zero) && !_isCollideLeft)
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
        if ((_direction == new Vector3(1, 0, 0) || _direction == Vector3.zero) && !_isCollideRight)
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
        _acceleration = _speedMax / 5;
        _isBlocked = false;
        _isCollideUp = false;
        _isCollideDown = false;
        _isCollideLeft = false;
        _isCollideRight = false;
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        // vérifie si la direction actuelle est bloquée
        bool directionBlocked =
            (_direction == Vector3.up && _isCollideUp) ||
            (_direction == Vector3.down && _isCollideDown) ||
            (_direction == Vector3.left && _isCollideLeft) ||
            (_direction == Vector3.right && _isCollideRight);

        if ((_IsMovingByPlayer || _speed > 0) && !directionBlocked)
        {
            Vector3 nextPos = _rigidbody.position + _direction * _speed * Time.deltaTime;
            bool isInsideBounds = false;

            foreach (Vector4 Box in _boundaries)
            {
                if (Box[0] <= nextPos.x - _collider.bounds.extents.x && nextPos.x + _collider.bounds.extents.x <= Box[1] &&
                    Box[2] <= nextPos.y - _collider.bounds.extents.y && nextPos.y + _collider.bounds.extents.y <= Box[3])
                {
                    isInsideBounds = true;
                    break;
                }
            }
            if (isInsideBounds)
            {
                _rigidbody.MovePosition(nextPos);
                _isBlocked = false;
            }
            else
            {
                _isBlocked = true;
            }
        }

        float oneAcceleration = _acceleration * Time.deltaTime;

        if (_IsMovingByPlayer && _speed + oneAcceleration < _speedMax)
        {
            _speed += oneAcceleration;
        }
        else if (!_IsMovingByPlayer && _speed - oneAcceleration > 0)
        {
            _speed -= oneAcceleration;
        }
        else if (!_IsMovingByPlayer || _isBlocked || directionBlocked)
        {
            _speed = 0;
            _direction = Vector2.zero;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                Vector3 normal = contact.normal;

                if (normal.y > 0.5f)
                {
                    _isCollideDown = true;
                }
                else if (normal.y < -0.5f)
                {
                    _isCollideUp = true;
                }
                if (normal.x > 0.5f)
                {
                    _isCollideLeft = true;
                }
                else if (normal.x < -0.5f)
                {
                    _isCollideRight = true;
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        _isCollideUp = false;
        _isCollideDown = false;
        _isCollideLeft = false;
        _isCollideRight = false;
    }
}
