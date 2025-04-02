using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_MovObj_Timer : BON_Actionnable
{
    //FIELD//
    private bool _isMoving = false;
    [SerializeField] float _maxTimer;
    private float _currentTimer = 0.0f;

    [SerializeField] private float _extensionMax;
    [SerializeField] private float _extensionMin;
    [SerializeField] private float _speed;


    [SerializeField] float _dumbassTimer;
    private float _currentDumbassTimer = 0.0f;

    //CLASS METHODS
    override public void On()
    {
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
        _currentTimer = 0.0f;
        _isMoving = true;
    }

    private void FixedUpdate()
    {
        if (_isMoving) 
        {
            if (Status)
            {
                if (gameObject.transform.localScale.x < _extensionMax)
                {
                    gameObject.transform.localScale += new Vector3(Time.deltaTime * _speed, 0, 0);
                }
            }
            else
            {
                if (_currentTimer < _maxTimer)
                {
                    _currentTimer += Time.deltaTime;
                }
                else if (gameObject.transform.localScale.x > _extensionMin)
                {
                    gameObject.transform.localScale -= new Vector3(Time.deltaTime * _speed, 0, 0);
                }
                else
                {
                    gameObject.transform.localScale = new Vector3(_extensionMin, 0, 0);
                    _isMoving = false;
                }
            }
        }


        _currentDumbassTimer += Time.deltaTime;

        if(_currentDumbassTimer > _dumbassTimer)
        {
            _currentDumbassTimer = 0.0f;
            Toggle();
        }

    }
}
