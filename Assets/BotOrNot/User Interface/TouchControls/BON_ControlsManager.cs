using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BON_ControlsManager : MonoBehaviour
{
    /*
     * FIELDS
     */

    private List<Vector2> _initialTouchPos = new List<Vector2>(3);
    private List<Vector2> _currentTouchPos = new List<Vector2>(3);
    private List<bool> _hasPassedThresholdList = new List<bool>(3);
    private Dictionary<int, BON_TouchComps> _touchDictionary = new Dictionary<int, BON_TouchComps>();

    [Range(20f, 50f)]
    private float _slideThreshold = 50f;
    public float SlideThreshold
    {
        get { return _slideThreshold; }
        set { _slideThreshold = value; }
    }

    [SerializeField] BON_COMPJoystick _compJoystick;
    public BON_COMPJoystick CJoystick
    { get { return _compJoystick; } }

    [SerializeField] BON_COMPPlayerButtons _compPlayerButtons;
    public BON_COMPPlayerButtons CPButtons
    { get { return _compPlayerButtons; } }

    [SerializeField] BON_COMPHUDButtons _compHUDButtons;
    public BON_COMPHUDButtons CHUDButtons
    { get { return _compHUDButtons; } }

    [SerializeField] GameObject _touchFeedback;

    private bool _isTouchEnabled = true;
    public bool IsTouchEnabled
    {
        get { return _isTouchEnabled; }
        set { _isTouchEnabled = value; }
    }

    private GameObject _lastInteractedObj;
    private Vector3 _lastInteractedPos;

    /*
     * METHODS 
     */

    private bool PRIVTryIsInThreshold(Vector2 position, int i)
    {
        if (Math.Abs(position.x - _initialTouchPos[i].x) > _slideThreshold || (Math.Abs(position.y - _initialTouchPos[i].y) > _slideThreshold))
        {
            return false;
        }
        return true;
    }

    private void PRIVTouchDispatch(BON_TouchComps comp, Touch touch, int i)
    {
        if (comp.IsCompActive)
        { comp.TouchResume(touch, _initialTouchPos[i]); }
        else
        { comp.TouchStart(touch, _initialTouchPos[i]); }
    }

    private void PRIVTouchFinish(BON_TouchComps comp, int i)
    {
        if (_touchDictionary.TryGetValue(i, out BON_TouchComps result))
        {
            comp.TouchEnd();
            _touchDictionary.Remove(i);
        }
        else
        {
            if ((_compPlayerButtons.TryIsButtonThere(_currentTouchPos[i]) == false || _compPlayerButtons.IsEnabled == false) 
                && (_compHUDButtons.TryIsButtonThere(_currentTouchPos[i]) == false || _compHUDButtons.IsEnabled == false))
            {

                Ray ray = Camera.main.ScreenPointToRay(_currentTouchPos[i]);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, (1 << 3)))    // The layermask will fail if the TouchInteractible layer ever become not the 3rd layer !
                {
                    _lastInteractedObj = hit.transform.gameObject;
                    _lastInteractedPos = _lastInteractedObj.transform.position;
                    _lastInteractedObj.GetComponent<BON_Interactive>().Activate();
                }
            }
        }
    }

    /*
     *  UNITY METHODS
     */

    void Update()
    {
        if (Input.touchCount > 0 && _isTouchEnabled)
        {
            for (int i = 0;  i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                if (touch.phase == TouchPhase.Began)
                {
                    _initialTouchPos.Add(touch.position);
                    _currentTouchPos.Add(touch.position);

                    _hasPassedThresholdList.Add(false);
                }

                else if (touch.phase == TouchPhase.Moved && _compJoystick.IsEnabled)
                {
                    _currentTouchPos[i] = touch.position;

                    if (_hasPassedThresholdList[i] == false && _compJoystick.IsCompActive == false)
                    {
                        if (PRIVTryIsInThreshold(_currentTouchPos[i], i) == false)
                        {
                            _hasPassedThresholdList[i] = true;
                            PRIVTouchDispatch(_compJoystick, touch, i);
                            _touchDictionary.Add(i, _compJoystick);
                            _compJoystick.TouchID = i;
                        }
                    }
                    else if (_compJoystick.TouchID == i)
                    {
                        PRIVTouchDispatch(_compJoystick, touch, i);
                    }
                }

                else if (touch.phase == TouchPhase.Ended)
                {
                    if (_touchDictionary.TryGetValue(i, out BON_TouchComps result))
                    {
                        PRIVTouchFinish(_touchDictionary[i], i);
                    }
                    else
                    {
                        PRIVTouchFinish(null, i);
                    }

                    _initialTouchPos.Remove(_initialTouchPos[i]);
                    _currentTouchPos.Remove(_currentTouchPos[i]);
                    _hasPassedThresholdList.Remove(_hasPassedThresholdList[i]);

                    if (Input.touchCount <= 1)
                    {
                        _initialTouchPos.Clear();
                        _currentTouchPos.Clear();
                        _hasPassedThresholdList.Clear();
                    }
                }
            }
        }
    }
}