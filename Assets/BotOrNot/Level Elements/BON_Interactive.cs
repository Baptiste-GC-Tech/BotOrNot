using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class BON_Interactive : MonoBehaviour
{
    /*
     *  FIELDS
     */
    [SerializeField]
    protected List<BON_Actionnable> _actionnablesList;



    /*
     *  CLASS METHODS
     */
    public virtual void Activate()
    {
        foreach (var action in _actionnablesList) 
        {
            action.Toggle();
        }
    }
}