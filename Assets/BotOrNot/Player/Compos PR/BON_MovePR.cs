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
public class BON_MovePR : MonoBehaviour
{
    /*
     *  FIELDS
     */
    /* Objects & GO related */
    [Header("Player")]
    [SerializeField] private BON_CCPlayer _player;
    private Rigidbody _rb;

    public LayerMask Deez;

    /* Input related */
    private InputAction _MoveAction;
    [SerializeField] Canvas _canvas;    // Used only in Start() --> This should go away
    private BON_COMPJoystick _joystick;
    Vector2 _moveInputValue;


    /* Speed related */
    [Space]
    [Header("Speed")]
    [SerializeField] float _maxSpeed;
    [SerializeField] AnimationCurve _SpeedMultiplierOverSlope;   // The axis represent the y component of the normal's value
    private float _curSpeed;

    /* Accelartion related */
    [Space]
    [Header("Acceleration")]
    [SerializeField] AnimationCurve _AccelOverSpeed;
    [SerializeField] AnimationCurve _DeccelOverSpeed;

    /* Direction related */
    private int _moveXAxisDir;      // Useless now since we are actually rotating the GO instead
    private Vector3 _curMoveDir;
    private Vector3 _groundNormalVect;

    private Vector3 _prevMoveDir;

    /* Drift related */
    [Space]
    [Header("Drift")]
    [SerializeField] private float _driftDuration = 0.5f;
    [SerializeField] private float _driftAcceleration = 400.0f;
    [SerializeField, Range(0, 1)] private float _timeBetweenDrifts = 0.3f;
    private Vector3 _desiredDirection;
    private float _driftTimer;
    private Vector2 _previousDirection;
    private bool _isFirstMove = true;
    private bool _needToResetDrift = false;
    private float _timeSinceLastMove = 0;

    /* Bounce related */
    [Space]
    [Header("Bounce")]
    [SerializeField] int _numberOfBounce = 3;
    [SerializeField] float _bounceHeight = 5.0f;
    [SerializeField] float _heightBonceStart = 6.0f;
    private bool _isBouncing;
    private Vector3 _fallHeight;
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
    }

    // Calculates the current movement direction induced by a player input 
    private void UpdateMoveDirFromInput()
    {
        // Updates only if there is an input on the X-axis
        if (!Mathf.Approximately(_moveInputValue.x, 0.0f))
            _moveXAxisDir = _moveInputValue.x > 0.0f ? 1 : -1;

        // Turns the PR around
        transform.eulerAngles = _moveXAxisDir == 1 ? new Vector3(0, 90, 0) : new Vector3(0, -90, 0);    // TODO: make it a rotation, no ?

        // Case of a flat ground : uses the forward direction instead of doing math
        if (Mathf.Approximately(_groundNormalVect.y, 1.0f)) _curMoveDir = Vector3.forward;
        // Case of a sloped ground : finds the tangent to the normal of the ground mathematically, to then find a logically equivalent moveDir
        else
        {
            Vector3 crossBTerm = _moveXAxisDir == 1 ? Vector3.forward : Vector3.back;
            Vector3 worldSpaceMoveDir = Vector3.Cross(_groundNormalVect, crossBTerm);
            _curMoveDir.x = 0;
            _curMoveDir.y = worldSpaceMoveDir.y;
            _curMoveDir.z = worldSpaceMoveDir.x * _moveXAxisDir;

            //Debug.Log("Going towards " + _moveXAxisDir + " (X axis), given normal " + _groundNormalVect + " and that direction, crossBTerm = " + crossBTerm + ". Mathematically, we have " + worldSpaceMoveDir + " and logically " + _curMoveDir);
        }

        //Debug.Log("moveDirThisFrame : " + _curMoveDir);
    }

    // Updates the ground's normal that PR is standing on
    private void UpdateGroundNormal()
    {
        RaycastHit groundRaycastHit;
        Debug.DrawRay(transform.position, Vector3.down * 3f, Color.green, Time.deltaTime);
        //Physics.Raycast(transform.position, Vector3.up, out hit, 100.0f, LayerMask.GetMask("Avatar"), QueryTriggerInteraction.Ignore);
        Physics.Raycast(transform.position, Vector3.down, out groundRaycastHit, 3f);
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
        _previousDirection = _moveInputValue.normalized;
        _player.AvatarState.IsAgainstWallRight = false; // init false in avatarState but stuck at True (?????)
        _player.AvatarState.IsAgainstWallLeft = false; //

        _prevMoveDir = _curMoveDir;
    }

    void Update()
    {
        // We do this first since it's always good data to have
        UpdateGroundNormal();

        /* Handles the input */
#if UNITY_EDITOR
        _moveInputValue = _MoveAction.ReadValue<Vector2>();
#elif UNITY_ANDROID
        _moveInputValue = _joystick.InputValues;
#endif

        // if wall on right/left, stop input
        if (_moveInputValue.x < 0 && _player.AvatarState.IsAgainstWallLeft)
        {
            _moveInputValue.x = 0;
        }
        if (_moveInputValue.x > 0 && _player.AvatarState.IsAgainstWallRight)
        {
            _moveInputValue.x = 0;
        }

        UpdateMoveDirFromInput();
        UpdateCurSpeed();

        _desiredDirection = transform.TransformDirection(_moveInputValue.normalized);
        //Drift
        if (_desiredDirection != Vector3.zero)
        {
            if (_isFirstMove)
            {
                _isFirstMove = false;
                _previousDirection = _moveInputValue.normalized;
            }
            if (_previousDirection != _moveInputValue.normalized)
            {
                _previousDirection = _moveInputValue.normalized;
                _driftTimer = _driftDuration;
            }
            if (_driftTimer > 0)
            {
                Debug.Log("Drifting");
                _driftTimer -= Time.deltaTime;
                _curSpeed = Mathf.Lerp(0, _curSpeed, Time.deltaTime * _driftAcceleration);
                _curMoveDir = -_curMoveDir;
            }
        }
        if (_player.AvatarState.IsMovingByPlayer)
        {
            _timeSinceLastMove = 0;
        }
        else
        {
            _timeSinceLastMove += Time.deltaTime;
        }

        if (_timeSinceLastMove > _timeBetweenDrifts)
        {
            _isFirstMove = true;
        }

        // Bounce
        if (!_player.AvatarState.IsGrounded && (_fallHeight.y - transform.position.y) >= _heightBonceStart && !_isBouncing)
        {
            Debug.Log("Should enter bounce");
            _isBouncing = true;
            _bounceCount = 0;
        }

        /* Changes the state */
        if (_moveInputValue.y != 0 || _moveInputValue.x != 0) //if player move once, change state
        {
            _player.AvatarState.IsMovingByPlayer = true;
        }
        else
        {
            _player.AvatarState.IsMovingByPlayer = false;
        }

        //print(_curSpeed);

        /* Applies the movement */
        Vector3 movementThisFrame = _curMoveDir * _curSpeed * Time.deltaTime;
        movementThisFrame.x = 0.0f;     // Hard-coded constraint that prevent movement to the local left or right (Z-axis)
        transform.Translate(movementThisFrame);
        //Debug.Log("Movement this frame : " + movementThisFrame);

        //Debug.Log("moveDir : " + _curMoveDir);
        if (_prevMoveDir != _curMoveDir) Debug.Log("New moveDir : " + _curMoveDir);
        _prevMoveDir = _curMoveDir;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            _player.AvatarState.IsGrounded = true;
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
            _player.AvatarState.IsGrounded = false;
            _fallHeight = gameObject.transform.position;
        }
    }

}
