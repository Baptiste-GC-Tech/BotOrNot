using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class BON_MoveDR : MonoBehaviour
{
    /*
     *  FIELDS
     */
    /* actions  related */
    private InputAction MoveAction;
    private InputAction JumpAction;
    InputAction InteractAction;
    Vector2 moveInputValue;

    // player script reference
    [SerializeField] private BON_CCPlayer player;

    /* Curve related */
    private float _timeSinceAccelStart, _timeSinceDeccelStart;
    private float _maxAccelTime, _maxDeccelTime;


    /* Direction related */
    private int _moveXAxisDir;
    private Vector3 _groundNormalVect;  // TODO: Use it to apply the movement using the normal. NOTE: Could be bad since we'd have the same speed everywhere. Either use a force, damp it down, or leave it as it is actually.


    /*
     *  UNITY METHODS
     */
    void Start()
    {
        MoveAction = InputSystem.actions.FindAction("ActionsMapDR/Move");
        JumpAction = InputSystem.actions.FindAction("ActionsMapDR/Jump");
        InteractAction = InputSystem.actions.FindAction("ActionsMapDR/Interact");
    }

    void Update()
    {
        /* Read input value */
        moveInputValue = MoveAction.ReadValue<Vector2>();
        transform.position += new Vector3(moveInputValue.x * Time.deltaTime, 0, 0);

        if (JumpAction.WasPressedThisFrame())
        {
            print("jump");
            GetComponent<Rigidbody>().AddForce(new Vector3(0,1000,0));
        }

        if (InteractAction.WasPressedThisFrame() && !player.IsSwitching)
        {
            print("switch to nut");
            StartCoroutine(player.CooldownSwitchControl());
            player.SwitchPlayer();
        }
    }
}
