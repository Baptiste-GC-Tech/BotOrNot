using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_Timer : BON_Actionnable
{
    [SerializeField] private float _maxTimer;
    private float _currentTimer;
    private float _extensionMax;

    override public void Off()
    {
        _currentTimer = 0;
    }

    private void Start()
    {
        _currentTimer = _maxTimer;
        Status = true;
    }
    private void FixedUpdate()
    {
        if (Status || _currentTimer < _maxTimer)
        {
            if (gameObject.transform.localScale.x < _extensionMax)
            {
                gameObject.transform.localScale += new Vector3(Time.deltaTime, 0, 0);
            }
        }
        else
        {
            if (gameObject.transform.localScale.x > 0)
            {
                gameObject.transform.localScale -= new Vector3(Time.deltaTime, 0, 0);
            }
        }
        if (_currentTimer < _maxTimer)
        {
            _currentTimer += Time.deltaTime;
        }
    }
}
