using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder;

// TODO: Implement the pause of accelaration and speed update when in the air
public class BON_MovePR : MonoBehaviour
{
    /*
     *  FIELDS
     */


    // DEBUG
    public Vector3 _MovThisFrame;
    private Vector3 _prevMoveDir;






    /* Objects & GO related */
    [Header("Player")]
    [SerializeField] private BON_CCPlayer _player;
    private Rigidbody _rb;

    /* Input related */
    private InputAction _MoveAction;
    [SerializeField] Canvas _canvas;    // Used only in Start() --> This should go away
    private BON_COMPJoystick _joystick;
    private Vector2 _moveInputValue;
    public Vector2 MoveInputValue
    { get { return _moveInputValue; }}


    /* Speed related */
    [Space]
    [Header("Speed")]
    [SerializeField] float _maxSpeed;
    [SerializeField, Range(0f, 1f)] float _rotationLerpSpeed = 0.025f;
    private float _curSpeed;
    public float CurSpeed
    {
        get { return _curSpeed; }
        set { _curSpeed = value; }
    }

    /* Acceleration related */
    [Space]
    [Header("Acceleration")]
    [SerializeField] AnimationCurve _AccelOverSpeed;
    [SerializeField] AnimationCurve _DeccelOverSpeed;

    /* Direction related */
    private int _moveXAxisDir;
    public int MoveXAxisDir
    {
        get { return _moveXAxisDir; }
    }
    private Vector3 _curMoveDir;
    private Vector3 _groundNormalVect;
    private CapsuleCollider _PRCollider;       // Used to scale relatively the ground check raycast

    /* Drift related */
    [Space]
    [Header("Drift")]
    [SerializeField] private float _driftDuration = 0.5f;
    public float DriftDuration
    { get { return _driftDuration; } }
    [SerializeField] private float _driftAcceleration = 400.0f;
    public float DriftAcceleration
        { get { return _driftAcceleration; } }
    [SerializeField, Range(0, 1)] private float _timeBetweenDrifts = 0.3f;
    private Vector3 _desiredDirection;
    private float _driftTimer;
    private Vector2 _previousDirection;
    private bool _isFirstMove = true;
    private bool _isPLayerMoving = false;
    private float _timeSinceLastMove = 0;
    public float TimeSinceLastMove
    { get { return _timeSinceLastMove; } }

    /* Bounce related */
    [Space]
    [Header("Bounce")]
    [SerializeField] int _numberOfBounce = 3;
    [SerializeField] float _bounceHeight = 5.0f;
    [SerializeField] float _heightBonceStart = 6.0f;
    private bool _isBouncing;
    public bool IsBouncing
    { get { return _isBouncing; } }
    private Vector3 _fallHeight;
    public Vector3 FallHeight
        { get { return _fallHeight; } }
    private int _bounceCount;
    public int BounceCount
    { get { return _bounceCount; } }

    /* animator related */
    private float _dot;
    private Vector2 _currentDir;
    private bool _didTurnBack;
    private bool _isSpeedHighEnough;
    private bool _triggerSkid;
    private bool _triggerStop;

    /* for debugTool */
    public string Layer = "";
    public string Tag = null;

    /* detection layer related*/
    private Vector3 _collisionPos;
    private Vector3 _collisionNormal;


    //Properties are mainly created for debugTool

    /*
     *  CLASS METHODS
     */
    // Calculates the current speed
    private void UpdateCurSpeed()
    {

        //float deFactoMaxSpeed = _maxSpeed * Mathf.Abs(_moveInputValue.x) * _SpeedMultiplierOverSlope.Evaluate(_groundNormalVect.y);  // This speed depends on the intensity of the player's input
        float deFactoMaxSpeed = _maxSpeed * Mathf.Abs(_moveInputValue.x);
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
            _curSpeed -= _DeccelOverSpeed.Evaluate(_curSpeed) * _maxSpeed * Time.deltaTime * 2;
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
        if (_moveXAxisDir != 0)
        {
            transform.eulerAngles = _moveXAxisDir == 1 ? (Vector3.Lerp(transform.eulerAngles, new Vector3(0, 90, 0), _rotationLerpSpeed)) : (Vector3.Lerp(transform.eulerAngles, new Vector3(0, 270, 0), _rotationLerpSpeed));    // TODO: make it a rotation, no ?
        }
        if (transform.eulerAngles.y - 90 < 0.1 && transform.eulerAngles.y - 90 > -0.1)
        {
            transform.eulerAngles = new Vector3(0, 90, 0);
        }
        if (transform.eulerAngles.y - 270 < 0.1 && transform.eulerAngles.y - 270 > -0.1)
        {
            transform.eulerAngles = new Vector3(0, 270, 0);
        }
        // Case of a flat ground : uses the forward direction instead of doing math
        if (Mathf.Approximately(_groundNormalVect.y, 1.0f))
        {
            _curMoveDir = Vector3.forward;
        }
        // Case of a sloped ground : finds the tangent to the normal of the ground mathematically, to then find a logically equivalent moveDir
        else
        {
            Vector3 crossBTerm = _moveXAxisDir == 1 ? Vector3.forward : Vector3.back;
            Vector3 worldSpaceMoveDir = Vector3.Cross(_groundNormalVect, crossBTerm);
            _curMoveDir.x = 0;
            _curMoveDir.y = worldSpaceMoveDir.y;
            _curMoveDir.z = worldSpaceMoveDir.x * _moveXAxisDir;
        }

        //Debug.Log("moveDirThisFrame : " + _curMoveDir);
    }

    // Updates the ground's normal that PR is standing on
    private void UpdateGroundNormal()
    {
        float groundRayLength = 0.55f * _PRCollider.height * transform.localScale.x;

        RaycastHit groundRaycastHit;
        Debug.DrawRay(transform.position, Vector3.down * groundRayLength, Color.green, Time.deltaTime);
        //Physics.Raycast(transform.position, Vector3.up, out hit, 100.0f, LayerMask.GetMask("Avatar"), QueryTriggerInteraction.Ignore);
        Physics.Raycast(transform.position, Vector3.down, out groundRaycastHit, groundRayLength);
        if (groundRaycastHit.collider != null) _groundNormalVect = groundRaycastHit.normal;

        //Debug.Log("_groundNormalVect : " + _groundNormalVect);
    }

    private void StopMove()
    {
        _moveInputValue.x = 0; //stop input
        _curSpeed = 0f; //stop speed
        _rb.velocity = Vector3.zero;
    }

    /*
     *  UNITY METHODS
     */
    void Start()
    {
        _MoveAction = InputSystem.actions.FindAction("ActionsMapPR/Move");
        _joystick = _canvas.GetComponentInChildren<BON_COMPJoystick>();
        _rb = GetComponent<Rigidbody>();
        _PRCollider = GetComponent<CapsuleCollider>();

        if (_bounceHeight >= _heightBonceStart)
        {
            _heightBonceStart = _bounceHeight + 1;
        }
        _previousDirection = _moveInputValue.normalized;
        _player.AvatarState.IsAgainstWallRight = false; // init false in avatarState but stuck at True (?????)
        _player.AvatarState.IsAgainstWallLeft = false; //

        _prevMoveDir = _curMoveDir;
        _player.AvatarState.IsDrifting = false;
    }

    void Update()
    {
        // We do this first since it's always good data to have
        UpdateGroundNormal();

        /* Handles the input */
        _moveInputValue = _MoveAction.ReadValue<Vector2>();
#if UNITY_EDITOR && !UNITY_ANDROID
#elif UNITY_ANDROID
        //_moveInputValue = _joystick.InputValues;
#endif

        // if input + wall on right/left, stop 
        if ((_moveInputValue.x < 0 && _player.AvatarState.IsAgainstWallLeft) || (_moveInputValue.x > 0 && _player.AvatarState.IsAgainstWallRight)) 
        {
            StopMove();
        }
        if (!_player.AvatarState.HasCableOut || _player.AvatarState.IsGrounded)
        {
            UpdateMoveDirFromInput();
            UpdateCurSpeed();
        }
        else
        {
            _curSpeed = 0f;
        }

        if (_player.AvatarState.IsGrounded && _rb.useGravity)
        {
            _rb.useGravity = false;
        }
        else if (!_player.AvatarState.IsGrounded && !_rb.useGravity)
        {
            _rb.useGravity = true;
        }


        _desiredDirection = transform.TransformDirection(_moveInputValue.normalized);
        //Drift
        if (_desiredDirection != Vector3.zero)
        {
            if (_isFirstMove)
            {
                _isFirstMove = false;
                _previousDirection = _moveInputValue.normalized;
                _player.AvatarState.IsDrifting = false;
            }
            if (_previousDirection != _moveInputValue.normalized)
            {
                _previousDirection = _moveInputValue.normalized;
                _player.AvatarState.IsDrifting = true;
                _driftTimer = _driftDuration;
            }
            if (_driftTimer > 0)
            {
                _driftTimer -= Time.deltaTime;
                _curSpeed = Mathf.Lerp(0, _curSpeed, Time.deltaTime * _driftAcceleration);
                _curMoveDir = -_curMoveDir;
            }
            else
            {
                _player.AvatarState.IsDrifting = false;
            }
        }
        if (_isPLayerMoving)
        {
            _timeSinceLastMove = 0;
            _player.AvatarState.IsMovingByPlayer = true;
        }
        else
        {
            _timeSinceLastMove += Time.deltaTime;
        }

        if (_timeSinceLastMove > _timeBetweenDrifts)
        {
            _isFirstMove = true;
            _player.AvatarState.IsMovingByPlayer = false;
        }

        // Bounce
        if ((_fallHeight.y - transform.position.y) >= _heightBonceStart && !_isBouncing)
        {
            _isBouncing = true;
            _bounceCount = 0;
        }

        /* Changes the state */
        if (_moveInputValue.y != 0 || _moveInputValue.x != 0) //if player move once, change state
        {
            _isPLayerMoving = true;
        }
        else
        {
            _isPLayerMoving = false;
        }

        //print(_curSpeed);

        /* Applies the movement, except if the cable is in use */
        if (!_player.AvatarState.HasCableOut && _player.AvatarState.IsGrounded)
        {
            //Debug.Log("MovDir : " + _curMoveDir + ", Speed : " + _curSpeed);
            Vector3 movementThisFrame = _curMoveDir * _curSpeed * Time.deltaTime;
            _MovThisFrame = movementThisFrame;
            movementThisFrame.x = 0.0f;     // Hard-coded constraint that prevent movement to the local left or right (Z-axis)
            if (_player.transform.position.z != 0)
            {
                movementThisFrame.y = -_player.transform.position.z;
            }
            transform.Translate(movementThisFrame);
        }
        //Debug.Log("Movement this frame : " + movementThisFrame);

        //Debug.Log("moveDir : " + _curMoveDir);
        if (_prevMoveDir != _curMoveDir) Debug.Log("New moveDir : " + _curMoveDir);
        _prevMoveDir = _curMoveDir;

        Animator animator = _player.GetComponentInChildren<Animator>();
         if (animator != null)
         {
             animator.SetFloat("Speed", _curSpeed);

             Vector2 currentDir = _moveInputValue.normalized;
             float dot = Vector2.Dot(_previousDirection, currentDir);

             bool didTurnBack = dot < -0.8f;
             bool isSpeedHighEnough = _curSpeed > (_maxSpeed * 0.5f);

             if (_moveInputValue.magnitude > 0.1f)
                 _previousDirection = currentDir;
         }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            if (collision.contactCount > 0)
            {
                _collisionPos = collision.GetContact(0).point; //contact point
                _collisionNormal = transform.position - _collisionPos;

                if (Mathf.Abs(_collisionNormal.y) > Mathf.Abs(_collisionNormal.x)) //collide on Y => floor
                {
                    _player.AvatarState.IsGrounded = true;
                }
            }
        }

        if (_isBouncing && !_player.AvatarState.IsGrounded)
        {
            _bounceCount++;
            _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
            _rb.AddForce(Vector3.up * (_bounceHeight / _bounceCount), ForceMode.Impulse);

            if (_bounceCount >= _numberOfBounce)
            {
                _isBouncing = false;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        Tag = collision.gameObject.tag;
        Layer = LayerMask.LayerToName(collision.gameObject.layer);

        if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            if (collision.contactCount > 0)
            {
                _collisionPos = collision.GetContact(0).point; //contact point
                _collisionNormal = _collisionPos - transform.position;

                if (Mathf.Abs(_collisionNormal.y) > Mathf.Abs(_collisionNormal.x)) //collide on Y => floor
                {
                    _player.AvatarState.IsGrounded = true;
                }
            }
        }

        if (_isBouncing && !_player.AvatarState.IsGrounded)
        {
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            _player.AvatarState.IsGrounded = false;
            _fallHeight = gameObject.transform.position;
        }
        Tag = null;
        Layer = null;
    }
}
