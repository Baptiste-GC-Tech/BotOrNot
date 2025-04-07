using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BON_Cable : MonoBehaviour
{
    /*
     *  FIELDS
     */
    InputAction _cableAction;

    // player reference
    [SerializeField] private BON_CCPlayer _player;

    /*
     *  UNITY METHODS
     */

    // Start is called before the first frame update
    void Start()
    {
        _cableAction = InputSystem.actions.FindAction("ActionsMapPR/Cable");
    }

    // Update is called once per frame
    void Update()
    {
        
        if (_cableAction.WasPressedThisFrame()) //cable => ""jump""
        {
            _player.AvatarState.ChangeState(BON_AvatarState.States.ThrowingCable);
        }
    }
}
