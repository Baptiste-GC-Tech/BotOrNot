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
        InteractAction = InputSystem.actions.FindAction("ActionsMapDR/Interact");
        _inventory = GetComponent<BON_Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (InteractAction.WasPressedThisFrame() ) //interact => switch player button
        {
            if (!player.IsSwitching && !player.IsMachineInRange) //Pas de machine a porté et pas deja en train de switch
            {
                StartCoroutine(player.CooldownSwitchControl());
                player.SwitchPlayer();
            }
        }
    }
}
