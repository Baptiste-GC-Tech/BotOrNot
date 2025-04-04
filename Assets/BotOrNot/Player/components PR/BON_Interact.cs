using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BON_Interact : MonoBehaviour
{
    /*
     *  FIELDS
     */
    InputAction InteractAction;

    // player script reference
    [SerializeField] private BON_CCPlayer player;
    //inventory reference
    private BON_Inventory _inventory;


    /*
     *  UNITY METHODS
     */

    // Start is called before the first frame update
    void Start()
    {
        InteractAction = InputSystem.actions.FindAction("ActionsMapPR/Interact");
        _inventory = GetComponent<BON_Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        // Take item action handling
        if (InteractAction.WasPressedThisFrame()) //interact
        {
            print("ah tu veux switch ?");
            if (player.IsDRInRange) //dame robot pas loin
            {
                //give inventory item(s) to DR

                print(_inventory.CountItem() +" objets déposées");

                for(int i = _inventory.CountItem()-1; i> 0; i--)
                {
                    _inventory.DeleteItem(i);
                }
            }
            else if (!player.IsSwitching)
            {
                print("switch to dame robot");
                StartCoroutine(player.CooldownSwitchControl());
                player.SwitchPlayer();
            }
        }
    }
}
