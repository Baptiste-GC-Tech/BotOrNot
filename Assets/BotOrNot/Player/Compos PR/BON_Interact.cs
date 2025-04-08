using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BON_Interact : MonoBehaviour
{
    /*
     *  FIELDS
     */
    InputAction _ItemInteractAction;

    // player script reference
    [SerializeField] private BON_CCPlayer _player;
    //inventory reference
    private BON_Inventory _inventory;


    /*
     *  UNITY METHODS
     */

    void Start()
    {
        _ItemInteractAction = InputSystem.actions.FindAction("ActionsMapPR/Interact");
        _inventory = GetComponent<BON_Inventory>();
    }

    void Update()
    {
        // Take item action handling
        if (_ItemInteractAction.WasPressedThisFrame()) //interact
        {
            if (_player.IsDRInRange) //dame robot pas loin
            {
                //give inventory item(s) to DR

                print(_inventory.CountItem() +" objets d�pos�es");

                for(int i = _inventory.CountItem()-1; i> 0; i--)
                {
                    _inventory.DeleteItem(i);
                }
            }
        }
    }
}
