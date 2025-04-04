using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BON_InteractDR : MonoBehaviour
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
        if (InteractAction.WasPressedThisFrame() && !player.IsSwitching) //interact => switch player button
        {
            print("switch to nut");
            StartCoroutine(player.CooldownSwitchControl());
            player.SwitchPlayer();
        }

        if (player.IsCollectibleInRange && player.Collectible != null) //item a porté
        {
            //stock in inventory

            print(player.Collectible);

            _inventory.AddItem(player.Collectible);

            player.Collectible.SetActive(false);

            player.Collectible = null;
        }
    }
}
