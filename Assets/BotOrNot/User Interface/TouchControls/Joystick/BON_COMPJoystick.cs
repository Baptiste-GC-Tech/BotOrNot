using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BON_COMPJoystick : BON_TouchComps
{
    /*
    * FIELDS
    */

    private InputAction _moveAction;

    private Vector2 _initialTouchPos;
    private Vector2 _previousTouchPos;
    private Vector2 _currentTouchPos;

    private Vector2 _inputValues = Vector2.zero;
    public Vector2 InputValues
    {
        get { return _inputValues; }
    }

    private int _touchID;
    public int TouchID 
    {  
        get { return _touchID; }
        set { _touchID = value; }
    }


    private RawImage _joystickImage;





    /*
    * METHODS 
    */

    override public void TouchStart(Touch touch, Vector2 initialTouchPos)
    {
        base.TouchStart(touch, initialTouchPos);

        _isCompActive = true;
        _initialTouchPos = initialTouchPos;

        gameObject.transform.GetLocalPositionAndRotation(out Vector3 pos, out Quaternion rot);
        gameObject.transform.SetLocalPositionAndRotation(new Vector3(_currentTouchPos.x - Screen.width / 2, _currentTouchPos.y - Screen.height / 2), rot);

        PRIVUnhideJoystick();
    }

    override public void TouchResume(Touch touch, Vector2 initialTouchPos)
    {
        base.TouchResume(touch, initialTouchPos);

        _previousTouchPos = _currentTouchPos;
        _currentTouchPos = touch.position;
    }

    override public void TouchEnd()
    {
        base.TouchEnd();

        _isCompActive = false;
        _inputValues = Vector2.zero;

        PRIVHideJoystick();
    }

    private void PRIVUnhideJoystick()
    {
        _joystickImage.color = new Color(_joystickImage.color.r, _joystickImage.color.g, _joystickImage.color.b, 50);
    }
    private void PRIVMoveJoystick()
    {
        gameObject.transform.GetLocalPositionAndRotation(out Vector3 pos, out Quaternion rot);
        gameObject.transform.SetLocalPositionAndRotation(new Vector3(_currentTouchPos.x - Screen.width / 2, _currentTouchPos.y - Screen.height / 2), rot);

        //Magnetude entre le pos initial et le currentPos
        _inputValues.x += (_currentTouchPos.x - _previousTouchPos.x)/200;
        _inputValues.y += (_currentTouchPos.y - _previousTouchPos.y)/200;
        PRIVClampInput();


    }

    private void PRIVHideJoystick()
    {
        _joystickImage.color = new Color(_joystickImage.color.r, _joystickImage.color.g, _joystickImage.color.b, 0);
    }

    private void PRIVClampInput()
    {
        if (_inputValues.x > 1)
            _inputValues.x = 1;
        else if (_inputValues.x < -1)
            _inputValues.x = -1;

        if (_inputValues.y > 1)
            _inputValues.y = 1;
        else if (_inputValues.y < -1)
            _inputValues.y = -1;
    }





    /*
     *  UNITY METHODS
     */

    override protected void Start()
    {
        base.Start();

        _joystickImage = gameObject.GetComponent<RawImage>();
        _joystickImage.color = new Color(_joystickImage.color.r, _joystickImage.color.g, _joystickImage.color.b, 0);
    }

    override protected void Update()
    {
        base.Update();

        if (_isCompActive)
        {
            PRIVMoveJoystick();
        }
    }
}
