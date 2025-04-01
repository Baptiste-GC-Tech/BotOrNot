using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BON_MovObj_ListBased : BON_Actionnable
{
    [SerializeField]
    protected List<Transform> _transformsList;

    [SerializeField]
    protected float _moveSpeed = 10.0f;

    [SerializeField]
    protected bool _looping = false;
    [SerializeField]
    protected bool _isCyclingPositive = false;

    private int _currentTargetIndex = 0;
    protected int _previousTargetIndex;
    protected float _currentTotalDistance;
    protected Vector3 _currentDirection;
    protected Vector3 _currentRotation;


    protected int CurrentTargetIndex {  
        get { return _currentTargetIndex; }
        set {

            _previousTargetIndex = _currentTargetIndex;

            if (value >= _transformsList.Count) { 
                if (_looping) { _currentTargetIndex = 0; } 
                else { _status = false; _isCyclingPositive = false; }  }
            else if (value < 0) { 
                if (_looping) { _currentTargetIndex = _transformsList.Count - 1; } 
                else { _status = false; _isCyclingPositive = true; } }
            else { _currentTargetIndex = value; }

            _currentDirection = (_transformsList[_currentTargetIndex].position - _transformsList[_previousTargetIndex].position).normalized;
            _currentRotation = _transformsList[_currentTargetIndex].rotation.eulerAngles - _transformsList[_previousTargetIndex].rotation.eulerAngles;
            _currentTotalDistance = (_transformsList[_currentTargetIndex].position - _transformsList[_previousTargetIndex].position).magnitude;
        }
    }

    override public void On()
    {
        CurrentTargetIndex++;
        base.On();
    }

    override public void Off()
    {
        CurrentTargetIndex--;
        base.Off();
    }

    override public void Toggle()
    {
        //_status = true;
        base.Toggle();
    }

    void Start()
    {
        if (_status) 
        {
            if (_isCyclingPositive)
            {
                CurrentTargetIndex++;
            }
            else
            {
                CurrentTargetIndex--;
            }
        }
    }

    void FixedUpdate()
    {
        if (_status) 
        {
            float oneSpeed = _moveSpeed * Time.deltaTime;

            if (oneSpeed >= (_transformsList[_currentTargetIndex].position - gameObject.transform.position).magnitude)
            {
                gameObject.transform.SetPositionAndRotation(_transformsList[_currentTargetIndex].position, _transformsList[_currentTargetIndex].rotation);
                if (_isCyclingPositive) { CurrentTargetIndex++; } else { CurrentTargetIndex--; }
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
