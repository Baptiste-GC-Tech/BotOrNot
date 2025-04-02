using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class BON_Interactive : MonoBehaviour
{
    [SerializeField]
    protected List<BON_Actionnable> _actionnablesList;

    public virtual void Activate()
    {
        foreach (var action in _actionnablesList) 
        {
            action.Toggle();
        }
    }
}