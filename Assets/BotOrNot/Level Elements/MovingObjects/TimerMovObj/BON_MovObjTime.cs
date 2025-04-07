using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_MovObjTime : BON_MovObj_ListBased
{
    //FIELD
    private bool _isMoving = false;
    [SerializeField] float _maxTimer;
    private float _currentTimer = 0.0f;

    //CLASS METHODS
    override public void On()
    {
        _currentTimer = _maxTimer;
        _isMoving = true;
    }

    override public void Off()
    {
        _currentTimer = 0.0f;
        _isMoving = true;
    }

    //UNITY METHODS
    private void Start()
    {
        _currentTimer = _maxTimer;
        _isMoving = true;
    }

    private void FixedUpdate()
    {
        if (_isMoving)
        {
            float oneSpeed = _moveSpeed * Time.deltaTime;

            if (Status || _currentTimer < _maxTimer) 
            { 
                if (oneSpeed >= (_transformsList[NextNodeIndex].position - gameObject.transform.position).magnitude)
                {
                    gameObject.transform.SetPositionAndRotation(_transformsList[NextNodeIndex].position, _transformsList[NextNodeIndex].rotation);
                    if (_isCyclingPositive) { NextNodeIndex++; } else { NextNodeIndex--; }
                }
                else
                {
                }
            }
        else
        {
            float movementFraction = oneSpeed / _currentTotalDistance;
            gameObject.transform.position += _currentDirection * oneSpeed;
            gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles + _currentRotation * movementFraction);
        }

        }
    }
}
