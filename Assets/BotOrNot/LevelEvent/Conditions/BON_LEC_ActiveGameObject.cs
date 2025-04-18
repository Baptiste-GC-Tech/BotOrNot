using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_LEC_ActiveGameObject : BON_LevelEventCondition
{
    /*
     * FIELDS
     */
    [SerializeField, Tooltip("0 means all must be active")] int _minimumActiveAmount;
    [SerializeField, Tooltip("should the objects be active or not to validate condition")] bool _shouldBeActive;
    [SerializeField] GameObject[] _gameObjects;

    /*
     * CLASS METHODS
     */
    public override bool Check()
    {
        if (_minimumActiveAmount == 0)
        {
            foreach (var obj in _gameObjects)
            {
                if (obj.activeSelf != _shouldBeActive)
                {
                    return false;
                }
            }
        }
        else
        {
            int total = 0;

            foreach (var obj in _gameObjects)
            {
                if (obj.activeSelf == _shouldBeActive)
                {
                    total++;
                }
            }

            if (total < _minimumActiveAmount)
            {
                return false;
            }
        }
        return true;
    }
}
