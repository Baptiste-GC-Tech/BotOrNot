using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_LET_TagCollide : BON_LevelEventTrigger
{
    /*
     * FIELDS
     */

    [SerializeField] string _targetTag;

    /*
     * UNITY METHODS
     */

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(_targetTag))
        {
            Try();
        }
    }
}
