using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_TouchComps : MonoBehaviour
{
    /*
     * FIELDS
     */

    protected Touch _touch;

    protected BON_TouchControls _controls;

    protected bool _isEnabled;





    /*
    * METHODS 
    */

    public virtual void TouchStart(Touch touch, Vector2 initialTouchPos)
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
       _controls = GetComponentInParent<BON_TouchControls>(); 
    }

    protected virtual void Update()
    {
        
    }
}
