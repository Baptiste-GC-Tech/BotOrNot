using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder;
using UnityEngine.Rendering;
using UnityEngine.Windows;

// TODO: Implement the pause of accelaration and speed update when in the air
public class BON_Move : MonoBehaviour
{
    /*
     *  FIELDS
     */

    // player reference
    [SerializeField] private BON_CCPlayer _player;
    private Rigidbody _rb;

    public LayerMask Deez;

    /* Input related */
    private InputAction _MoveAction;
    [SerializeField] Canvas _canvas;    // Used only in Start() --> This should go away
    private BON_COMPJoystick _joystick;
    Vector2 _moveInputValue;

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
    private Vector3 _groundNormalVect;

    /*Drift related*/
    [SerializeField] private float _driftDuration = 0.3f;
    [SerializeField] private float _driftAcceleration = 10f;
    private Vector3 _desiredDirection;
    private float _driftTimer;
    private Vector3 _currentVelocity;

    /*Bounce related*/
    private bool _isGrounded;
    private bool _isBouncing;
    private Vector3 _fallHeight;
    private Vector3 _landingHeight;
    [SerializeField] int _numberOfBounce = 2;
    [SerializeField] float _bounceHeight = 5.0f;
    [SerializeField] float _heightBonceStart = 6.0f;
    private int _bounceCount;

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
        _rb = GetComponent<Rigidbody>();
        if (_bounceHeight >= _heightBonceStart)
        {
            _heightBonceStart = _bounceHeight + 1;
        }
    }

    void Update()
    {
        // We do this first since it's always good data to have
        UpdateGroundNormal();

        /* Handles the input */
#if UNITY_EDITOR
        _moveInputValue = _joystick.InputValues;
        _moveInputValue = _MoveAction.ReadValue<Vector2>();
#elif UNITY_ANDROID
        _moveInputValue = _joystick.InputValues;
#endif

        //changing state in BON_Avatarstate
        if (_moveInputValue.y != 0 || _moveInputValue.x != 0 && _player.AvatarState.CurrentState != BON_AvatarState.States.Moving) //if player move once, change state
        {
            _player.AvatarState.ChangeState(BON_AvatarState.States.Moving);
        }

        UpdateMoveDirFromInput();
        UpdateCurSpeed();

        _desiredDirection = transform.TransformDirection(_moveInputValue.normalized);

        //Drift
        if (_desiredDirection != Vector3.zero)
        {
            if (Vector3.Dot(_currentVelocity.normalized, _desiredDirection.normalized) < -0.5f)
            {
                _driftTimer = _driftDuration; 
            }
            if (_driftTimer > 0)
            {
                _driftTimer -= Time.deltaTime;
                _currentVelocity = Vector3.Lerp(_currentVelocity, Vector3.zero, Time.deltaTime * _driftAcceleration);
            }
            else
            {
                Vector3 targetVelocity = _desiredDirection * _curSpeed;
                _currentVelocity = Vector3.Lerp(_currentVelocity, targetVelocity, Time.deltaTime * _driftAcceleration);
            }
        }

        //Bounce
        if (!_isGrounded && (_fallHeight.y - transform.position.y) >= _heightBonceStart && !_isBouncing)
        {
            Debug.Log("SHould enter bounce");
            _isBouncing = true;
            _bounceCount = 0;
        }

        /* Applies the movement */
        transform.Translate(_currentVelocity * Time.deltaTime, Space.World);
        //transform.Translate(new Vector3(_curMoveDir.x * _curSpeed, _curMoveDir.y * _curSpeed, 0.0f) * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            _isGrounded = true;
            _landingHeight = gameObject.transform.position;
        }
        if (_isBouncing)
        {
            Debug.Log("bouncing");
            _bounceCount++;
            _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
            _rb.AddForce(Vector3.up * (_bounceHeight / _bounceCount), ForceMode.Impulse);

            if (_bounceCount >= _numberOfBounce)
            {
                _isBouncing = false;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            _isGrounded = false;
            _fallHeight = gameObject.transform.position;
        }
    }

}
