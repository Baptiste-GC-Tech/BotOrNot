using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_TouchComps : MonoBehaviour
{
    /*
     * FIELDS
     */

    protected Touch _touch;

    protected BON_ControlsManager _controls;

    protected bool _isEnabled;
    public bool IsEnabled
    {
        get { return _isEnabled; }
    }

    protected bool _isCompActive;
    public bool IsCompActive
    {
        get { return _isCompActive; }
    }


    /*
    * METHODS 
    */

    public virtual void TouchStart(Touch touch, Vector2 initialTouchPos)
    {
        _touch = touch;
    }

    public virtual void TouchResume(Touch touch, Vector2 initialTouchPos)
    {
        _touch = touch;
    }

    public virtual void TouchEnd()
    {

    }

    public void ComponentToggle()
    {
        switch(_isEnabled)
        {
            case true:
                _isEnabled = false;
                break;
            case false:
                _isEnabled = true;
                break;
        }
    }





    /*
     *  UNITY METHODS
     */

    protected virtual void Start()
    {
       _controls = GetComponentInParent<BON_ControlsManager>(); 
    }

    protected virtual void Update()
    {
        
    }
}
