using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_LE_Actionnable : BON_LevelEvent
{
    /*
     * FIELDS
     */
    enum TargetBehavior { ON, OFF, TOGGLE, ONIFOFF, OFFIFON }
    [SerializeField] TargetBehavior _targetBehavior;
    [SerializeField] List<BON_Actionnable> _actionnables;

    /*
     * CLASS METHODS
     */
    public override void Happen()
    {
        foreach (BON_Actionnable actionnable in _actionnables)
        {
            switch (_targetBehavior) 
            {
                case TargetBehavior.ON: { actionnable.Status = true ; break; }
                case TargetBehavior.OFF: { actionnable.Status = false; break; }
                case TargetBehavior.TOGGLE: { actionnable.Toggle(); break; }
                case TargetBehavior.ONIFOFF: { if (actionnable.Status == false) { actionnable.Status = true; } break; }
                case TargetBehavior.OFFIFON: { if (actionnable.Status == true) { actionnable.Status = false; } break; }
            }
        }
    }
}
