using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_Actionnable : MonoBehaviour
{
    /*
     *  FIELDS
     */
    [SerializeField]
    protected bool _status ;
    public bool Status {  
        get { return _status; } 
        set { _status = value;
            if (_status) { On(); }
            else { Off(); }
        } }


    /*
     *  CLASS METHODS
     */
    public virtual void On()
    {
    }

    public virtual void Off()
    {
    }

    public virtual void Toggle()
    {
        Status = !Status;
    }
}
