using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_MovObj_Timer : BON_MovObj_ListBased
{

    /*
     * FIELDS 
     */

    [SerializeField]
    protected float _timerMax;
    [SerializeField] float _currentTimer = 0f;
    [SerializeField] float _holdTimer = 0f;

    private bool _returning = false;
    private bool _isMoving = false;

    new public int NextNodeIndex
    {
        get { return _nextNodeIndex; }
        set
        {
            _currentNodeIndex = _nextNodeIndex;

            if (value >= _transformsList.Count || value < 0)
            {
                _returning = !_returning;

                if (_looping)
                {

                    if (_turnBackWhenListEnd)
                    {
                        _isCyclingPositive = !_isCyclingPositive;

                        if (value >= _transformsList.Count)
                        {
                            _nextNodeIndex = _transformsList.Count - 1;
                        }
                        else if (value < 0)
                        {
                            _nextNodeIndex = 0;
                        }
                    }
                    else
                    {
                        if (value >= _transformsList.Count)
                        {
                            _nextNodeIndex = 0;
                        }
                        else if (value < 0)
                        {
                            _nextNodeIndex = _transformsList.Count - 1;
                        }
                    }
                }
                else
                {
                    if (_turnBackWhenListEnd)
                    {
                        _isCyclingPositive = !_isCyclingPositive;
                    }
                    _currentTimer = 0;
                    _isMoving = false;
                }
            }
            else
            {
                _nextNodeIndex = value;
            }

            _currentDirection = (_transformsList[_nextNodeIndex].position - _transformsList[_currentNodeIndex].position).normalized;
            _currentRotation = _transformsList[_nextNodeIndex].rotation.eulerAngles - _transformsList[_currentNodeIndex].rotation.eulerAngles;
            _currentTotalDistance = (_transformsList[_nextNodeIndex].position - _transformsList[_currentNodeIndex].position).magnitude;
        }
    }

    /*
    * CLASS METHODS
    */

    /*
     * UNITY METHODS
     */

    new void Start()
    {
        base.Start();
        _turnBackWhenListEnd = true;
        _looping = false;
        _isCyclingPositive = true;

    }

    new void FixedUpdate()
    {
        if (Status && _isMoving == false && _returning == false)
        {
            _isMoving = true;
        }

        else if (_isMoving)
        {
            float oneSpeed = _moveSpeed * Time.deltaTime;
            if (oneSpeed >= (_transformsList[_nextNodeIndex].position - gameObject.transform.position).magnitude)
            {
                gameObject.transform.SetPositionAndRotation(_transformsList[_nextNodeIndex].position, _transformsList[_nextNodeIndex].rotation);
                if (_isCyclingPositive) { NextNodeIndex++; } else { NextNodeIndex--; }
            }
            else
            {
                float movementFraction = oneSpeed / _currentTotalDistance;
                gameObject.transform.position += _currentDirection * oneSpeed;
                gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles + _currentRotation * movementFraction);
            }
        }

        else if (_returning && Status == false && _isMoving == false)
        {
            _currentTimer += Time.deltaTime;
            if (_currentTimer >= _holdTimer)
            {
                _isMoving = true;
                _holdTimer = 0;
            }
        }

        else if (_returning && Status == true && _isMoving == false)
        {
            _holdTimer += Time.deltaTime * 4;
            if (_holdTimer >= _timerMax)
            {
                _holdTimer = _timerMax;
            }
        }
    }
}
