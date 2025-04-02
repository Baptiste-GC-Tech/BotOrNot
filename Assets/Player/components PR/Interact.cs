using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    /*
     *  FIELDS
     */
    InputAction TakeAction;

    // player script reference
    [SerializeField] private CCPlayer player;
    //inventory reference
    private Inventory _inventory;


    // Start is called before the first frame update
    void Start()
    {
        TakeAction = InputSystem.actions.FindAction("Player/Take");
        _inventory = GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        // Take item action handling
        if (TakeAction.WasPressedThisFrame()) //interact
        {
            if (player.IsCollectibleInRange && player.Collectible != null) //item a porté
            {
                //stock in inventory

                print(player.Collectible);

                _inventory.AddItem(player.Collectible);

                player.Collectible.SetActive(false);

                player.Collectible = null;
            }
            else if (player.IsDRInRange) //dame robot pas loin
            {
                //give inventory item(s) to DR

                print(_inventory.CountItem() +" objets déposées");

                for(int i = _inventory.CountItem()-1; i> 0; i--)
                {
                    _inventory.DeleteItem(i);
                }
            }
        }
    }
}
