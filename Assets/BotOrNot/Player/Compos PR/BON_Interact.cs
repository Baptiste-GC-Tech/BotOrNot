using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BON_Interact : MonoBehaviour
{
    /*
     *  FIELDS
     */
    InputAction _interactAction;

    // player script reference
    [SerializeField] private BON_CCPlayer _player;
    //inventory reference
    private BON_Inventory _inventory;


    /*
     *  UNITY METHODS
     */

    // Start is called before the first frame update
    void Start()
    {
        _interactAction = InputSystem.actions.FindAction("ActionsMapPR/Interact");
        _inventory = GetComponent<BON_Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        // Take item action handling
        if (_interactAction.WasPressedThisFrame()) //interact
        {
            if (_player.IsDRInRange) //dame robot pas loin
            {
                //give inventory item(s) to DR

                print(_inventory.CountItem() +" objets déposées");

                for(int i = _inventory.CountItem()-1; i> 0; i--)
                {
                    _inventory.DeleteItem(i);
                }
            }
            else if (!_player.IsSwitching && !_player.IsMachineInRange) //Pas de machine a porté et pas deja en train de switch
            {
                StartCoroutine(_player.CooldownSwitchControl());
                _player.SwitchPlayer();
            }
        }
    }
}
