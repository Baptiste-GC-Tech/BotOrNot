using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_LET_TagExit : BON_LevelEventTrigger
{
    /*
     * FIELDS
     */

    [SerializeField] string _targetTag;

    /*
     * UNITY METHODS
     */

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(_targetTag))
        {
            Try();
        }
    }
}
