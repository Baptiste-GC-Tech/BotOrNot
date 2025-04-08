using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_MovObj_Timer_Old : BON_Actionnable
{
    //FIELD//
    private bool _isMoving = false;
    [SerializeField] float _maxTimer;
    private float _currentTimer = 0.0f;

    [SerializeField] private float _extensionMax;
    [SerializeField] private float _extensionMin;
    [SerializeField] private float _speed;

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
            float oneSpeed = Time.deltaTime * _speed;

            if (Status || _currentTimer < _maxTimer)
            {
                if (gameObject.transform.localScale.x + oneSpeed < _extensionMax)
                {
                    gameObject.transform.localScale += new Vector3(oneSpeed, 0, 0);
                }
                else
                {
                    gameObject.transform.localScale = new Vector3(_extensionMax, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                }
            }
            else
            {
                if (gameObject.transform.localScale.x - oneSpeed > _extensionMin)
                {
                    gameObject.transform.localScale -= new Vector3(oneSpeed, 0, 0);
                }
                else
                {
                    gameObject.transform.localScale = new Vector3(_extensionMin, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                    _isMoving = false;
                }
            }

            _currentTimer += Time.deltaTime;
        }
    }
}
