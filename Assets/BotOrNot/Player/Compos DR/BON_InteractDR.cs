using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BON_SwitchPlayerDR : MonoBehaviour
{
    /*
     *  FIELDS
     */
    InputAction _switchPlayerAction;

    // player script reference
    private BON_CCPlayer _player;
    //inventory reference
    private BON_Inventory _inventory;


    /*
     *  UNITY METHODS
     */

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindFirstObjectByType<BON_CCPlayer>();
        _switchPlayerAction = InputSystem.actions.FindAction("ActionsMapDR/SwitchPR");
    }

    // Update is called once per frame
    void Update()
    {
        if (_switchPlayerAction.WasPressedThisFrame() ) //interact => switch player button
        {
            if (!BON_GameManager.Instance().IsSwitching && !_player.AvatarState.IsNearHumanoidObject) //Pas d'interact humain a porté et pas deja en train de switch
            {
                StartCoroutine(BON_GameManager.Instance().CooldownSwitchControl());
                BON_GameManager.Instance().SwitchPlayer();
            }
        }
    }
}
