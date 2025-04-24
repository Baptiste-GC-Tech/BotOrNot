using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_VFXObjectShake : BON_Actionnable
{
    /*
     *  FIELDS
     */

    Vector3 _initialPosition;
    bool _onLeft = false;
    [SerializeField] Vector3 _shakeOffset;
    [SerializeField] float _shakeDelay;
    float _shakeTimer;

    /*
     * CLASS METHODS
     */

    public override void On()
    {
        base.On();
        _shakeTimer = 0f;
    }

    public override void Off()
    {
        base.Off();
        transform.position = _initialPosition;
    }

    /*
     * UNITY METHODS
     */

    private void Start()
    {
        _initialPosition = transform.position;
    }

    void Update()
    {
        if (Status)
        {

            if (_onLeft)
            {
                transform.position = _initialPosition + _shakeOffset * Time.deltaTime;
            }
            else
            {
                transform.position = _initialPosition - _shakeOffset * Time.deltaTime;
            }

            _shakeTimer += Time.deltaTime;

            if (_shakeTimer > _shakeDelay) 
            {
                _onLeft = !_onLeft;
            }

        }
    }
}
