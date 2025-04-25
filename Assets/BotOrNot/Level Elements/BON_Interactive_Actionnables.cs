using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_Interactive_Actionnables : BON_Interactive
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
    public override void Activate()
    {
        //Debug.Log("Tryng to activate from GO " + gameObject.name);
        
        foreach (var action in _actionnablesList)
        {
            action.Toggle();
        }
    }
}
