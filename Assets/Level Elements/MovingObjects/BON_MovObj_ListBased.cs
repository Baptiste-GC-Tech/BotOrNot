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
    protected int CurrentTargetIndex {  
        get { return _currentTargetIndex; }
        set {
            if (value >= _transformsList.Count) { 
                if (_looping) { _currentTargetIndex = 0; } 
                else { _currentTargetIndex = _transformsList.Count - 1; }  }
            else if (value < 0) { 
                if (_looping) { _currentTargetIndex = _transformsList.Count - 1; } 
                else { _currentTargetIndex = 0; } }
            else { _currentTargetIndex = value; } 
        } 
    }

    protected int PreviousTargetIndex { 
        get { 
            if (_isCyclingPositive) { 
                if (_currentTargetIndex == 0) { return _transformsList.Count - 1; } 
                else { return _currentTargetIndex - 1; } 
            } 
            else {
                if (_currentTargetIndex == _transformsList.Count - 1) { return 0; }
                else { return _currentTargetIndex + 1; }
            }
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
        _status = true;
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

    void Update()
    {
        if (_status) 
        {
            float oneSpeed = _moveSpeed * Time.deltaTime;

            

        }
    }
}
