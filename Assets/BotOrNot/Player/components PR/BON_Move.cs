using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class BON_Move : MonoBehaviour
{
    /*
     *  FIELDS
     */
    InputAction MoveAction;

    Rigidbody _parentRigidBody;

    [SerializeField] float _acceleration;   // Unit is m/s/s;
    [SerializeField] float _maxSpeed;       // Should be used only with ForceMode.Acceleration, Unit is m/s/s.
    private Vector3 _accumulatedMoveForce;



    /*
     *  UNITY METHODS
     */
    void Start()
    {
        MoveAction = InputSystem.actions.FindAction("Player/Move");
        _parentRigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        /* Read input value */
        Vector2 moveInputValue = MoveAction.ReadValue<Vector2>();

        /* Updates the accumulated force the PR is using to move around, and clamps it if needed */
        _accumulatedMoveForce = new Vector3(Mathf.Clamp(moveInputValue.x * _acceleration, -_maxSpeed, _maxSpeed),
                                            0.0f/*Mathf.Clamp(moveInputValue.y * _acceleration, -_maxAccel, _maxAccel)*/,
                                            0.0f);

        /* Resets velocity, and then apply the accumulated force */
        //_parentRigidBody.velocity = Vector3.zero;
        _parentRigidBody.AddForce(_accumulatedMoveForce, ForceMode.Force);



        //GetComponent<Rigidbody>().AddForce(new Vector3(moveValue.x, moveValue.y, 0.0f), ForceMode.Acceleration);
        //Mathf.Clamp(GetComponent<Rigidbody>().velocity.x, 0.0f, _maxSpeed);
        //Mathf.Clamp(GetComponent<Rigidbody>().velocity.y, 0.0f, _maxSpeed);
        //transform.position += new Vector3(moveValue.x * Time.deltaTime, 0, 0);
    }
}
