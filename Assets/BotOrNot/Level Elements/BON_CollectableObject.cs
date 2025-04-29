using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class BON_CollectableObject : MonoBehaviour
{
    /*
     FIELDS
     */
    [SerializeField] int _ObjectId;

    BON_Inventory _playerInventory;

    public int ObjectId { get { return _ObjectId; } }

    [SerializeField] GameObject _pickupVFXPrefab;
    /*
     * CLASS METHODS
     */

    public virtual void Pickup()
    {

        if(_playerInventory != null)
        {
            //give its id to player inventory
            _playerInventory.AddItem(_ObjectId);
        }

        GameObject VFXObj = Instantiate(_pickupVFXPrefab, transform);
        VFXObj.transform.parent = null;
        VFXObj.transform.SetLocalPositionAndRotation(transform.position, Quaternion.identity);

        //die
        Destroy(gameObject);
    }

    /*
     * UNITY METHODS
     */

    private void Start()
    {
        _playerInventory = GameObject.FindWithTag("Player").GetComponent<BON_Inventory>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Pickup();
        }
    }
}
