using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class BON_CablePR : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera _mainCam;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _gunOrigin;
    [SerializeField] private LayerMask _raycastLayers;
    [SerializeField] private Transform _directionReference;
    [SerializeField] private BON_CCPlayer _player;

    [Header("Hook Settings")]
    [SerializeField] private float _rayDistance = 100f;
    [SerializeField] private float _springForce = 20f;
    [SerializeField] private float _damping = 5f;
    [SerializeField] private float _cableLengthSpeed = 5f;
    [SerializeField] private float _swingForce = 6f;

    [Header("Visual Settings")]
    [SerializeField] private int _lineSegments = 20;
    [SerializeField] private float _deployTime = 0.3f;
    [SerializeField] private float _waveAmplitude = 0.5f;
    [SerializeField] private float _waveFrequency = 2f;
    [SerializeField] private float _swingAmplitude = 0.1f;
    [SerializeField] private float _swingFrequency = 2f;

    [Header("Swing Settings")]
    [SerializeField] private float _lateralDamping = 1f;


    private bool _lineVisible = false;
    private bool _animating = false;
    private Vector3 _targetPoint;
    private float _swingTimer;

    private InputAction _clickAction;
    private InputAction _cablemoveDown;
    private InputAction _cablemoveUp;
    private InputAction _cablemoveLeft;
    private InputAction _cablemoveRight;

    private SpringJoint _joint;
    private Rigidbody _rb;
    private BON_MovePR _moveScript;

    private void Start()
    {
        _clickAction = InputSystem.actions.FindAction("ActionsMapPR/Cable");
        _cablemoveDown = InputSystem.actions.FindAction("ActionsMapPR/CablemoveDown");
        _cablemoveUp = InputSystem.actions.FindAction("ActionsMapPR/CablemoveUp");
        _cablemoveLeft = InputSystem.actions.FindAction("ActionsMapPR/CablemoveLeft");
        _cablemoveRight = InputSystem.actions.FindAction("ActionsMapPR/CablemoveRight");

        _rb = GetComponent<Rigidbody>();
        _moveScript = GetComponent<BON_MovePR>();

        if (_clickAction == null)
            Debug.LogError("L'action 'ActionsMapPR/Cable' est introuvable.");
    }

    private void Update()
    {
        if (_clickAction != null && _clickAction.triggered)
        {
            GererClic();
        }

        if (_lineVisible && _joint != null && !_animating)
        {
            PRIVAfficherLigne(_gunOrigin.position, _joint.connectedAnchor);
            _swingTimer += Time.deltaTime;
        }

        if (_joint != null)
        {
            float lengthChange = 0f;
            if (_cablemoveUp?.ReadValue<float>() > 0.5f)
                lengthChange -= _cableLengthSpeed * Time.deltaTime;
            if (_cablemoveDown?.ReadValue<float>() > 0.5f)
                lengthChange += _cableLengthSpeed * Time.deltaTime;
            _joint.maxDistance = Mathf.Clamp(_joint.maxDistance + lengthChange, 1f, _rayDistance);


            float swingInput = 0f;
            if (_cablemoveLeft != null && _cablemoveLeft.IsPressed()) swingInput = -1f;
            if (_cablemoveRight != null && _cablemoveRight.IsPressed()) swingInput = 1f;

            Vector3 toAnchor = _joint.connectedAnchor - transform.position;
            Vector3 horizontalToAnchor = new Vector3(toAnchor.x, 0f, toAnchor.z).normalized;
            Vector3 swingDir = Vector3.Cross(Vector3.up, horizontalToAnchor).normalized;

            if (swingInput != 0f)
            {
                Vector3 force = swingDir * swingInput * _swingForce;
                _rb.AddForce(force, ForceMode.VelocityChange);
            }
            else
            {
                Vector3 lateralVel = Vector3.Project(_rb.velocity, swingDir);
                _rb.velocity -= lateralVel * _lateralDamping * Time.deltaTime;
            }
        }

    }


    public void GererClic()
    {
        if (!_lineVisible)
        {
            Transform closest = PRIVTrouverPlusProcheHook(GameObject.FindGameObjectsWithTag("Hook"));

            if (closest != null)
            {
                _lineVisible = true;
                _lineRenderer.enabled = true;
                _lineRenderer.positionCount = _lineSegments;
                _targetPoint = closest.position;

                StartCoroutine(PRIVAnimerLigneAvecVague());

                BON_Interactive interactive = closest.GetComponent<BON_Interactive>();
                if (interactive != null)
                    interactive.Activate();

                _player.AvatarState.HasCableOut = true;
                if (_moveScript != null) _moveScript.enabled = false;
            }
        }
        else
        {
            
            StartCoroutine(PRIVRetirerLigne());

            Transform closest = PRIVTrouverPlusProcheHook(GameObject.FindGameObjectsWithTag("Hook"));
            if (closest != null)
            {
                _targetPoint = closest.position;
                BON_Interactive interactive = closest.GetComponent<BON_Interactive>();
                if (interactive != null)
                    interactive.Activate();
            }
            _player.AvatarState.HasCableOut = false;
            if (_moveScript != null) _moveScript.enabled = true;
        }
    }

    private Transform PRIVTrouverPlusProcheHook(GameObject[] hooks)
    {
        Transform closest = null;
        float shortest = Mathf.Infinity;

        Vector3 direction = _directionReference != null ? _directionReference.forward : transform.forward;

        foreach (GameObject hook in hooks)
        {
            Vector3 toHook = hook.transform.position - _gunOrigin.position;

            if (Vector3.Dot(direction, toHook.normalized) > 0)
            {
                float dist = toHook.magnitude;
                if (dist <= _rayDistance && dist < shortest)
                {
                    shortest = dist;
                    closest = hook.transform;
                }
            }
        }
        return closest;
    }

    private IEnumerator PRIVAnimerLigneAvecVague()
    {
        _animating = true;
        float timer = 0f;

        while (timer < _deployTime)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / _deployTime);
            Vector3 start = _gunOrigin.position;
            Vector3 end = _targetPoint;
            Vector3 direction = (end - start).normalized;
            Vector3 normal = Vector3.Cross(direction, Vector3.forward);

            for (int i = 0; i < _lineSegments; i++)
            {
                float delta = (float)i / (_lineSegments - 1);
                Vector3 point = Vector3.Lerp(start, end, delta * t);
                point += normal * Mathf.Sin(delta * _waveFrequency * Mathf.PI) * _waveAmplitude * (1f - t);
                _lineRenderer.SetPosition(i, point);
            }

            yield return null;
        }

        PRIVActiverRappel(_targetPoint);
        _swingTimer = 0f;
        _animating = false;
    }

    private IEnumerator PRIVRetirerLigne()
    {
        Vector3 start = _gunOrigin.position;
        Vector3 end = _joint != null ? _joint.connectedAnchor : _targetPoint;
        PRIVSupprimerRappel();

        float timer = 0f;
        while (timer < _deployTime)
        {
            timer += Time.deltaTime;
            float t = 1f - Mathf.Clamp01(timer / _deployTime);
            Vector3 currentStart = _gunOrigin.position;
            Vector3 direction = (end - currentStart).normalized;
            Vector3 normal = Vector3.Cross(direction, Vector3.forward);

            for (int i = 0; i < _lineSegments; i++)
            {
                float delta = (float)i / (_lineSegments - 1);
                Vector3 point = Vector3.Lerp(currentStart, end, delta * t);
                point += normal * Mathf.Sin(delta * _waveFrequency * Mathf.PI) * _waveAmplitude * t;
                _lineRenderer.SetPosition(i, point);
            }

            yield return null;
        }

        _lineRenderer.enabled = false;
        _lineVisible = false;
    }

    private void PRIVActiverRappel(Vector3 cible)
    {
        if (_joint != null) Destroy(_joint);

        _joint = gameObject.AddComponent<SpringJoint>();
        _joint.autoConfigureConnectedAnchor = false;
        _joint.connectedAnchor = cible;

        float distance = Vector3.Distance(transform.position, cible);
        _joint.maxDistance = distance * 0.75f;
        _joint.minDistance = 0.2f;
        _joint.spring = _springForce;
        _joint.damper = _damping;
        _joint.massScale = 1f;
    }

    private void PRIVSupprimerRappel()
    {
        if (_joint != null)
        {
            Destroy(_joint);
            _joint = null;
        }
    }

    private void PRIVAfficherLigne(Vector3 start, Vector3 end)
    {
        Vector3 direction = (end - start).normalized;
        Vector3 normal = Vector3.Cross(direction, Vector3.forward);

        for (int i = 0; i < _lineSegments; i++)
        {
            float delta = (float)i / (_lineSegments - 1);
            Vector3 point = Vector3.Lerp(start, end, delta);
            point += normal * Mathf.Sin(_swingTimer * _swingFrequency + delta * Mathf.PI) * _swingAmplitude;
            _lineRenderer.SetPosition(i, point);
        }
    }
}
