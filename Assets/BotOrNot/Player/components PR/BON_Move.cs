using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder;

// TODO: Implement the pause of accelaration and speed update when in the air
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
    [SerializeField] AnimationCurve _slopedSpeedCurve;
    private float _curSpeed;

    /* Accelartion related */
    [SerializeField] AnimationCurve _AccelCurve;
    [SerializeField] AnimationCurve _DeccelCurve;

    /* Direction related */
    private int _moveXAxisDir;
    private Vector3 _groundNormalVect;
    

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


        /* Determines the direction using the ground's normal. If not ground is found, can't accelerate ( cuz wheel no touchy :( ) */
        if (moveInputValue.x != 0.0f) _moveXAxisDir = moveInputValue.x > 0.0f ? 1 : -1; // Doesn't update if their is no input.

        RaycastHit groundRaycastHit;
        Debug.DrawRay(transform.position, Vector3.down * 2.0f, Color.red, Time.deltaTime);
        //Physics.Raycast(transform.position, Vector3.up, out hit, 100.0f, LayerMask.GetMask("Avatar"), QueryTriggerInteraction.Ignore);
        Physics.Raycast(transform.position, Vector3.down, out groundRaycastHit, 2.0f);
        if (groundRaycastHit.collider != null) _groundNormalVect = groundRaycastHit.normal;
        //Debug.Log("_groundNormalVect : " + _groundNormalVect);


        /* Calculates the speed, then multiplies it to the slopeCurve (to simulate ease or hinder of travel on slopes) */
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


        /* Applies the movement */  
        Vector3 moveDirThisFrame;

        // Case of a flat ground : uses either forward directions instead of doing math
        if (Mathf.Approximately(_groundNormalVect.y, 1.0f)) moveDirThisFrame = Vector3.right * _moveXAxisDir;
        // Case of a sloped ground : finds the tangent to the normal depending on which direction we are going towards, and applies a speed debuff
        else
        {
            moveDirThisFrame = _moveXAxisDir == 1 ? Vector3.Cross(_groundNormalVect, Vector3.forward) : Vector3.Cross(Vector3.forward, _groundNormalVect);
            moveDirThisFrame *= _slopedSpeedCurve.Evaluate(_groundNormalVect.y);
        }

        //Debug.Log("moveDirThisFrame : " + moveDirThisFrame);

        transform.Translate(new Vector3(moveDirThisFrame.x * _curSpeed, moveDirThisFrame.y * _curSpeed, 0.0f) * Time.deltaTime);
    }
}
