using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BON_FreeMovementCrane : BON_Actionnable
{
    //FIELD
    private bool _isMoving;
    [SerializeField] float _speed;
    Vector3 _direction;

    //CLASS METHODS
    public override void On()
    {
        //change CC state to control crane
    }

    public override void Off()
    {
        _isMoving = false;
        //change CC state to control robot again
    }

    public void Up()
    {
        if (_direction.x == 0)
        {
            _isMoving = true;
            _direction = new Vector2(0, 1);
        }
    }

    public void Down() 
    {
        if (_direction.x == 0)
        {
            _isMoving = true;
            _direction = new Vector2(0, -1);
        }
    }

    public void Left()
    {
        if (_direction.y == 0)
        {
            _isMoving = true;
            _direction = new Vector2(-1, 0);
        }
    }

    public void Right()
    {
        if (_direction.y == 0)
        {
            _isMoving = true;
            _direction = new Vector2(1, 0);
        }
    }

    public void Stop()
    {
        _isMoving = false;
        _direction = Vector2.zero;
    }



    //UNITY METHODS

    private void Start()
    {
        _direction = Vector2.zero;
    }
    private void FixedUpdate()
    {
        if (_isMoving)
        {
            gameObject.transform.position += _direction * _speed * Time.deltaTime;
        }
    }
}
