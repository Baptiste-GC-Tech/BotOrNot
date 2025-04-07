using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder;
using UnityEngine.Rendering;
using static UnityEditor.Experimental.GraphView.GraphView;

// TODO: Implement the pause of accelaration and speed update when in the air
public class BON_Move : MonoBehaviour
{
    /*
     *  FIELDS
     */

    // player reference
    [SerializeField] private BON_CCPlayer _player;

    public LayerMask Deez;

    private InputAction _MoveAction;
    Vector2 _moveInputValue;

    ///* Curve related */
    //private float _timeSinceAccelStart, _timeSinceDeccelStart;
    //private float _maxAccelTime, _maxDeccelTime;

    /* Speed related */
    [SerializeField] float _maxSpeed;
    [SerializeField] AnimationCurve _SpeedMultiplierOverSlope;   // The axis represent the y component of the normal's value
    private float _curSpeed;

    /* Accelartion related */
    [SerializeField] AnimationCurve _AccelOverSpeed;
    [SerializeField] AnimationCurve _DeccelOverSpeed;

    /* Direction related */
    private int _moveXAxisDir;
    private Vector3 _curMoveDir;
    private Vector3 _groundNormalVect;  // TODO: Use it to apply the movement using the normal. NOTE: Could be bad since we'd have the same speed everywhere. Either use a force, damp it down, or leave it as it is actually.

    [SerializeField] Canvas _canvas;
    private BON_COMPJoystick _joystick;

    /*
     *  CLASS METHODS
     */
    // Calculates the current speed
    private void UpdateCurSpeed()
    {

        float deFactoMaxSpeed = _maxSpeed * Mathf.Abs(_moveInputValue.x) * _SpeedMultiplierOverSlope.Evaluate(_groundNormalVect.y);  // This speed depends on the intensity of the player's input
        float speedDelta = deFactoMaxSpeed - _curSpeed;

        //Debug.Log("defactoMax - cur = delta : " + deFactoMaxSpeed + " - " + _curSpeed + " = " + speedDelta);

        // Case in which we are below our de facto max speed : we want to go faster
        if (speedDelta > 0.0f)
        {
            _curSpeed += _AccelOverSpeed.Evaluate(_curSpeed) * _maxSpeed * Time.deltaTime;
            _curSpeed = Mathf.Clamp(_curSpeed, 0.0f, deFactoMaxSpeed);
        }
        // Case in which we are above our de facto max speed : we want to go slower
        if (speedDelta < 0.0f)
        {
            _curSpeed -= _DeccelOverSpeed.Evaluate(_curSpeed) * _maxSpeed * Time.deltaTime;
            _curSpeed = Mathf.Clamp(_curSpeed, 0.0f, _maxSpeed);
        }

        //// Applies the slope multiplier
        //_curSpeed *= _SpeedMultiplierOverSlope.Evaluate(_groundNormalVect.y);
        //Debug.Log(_SpeedMultiplierOverSlope.Evaluate(_groundNormalVect.y));
    }

    // Calculates the current movement direction induced by a player input 
    private void UpdateMoveDirFromInput()
    {
        // Case in which there is no input : don't touch anything
        if (Mathf.Approximately(_moveInputValue.x, 0.0f) && Mathf.Approximately(_moveInputValue.y, 0.0f)) return;

        _moveXAxisDir = _moveInputValue.x > 0.0f ? 1 : -1;

        // Case of a flat ground : uses either forward directions instead of doing math
        if (Mathf.Approximately(_groundNormalVect.y, 1.0f)) _curMoveDir = Vector3.right * _moveXAxisDir;
        // Case of a sloped ground : finds the tangent to the normal depending on which direction we are going towards, and applies a speed debuff
        else _curMoveDir = _moveXAxisDir == 1 ? Vector3.Cross(_groundNormalVect, Vector3.forward) : Vector3.Cross(Vector3.forward, _groundNormalVect);

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

    /*
     *  UNITY METHODS
     */
    void Start()
    {
        _MoveAction = InputSystem.actions.FindAction("ActionsMapPR/Move");
        _joystick = _canvas.GetComponentInChildren<BON_COMPJoystick>();

        ///* Curve fields setup */
        //_maxAccelTime = _AcceleratingSpeedCurve.keys[_AcceleratingSpeedCurve.keys.Length - 1].time;
        //_maxDeccelTime = _DecceleratingSpeedCurve.keys[_DecceleratingSpeedCurve.keys.Length - 1].time;
        //_timeSinceAccelStart = 0.0f;
        //_timeSinceDeccelStart = _maxDeccelTime;
    }

    void Update()
    {
        // We do this first since it's always good data to have
        UpdateGroundNormal();

        //_moveInputValue = _joystick.InputValues;

        /* Handles the input */
        _moveInputValue = _MoveAction.ReadValue<Vector2>();

        //changing state in BON_Avatarstate
        if (_moveInputValue != null ) 
        {
            _player.AvatarState.ChangeState(BON_AvatarState.States.Moving);
        }

        UpdateMoveDirFromInput();
        UpdateCurSpeed();


        /* Applies the movement */
        transform.Translate(new Vector3(_curMoveDir.x * _curSpeed, _curMoveDir.y * _curSpeed, 0.0f) * Time.deltaTime);
    }

}
