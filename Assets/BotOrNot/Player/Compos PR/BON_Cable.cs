using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(Rigidbody))]
public class BON_Cable : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera _mainCam;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _gunOrigin;
    [SerializeField] private LayerMask _raycastLayers;

    [Header("Hook Settings")]
    [SerializeField] private float _rayDistance = 100f;
    [SerializeField] private float _springForce = 30f; 
    [SerializeField] private float _damping = 5f;
    [SerializeField] private float _cableLengthSpeed = 5f;

    [Header("Visual Settings")]
    [SerializeField] private int _lineSegments = 20;
    [SerializeField] private float _deployTime = 0.3f;
    [SerializeField] private float _waveAmplitude = 0.5f;
    [SerializeField] private float _waveFrequency = 2f;
    [SerializeField] private float _swingAmplitude = 0.1f;
    [SerializeField] private float _swingFrequency = 2f;

    // player reference
    [SerializeField] private BON_CCPlayer _player;

    private bool _lineVisible = false;
    private bool _animating = false;
    private InputAction _clickAction;
    private InputAction _CablemoveDown;
    private InputAction _CablemoveUp;
    private SpringJoint _joint;
    private Rigidbody _rb;
    private float _swingTimer;
    private Vector3 _targetPoint;

    private void Start()
    {
        _clickAction = InputSystem.actions.FindAction("ActionsMapPR/Cable");
        _CablemoveDown = InputSystem.actions.FindAction("ActionsMapPR/CablemoveDown");
        _CablemoveUp = InputSystem.actions.FindAction("ActionsMapPR/CablemoveUp");
        _rb = GetComponent<Rigidbody>();

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

            if (_CablemoveUp != null && _CablemoveUp.ReadValue<float>() > 0.5f)
            {
                lengthChange -= _cableLengthSpeed * Time.deltaTime;
            }

            if (_CablemoveDown != null && _CablemoveDown.ReadValue<float>() > 0.5f)
            {
                lengthChange += _cableLengthSpeed * Time.deltaTime;
            }

            if (Mathf.Abs(lengthChange) > 0.001f)
            {
                _joint.maxDistance = Mathf.Clamp(_joint.maxDistance + lengthChange, 1f, _rayDistance);
            }
        }

    }

    public void GererClic()
    {
        if (!_lineVisible)
        {
            // State Machine
            _player.AvatarState.IsthrowingCable = true;

            Transform closest = PRIVTrouverPlusProcheHook(GameObject.FindGameObjectsWithTag("Hook"));

            if (closest != null)
            {
                _lineVisible = true;
                _lineRenderer.enabled = true;
                _lineRenderer.positionCount = _lineSegments;
                _targetPoint = closest.position;
                StartCoroutine(PRIVAnimerLigneAvecVague());

                // Activation du hook via BON_Interactive
                BON_Interactive interactive = closest.GetComponent<BON_Interactive>();
                if (interactive != null)
                {
                    interactive.Activate();
                }
            }
        }
        else
        {
            // State Machine
            _player.AvatarState.IsthrowingCable = false;
            StartCoroutine(PRIVRetirerLigne());
        }
    }


    private Transform PRIVTrouverPlusProcheHook(GameObject[] hooks)
    {
        Transform closest = null;
        float shortest = Mathf.Infinity;

        Vector3 direction = transform.right;

        foreach (GameObject hook in hooks)
        {
            Vector3 toHook = hook.transform.position - _gunOrigin.position;

            if (Vector3.Dot(direction, toHook.normalized) > 0) // hook dans la direction du regard
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
        _joint.maxDistance = distance * 0.8f;
        _joint.minDistance = distance * 0.25f;
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