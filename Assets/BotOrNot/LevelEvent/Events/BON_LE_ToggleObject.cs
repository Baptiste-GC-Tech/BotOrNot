using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_LE_ToggleObject : BON_LevelEvent
{
    /*
     * FIELDS
     */

    [SerializeField] List<GameObject> _objects;

    /*
     * CLASS METHODS
     */
    public override void Happen()
    {
        foreach (var obj in _objects) 
        {
            obj.SetActive(!obj.activeSelf);
        }

    }
}
