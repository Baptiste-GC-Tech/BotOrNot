using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BON_COMPJoystick : BON_TouchComps
{
    /*
    * FIELDS
    */

    private Vector2 _initialTouchPos;
    private Vector2 _previousTouchPos;
    private Vector2 _currentTouchPos;

    private bool _hasPassedThreshold;

    [SerializeField] GameObject joystick;
    private RawImage _joystickImage;





    /*
    * METHODS 
    */

    override public void TouchStart(Touch touch, Vector2 initialTouchPos)
    {
        base.TouchStart(touch, initialTouchPos);
    }

    override public void TouchEnd()
    {
        base.TouchEnd();
    }

    private void PRIVUnhideJoystick()
    {
        _joystickImage.enabled = true;
    }
    private void PRIVMoveJoystick()
    {
        joystick.transform.GetLocalPositionAndRotation(out Vector3 pos, out Quaternion rot);
        joystick.transform.SetLocalPositionAndRotation(new Vector3(_currentTouchPos.x - Screen.width / 2, _currentTouchPos.y - Screen.height / 2), rot);

        //Magnetude entre le pos initial et le currentPos
        //Quand la diff entre _previousPos et _currentPos < 0-threshold, alors changer le initial au nouveau x/y du changement
        //Le threshold etant la meme distance que pour atteindre la vitesse dep pointe du robot
    }

    private void PRIVHideJoystick()
    {
        _joystickImage.enabled = false;
    }

    private bool PRIVIsInThreshold(Vector2 position)
    {
        if (position.x - _initialTouchPos.x > _controls.SlideThreshold
                    || position.x - _initialTouchPos.x < -_controls.SlideThreshold
                    || position.y - _initialTouchPos.y > _controls.SlideThreshold
                    || position.y - _initialTouchPos.y < -_controls.SlideThreshold)
        {
            return false;
        }
        return true;
    }





    /*
     *  UNITY METHODS
     */

    override protected void Start()
    {
        base.Start();
    }

    override protected void Update()
    {
        base.Update();
    }
}
