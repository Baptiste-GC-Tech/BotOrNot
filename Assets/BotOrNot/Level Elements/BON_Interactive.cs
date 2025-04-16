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

    public List<BON_Actionnable> ActionnablesList
    {
        get { return _actionnablesList; }
    }

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