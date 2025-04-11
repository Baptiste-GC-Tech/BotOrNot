using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BON_InteractDR : MonoBehaviour
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
        _interactAction = InputSystem.actions.FindAction("ActionsMapDR/Interact");
        _inventory = GetComponent<BON_Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_interactAction.WasPressedThisFrame() ) //interact => switch player button
        {
            if (!BON_GameManager.Instance().IsSwitching && !_player.AvatarState.IsNearHumanoidObject) //Pas d'interact humain a porté et pas deja en train de switch
            {
                StartCoroutine(BON_GameManager.Instance().CooldownSwitchControl());
                BON_GameManager.Instance().SwitchPlayer();
            }
        }
    }
}
