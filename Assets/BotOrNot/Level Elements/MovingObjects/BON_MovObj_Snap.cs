using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_MovObj_Snap : BON_Actionnable
{
    [SerializeField]
    Transform[] _positions = new Transform[2];

    [SerializeField]
    float _maxTimer;
    float _currentTimer;


    public override void On()
    {
        base.On();
        gameObject.transform.SetLocalPositionAndRotation(_positions[1].position, _positions[1].rotation);
    }

    public override void Off()
    {
        base.Off();
        gameObject.transform.SetLocalPositionAndRotation(_positions[0].position, _positions[0].rotation);
    }


    private void Update()
    {
        _currentTimer += Time.deltaTime;

        if (_currentTimer > _maxTimer) { Toggle(); _currentTimer = 0; }
    }
}
