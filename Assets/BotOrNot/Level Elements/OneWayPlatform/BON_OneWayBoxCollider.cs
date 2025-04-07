using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class BON_OneWayBoxCollider : MonoBehaviour
{
    /*
     * FIELDS
     */
    [SerializeField] Vector3 _entryDirecton = Vector3.up;
    [SerializeField] bool _localDirection = false;
    [SerializeField, Range(1.0f, 2.0f)] float _triggerscale = 1.25f;
    BoxCollider _collider = null;
    BoxCollider _collisionCheckTrigger;

    /*
     * UNITY METHODS
     */
    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.isTrigger= false;

        _collisionCheckTrigger = gameObject.AddComponent<BoxCollider>();
        _collisionCheckTrigger.size = _collider.size * _triggerscale;
        _collisionCheckTrigger.center = _collider.center;
        _collisionCheckTrigger.isTrigger = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (Physics.ComputePenetration(_collisionCheckTrigger, transform.position, transform.rotation, other, other.transform.position, other.transform.rotation, out Vector3 collisionDirection, out float penetrationDepth))
        {
            Vector3 direction;
            if (_localDirection)
            {
                direction = transform.TransformDirection(_entryDirecton.normalized);
            }
            else
            {
                direction = _entryDirecton.normalized;
            }

            float dot = Vector3.Dot(direction, collisionDirection);
            //Opposite direction, passing not allowed
            if (dot < 0)
            {
                Physics.IgnoreCollision(_collider, other, false);
            }
            else
            {
                Physics.IgnoreCollision(_collider, other, true);
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        Vector3 direction;
        if (_localDirection)
        {
            direction = transform.TransformDirection(_entryDirecton.normalized);
        }
        else
        {
            direction = _entryDirecton.normalized;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, direction);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, -direction);
    }
}
