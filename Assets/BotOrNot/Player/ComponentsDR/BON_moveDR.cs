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
    private InputAction _moveAction;
    private InputAction _jumpAction;
    Vector2 _moveInputValue;

    // player script reference
    [SerializeField] private BON_CCPlayer _player;

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
        _moveAction = InputSystem.actions.FindAction("ActionsMapDR/Move");
        _jumpAction = InputSystem.actions.FindAction("ActionsMapDR/Jump");
    }

    void Update()
    {
        /* Read input value */
        _moveInputValue = _moveAction.ReadValue<Vector2>();
        transform.position += new Vector3(_moveInputValue.x * Time.deltaTime, 0, 0);

        if (_jumpAction.WasPressedThisFrame())
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(0,350,0));
        }
    }
}
