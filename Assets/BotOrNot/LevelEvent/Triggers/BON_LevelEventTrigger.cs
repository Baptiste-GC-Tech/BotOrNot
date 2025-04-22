using System.Collections.Generic;
using UnityEngine;

public class BON_LevelEventTrigger : MonoBehaviour
{
    /*
     * FIELDS
     */

    [SerializeField, Tooltip("0 means all must be active")] protected int _minimumConditionsToFulfill;
    [SerializeField] List<BON_LevelEvent> _events = new List<BON_LevelEvent>();
    [SerializeField] List<BON_LevelEventCondition> _conditions = new List<BON_LevelEventCondition>();

    /*
     * CLASS METHODS
     */

    public bool CheckConditions()
    {
        if(_conditions.Count == 0)
        {
            return true;
        }
        else if (_minimumConditionsToFulfill > 0)
        {
            int fulfilled = 0;
            foreach (var condition in _conditions)
            {
                if (condition.Check())
                {
                    fulfilled++;
                }
            }

            return fulfilled >= _minimumConditionsToFulfill;
        }
        else
        {
            foreach (var condition in _conditions)
            {
                if (condition.Check() == false) { return false; }
            }
            return true;
        }
    }

    public void TriggerEvents()
    {
        foreach (var myEvent in _events)
        {
            myEvent.Happen();
        }
    }

    public void Try()
    {
        if (CheckConditions())
        {
            TriggerEvents();
        }
    }

    /*
     * UNITY METHODS
     */

    private void Start()
    {
        /*
        foreach (var myEvent in GetComponents<BON_LevelEvent>())
        {
            _events.Add(myEvent);
        }

        foreach (var myCondition in GetComponents<BON_LevelEventCondition>())
        {
            _conditions.Add(myCondition);
        }
        */
    }
}
