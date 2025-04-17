using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_Interactive_LevelEventTrigger : BON_Interactive
{
    /*
     *  FIELDS
     */

    [SerializeField]
    protected List<BON_LevelEventTrigger> _triggersList;
    
    /*
     *  CLASS METHODS
     */
    public override void Activate()
    {
        foreach (var trigger in _triggersList)
        {
            trigger.Try();
        }
    }
}
