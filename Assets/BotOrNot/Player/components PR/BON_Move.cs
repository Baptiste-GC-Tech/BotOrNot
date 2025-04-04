using System.Collections;
using System.Collections.Generic;
using System.Data;
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
    private Vector3 _curMoveDir;
    

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
        // We do this first since it's always good data to have
        UpdateGroundNormal();

        /* Handles the input */
        moveInputValue = MoveAction.ReadValue<Vector2>();
        UpdateMoveDirFromInput(moveInputValue);
        UpdateCurSpeed(moveInputValue);


        /* Applies the movement */
        transform.Translate(new Vector3(_curMoveDir.x * _curSpeed, _curMoveDir.y * _curSpeed, 0.0f) * Time.deltaTime);
    }

    /*
     *  CLASS METHODS
     */
    // Calculates the current speed
    private void UpdateCurSpeed(Vector2 ARGinputValue)
    {
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
    }

    // Calculates the current movement direction induced by a player input 
    private void UpdateMoveDirFromInput(Vector2 ARGinputValue)
    {
        // Case in which there is no input : don't touch anything
        if (Mathf.Approximately(ARGinputValue.x, 0.0f) && Mathf.Approximately(ARGinputValue.y, 0.0f)) return;

        _moveXAxisDir = moveInputValue.x > 0.0f ? 1 : -1;

        // Case of a flat ground : uses either forward directions instead of doing math
        if (Mathf.Approximately(_groundNormalVect.y, 1.0f)) _curMoveDir = Vector3.right * _moveXAxisDir;
        // Case of a sloped ground : finds the tangent to the normal depending on which direction we are going towards, and applies a speed debuff
        else
        {
            _curMoveDir = _moveXAxisDir == 1 ? Vector3.Cross(_groundNormalVect, Vector3.forward) : Vector3.Cross(Vector3.forward, _groundNormalVect);
            _curMoveDir *= _slopedSpeedCurve.Evaluate(_groundNormalVect.y);
        }

        //Debug.Log("moveDirThisFrame : " + moveDirThisFrame);
    }

    // Updates the ground's normal that PR is standing on
    private void UpdateGroundNormal()
    {
        RaycastHit groundRaycastHit;
        Debug.DrawRay(transform.position, Vector3.down * 1.5f, Color.red, Time.deltaTime);
        //Physics.Raycast(transform.position, Vector3.up, out hit, 100.0f, LayerMask.GetMask("Avatar"), QueryTriggerInteraction.Ignore);
        Physics.Raycast(transform.position, Vector3.down, out groundRaycastHit, 1.5f);
        if (groundRaycastHit.collider != null) _groundNormalVect = groundRaycastHit.normal;

        //Debug.Log("_groundNormalVect : " + _groundNormalVect);
    }
}
