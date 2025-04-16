using UnityEngine;
using UnityEngine.InputSystem;

public class BON_InteractPR : MonoBehaviour
{
    /*
     *  FIELDS
     */
    InputAction _itemInteractAction;

    // player script reference
    [SerializeField] private BON_CCPlayer _player;
    //inventory reference
    private BON_Inventory _inventory;


    /*
     *  UNITY METHODS
     */

    void Start()
    {
        _itemInteractAction = InputSystem.actions.FindAction("ActionsMapPR/SwitchDR");
        _inventory = GetComponent<BON_Inventory>();
    }

    void Update()
    {
        // Take item action handling
        if (_itemInteractAction.WasReleasedThisFrame()) //interact
        {
            /*if (_player.IsDRInRange) //dame robot pas loin
            {
                //give inventory item(s) to DR

                print(_inventory.CountItem() +" objets d�pos�es");

                for(int i = _inventory.CountItem()-1; i> 0; i--)
                {
                    _inventory.DeleteItem(i);
                }
            }*/
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
