using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class BON_Move : MonoBehaviour
{
    /*
     *  FIELDS
     */
    public LayerMask deez;

    private InputAction MoveAction;
    Vector2 moveInputValue;

    /* Curve related */
    private float _timeSinceAccelStart, _timeSinceDeccelStart;
    private float _maxAccelTime, _maxDeccelTime;

    /* Speed related */
    [SerializeField] float _maxSpeed;
    private float _curSpeed;

    /* Accelartion related */
    [SerializeField] AnimationCurve _AccelCurve;
    [SerializeField] AnimationCurve _DeccelCurve;

    /* Direction related */
    private int _moveXAxisDir;
    private Vector3 _groundNormalVect;  // TODO: Use it to apply the movement using the normal. NOTE: Could be bad since we'd have the same speed everywhere. Either use a force, damp it down, or leave it as it is actually.
    

    /*
     *  UNITY METHODS
     */
    void Start()
    {
        MoveAction = InputSystem.actions.FindAction("ActionsMapPR/Move");

        /* Curve fields setup */
        _maxAccelTime = _AccelCurve.keys[_AccelCurve.keys.Length - 1].time;
        _maxDeccelTime = _DeccelCurve.keys[_DeccelCurve.keys.Length - 1].time;
        _timeSinceAccelStart = 0.0f;
        _timeSinceDeccelStart = _maxDeccelTime;
    }

    void Update()
    {
        /* Read input value */
        moveInputValue = MoveAction.ReadValue<Vector2>();

        /* Calculates the speed */
        switch (Mathf.Approximately(moveInputValue.x, 0.0f))
        {
            // Case in which we accelerate
            case false:
                /* Updates the timers (both in case the input starts and stops very quickly) */
                _timeSinceAccelStart = Mathf.Clamp(_timeSinceAccelStart + Time.deltaTime, 0.0f, _maxAccelTime);
                _timeSinceDeccelStart = Mathf.Clamp(_timeSinceDeccelStart - Time.deltaTime, 0.0f, _maxDeccelTime);

                // Use the acceleration timer to set the speed
                _curSpeed = _maxSpeed * _AccelCurve.Evaluate(_timeSinceAccelStart);
                break;

            // Case in which we deccelerate
            case true:
                /* Updates the timers (both in case the input starts and stops very quickly) */
                _timeSinceAccelStart = Mathf.Clamp(_timeSinceAccelStart - Time.deltaTime, 0.0f, _maxAccelTime);
                _timeSinceDeccelStart = Mathf.Clamp(_timeSinceDeccelStart + Time.deltaTime, 0.0f, _maxDeccelTime);

                // Use the decceleration timer to set the speed
                _curSpeed = _maxSpeed * _DeccelCurve.Evaluate(_timeSinceDeccelStart);
                break;
        }

        /* Updates the movement's direction as long as there is a input to read */
        if (moveInputValue.x != 0.0f) _moveXAxisDir = moveInputValue.x > 0.0f ? 1 : -1;

        /* Applies the movement */
        transform.Translate(new Vector3(_moveXAxisDir * _curSpeed, 0.0f, 0.0f) * Time.deltaTime);



        //
        // WIP
        //

        /* Snaps the player to the ground if he goes under it */
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.up, out hit, 100.0f, LayerMask.GetMask("Avatar"), QueryTriggerInteraction.Ignore);
        LineRenderer ligma = new LineRenderer();
            //lineRenderer.loop = false;
            //lineRenderer.positionCount = 2;
        Vector3[] lineRendPos = { transform.position, transform.position + 10.0f * Vector3.up };
        //ligma.SetPositions(lineRendPos);
        Debug.DrawRay(transform.position, Vector3.up);

        if (hit.collider != null) Debug.Log("hit : " + hit);
    }
}
