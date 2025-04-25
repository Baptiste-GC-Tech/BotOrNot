using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BON_MovObj_ListBased : BON_Actionnable
{

    /*
    FIELDS
    */

    [SerializeField]
    protected List<Transform> _transformsList;

    [SerializeField]
    protected float _moveSpeed = 10.0f;

    [SerializeField] 
    protected bool _turnBackWhenListEnd = false;
    [SerializeField]
    protected bool _looping = false;
    [SerializeField]
    protected bool _isCyclingPositive = true;
    [SerializeField]
    protected bool _onlyRotation = false;

    protected int _nextNodeIndex = 0;
    protected int _currentNodeIndex;
    protected float _currentTotalDistance;
    protected Vector3 _currentDirection;
    protected Vector3 _currentRotation;

    public bool IsCyclingPositive { get { return _isCyclingPositive; } }
    public int NextNodeIndex {  
        get { return _nextNodeIndex; }
        set {

            _currentNodeIndex = _nextNodeIndex;

            if(value >= _transformsList.Count || value < 0)
            {
                

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
                    Status = false;
                }
            }
            else { 
                _nextNodeIndex = value; 
            }

            _currentDirection = (_transformsList[_nextNodeIndex].position - _transformsList[_currentNodeIndex].position).normalized;
            _currentRotation = _transformsList[_nextNodeIndex].rotation.eulerAngles - _transformsList[_currentNodeIndex].rotation.eulerAngles;
            _currentTotalDistance = (_transformsList[_nextNodeIndex].position - _transformsList[_currentNodeIndex].position).magnitude;
        }
    }

    public void Start()
    {
        if (Status) 
        {
            if (_isCyclingPositive)
            {
                NextNodeIndex++;
            }
            else
            {
                NextNodeIndex--;
            }
        }
    }

    public void FixedUpdate()
    {
        if (!Status || _transformsList == null || _transformsList.Count < 2) return;

        if (_nextNodeIndex < 0 || _nextNodeIndex >= _transformsList.Count)
        {
            Debug.LogWarning($"{gameObject.name} : _nextNodeIndex ({_nextNodeIndex}) est en dehors des bornes de _transformsList (count: {_transformsList.Count})");
            Status = false;
            return;
        }

        if (_onlyRotation)
        {
            float oneTurnSpeed = _moveSpeed * Time.deltaTime;

            if ((_transformsList[_nextNodeIndex].rotation.eulerAngles - transform.rotation.eulerAngles).magnitude <=
                (_transformsList[_nextNodeIndex].rotation.eulerAngles - (transform.rotation.eulerAngles + _currentRotation * oneTurnSpeed)).magnitude)
            {
                transform.SetPositionAndRotation(_transformsList[_nextNodeIndex].position, _transformsList[_nextNodeIndex].rotation);
                if (_isCyclingPositive) NextNodeIndex++;
                else NextNodeIndex--;
            }
            else
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + _currentRotation * oneTurnSpeed);
            }

        }
        else
        {
            float oneSpeed = _moveSpeed * Time.deltaTime;
            if (oneSpeed >= (_transformsList[_nextNodeIndex].position - transform.position).magnitude)
            {
                transform.SetPositionAndRotation(_transformsList[_nextNodeIndex].position, _transformsList[_nextNodeIndex].rotation);
                if (_isCyclingPositive) NextNodeIndex++;
                else NextNodeIndex--;
            }
            else
            {
                float movementFraction = oneSpeed / _currentTotalDistance;
                transform.position += _currentDirection * oneSpeed;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + _currentRotation * movementFraction);
            }
        }
    }

}
