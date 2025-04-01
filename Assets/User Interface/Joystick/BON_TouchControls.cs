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
    private Vector2 _joystickOrigin;

    [Range(20f, 50f)]
    public float slideThreshold = 10f;





    /*
     * METHODS 
     */

    private void PRIVUnhideJoystick()
    {
        joystick.GetComponent<RawImage>().enabled = true;
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
        joystick.GetComponent<RawImage>().enabled = false;
    }

    private bool PRIVIsInThreshold(Vector2 position)
    {
        if (position.x - _initialTouchPos.x > slideThreshold
                    || position.x - _initialTouchPos.x < -slideThreshold
                    || position.y - _initialTouchPos.y > slideThreshold
                    || position.y - _initialTouchPos.y < -slideThreshold)
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
