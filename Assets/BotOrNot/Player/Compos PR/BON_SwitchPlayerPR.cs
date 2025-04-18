using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BON_SwitchPlayerPR : MonoBehaviour
{
    /*
     *  FIELDS
     */
    InputAction _switchPlayerAction;

    // player script reference
    private BON_CCPlayer _player;


    /*
     *  UNITY METHODS
     */

    void Start()
    {
        _player = GameObject.FindFirstObjectByType<BON_CCPlayer>();
        _switchPlayerAction = InputSystem.actions.FindAction("ActionsMapPR/SwitchDR");
    }

    void Update()
    {
        // Take item action handling
        if (_switchPlayerAction.WasReleasedThisFrame()) //switch to DR
        {
            Debug.Log("click interact");
            if (!BON_GameManager.Instance().IsSwitching && !_player.AvatarState.IsNearIOMInteractible) //Pas de machine a porté et pas deja en train de switch
            {
                Debug.Log("switch DR");
                StartCoroutine(BON_GameManager.Instance().CooldownSwitchControl());
                BON_GameManager.Instance().SwitchPlayer();
            }
        }
    }
}
