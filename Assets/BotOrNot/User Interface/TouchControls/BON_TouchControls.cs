using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BON_TouchControls : MonoBehaviour
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

    [Range(20f, 50f)]
    private float _slideThreshold = 10f;

    public float SlideThreshold
    {
        get { return _slideThreshold; }
        set { _slideThreshold = value; } 
    }




    /*
     * METHODS 
     */

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
        if (position.x - _initialTouchPos.x > _slideThreshold
                    || position.x - _initialTouchPos.x < -_slideThreshold
                    || position.y - _initialTouchPos.y > _slideThreshold
                    || position.y - _initialTouchPos.y < -_slideThreshold)
        {
            return false;
        }
        return true;
    }





    /*
     *  UNITY METHODS
     */

    void Start()
    {
        _joystickImage = joystick.GetComponent<RawImage>();
    }
    
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _initialTouchPos = touch.position;
                _hasPassedThreshold = false;
            }

            else if (touch.phase == TouchPhase.Moved)
            {
                _previousTouchPos = _currentTouchPos;
                _currentTouchPos = touch.position;

                if (_hasPassedThreshold == false)
                {
                    if (PRIVIsInThreshold(_currentTouchPos) == false)
                    {
                        _hasPassedThreshold = true;
                        PRIVUnhideJoystick();
                        PRIVMoveJoystick();
                    }
                }
                else
                {
                    PRIVMoveJoystick();
                }
            }

            else if (touch.phase == TouchPhase.Ended)
            {
                PRIVHideJoystick();
            }
        }
    }
}
