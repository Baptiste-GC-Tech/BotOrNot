using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.VisualScripting;

[RequireComponent(typeof(Rigidbody))]
public class BON_CablePR : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera _mainCam; //non set & inutilisé 
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _gunOrigin;
    [SerializeField] private LayerMask _raycastLayers;
    [SerializeField] private Transform _directionReference;
    [SerializeField] private BON_CCPlayer _player;

    [Header("Hook Settings")]
    [SerializeField] private float _rayDistance = 15f;
    [SerializeField] private float _springForce = 1f;
    [SerializeField] private float _damping = 0.51f;
    [SerializeField] private float _cableLengthSpeed = 3.86f;
    [SerializeField] private float _swingForce = 0.32f;

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
    private Transform _hookActif;

    private InputAction _clickAction;

    private SpringJoint _joint;
    private ParticleSystem _fxhooked;
    private Rigidbody _rb;

    private Vector2 input;
    private float _swingInput;
    float _lengthChange = 0f;
    private Vector3 _currentTarget;
    private Transform _closest;

    /* animation related */
    Vector3 _start;
    Vector3 _end;
    Vector3 _direction;
    Vector3 _normal;
    Vector3 _point;

    private void Start()
    {
        _clickAction = InputSystem.actions.FindAction("ActionsMapPR/Cable");

        _rb = GetComponent<Rigidbody>();

        if (_clickAction == null)
            Debug.LogError("L'action 'ActionsMapPR/Cable' est introuvable.");

        _player.AvatarState.HasCableOut = false;
    }

    private void Update()
    {
        input = BON_GameManager.Instance().DirectionalInputValue;

        if (_clickAction != null && _clickAction.triggered)
        {
            GererClic();
        }

        if (_lineVisible && _joint != null && !_animating)
        {
            _currentTarget = _hookActif != null ? _hookActif.position : _joint.connectedAnchor;
            PRIVAfficherLigne(_gunOrigin.position, _currentTarget);

            _swingTimer += Time.deltaTime;
        }

        if (_joint != null)
        {

            if (_hookActif != null)
            {
                _joint.connectedAnchor = _hookActif.position;
            }

            _lengthChange = 0f;

            if (input.y > 0.5f)
            {
                _lengthChange -= _cableLengthSpeed * Time.deltaTime;
            }
            else if (input.y < -0.5f)
            {
                _lengthChange += _cableLengthSpeed * Time.deltaTime;
            }

            _joint.maxDistance = Mathf.Clamp(_joint.maxDistance + _lengthChange, 0.5f, _rayDistance);

            _swingInput = Mathf.Abs(input.x) > 0.5f ? Mathf.Sign(input.x) : 0f;

            Vector3 toAnchor = _joint.connectedAnchor - transform.position;
            Vector3 horizontalToAnchor = new Vector3(toAnchor.x, 0f, toAnchor.z).normalized;
            Vector3 swingDir = Vector3.Cross(Vector3.up, horizontalToAnchor).normalized;

            if (_swingInput != 0f)
            {
                Vector3 force = swingDir * _swingInput * _swingForce;
                _rb.AddForce(force, ForceMode.VelocityChange);
            }
            else
            {
                Vector3 lateralVel = Vector3.Project(_rb.velocity, swingDir);
                _rb.velocity -= lateralVel * _lateralDamping * Time.deltaTime;
            }

            float currentDistance = Vector3.Distance(transform.position, _joint.connectedAnchor);
            if (currentDistance > _joint.maxDistance + 0.1f)
            {
                Vector3 direction = (_joint.connectedAnchor - transform.position).normalized;
                _rb.AddForce(direction * (_springForce * 2f), ForceMode.Acceleration);
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
                _hookActif = closest;

                // Active FX
                // Transform fx = _hookActif.Find("FX - Hooked Particle System");
                // if (fx != null) fx.gameObject.SetActive(true);

                _player.AvatarState.HasCableOut = true;

                StartCoroutine(PRIVAnimerLigneAvecVague());
            }
        }
        else
        {
            _player.AvatarState.HasCableOut = false;

            StartCoroutine(PRIVRetirerLigne());

            if (_hookActif != null)
            {
                _targetPoint = _hookActif.position;
                BON_Interactive_Actionnables interactive = _hookActif.GetComponent<BON_Interactive_Actionnables>();
                if (interactive != null)
                {
                    if (interactive.ActionnablesList != null)
                    {
                        interactive.Activate();
                    }
                }
            }
            _hookActif = null;
            
        }
    }

    private Transform PRIVTrouverPlusProcheHook(GameObject[] hooks)
    {
        _closest = null;
        float shortest = Mathf.Infinity;

        Vector3 direction = _directionReference != null ? _directionReference.forward : transform.forward;
        Debug.DrawRay(_gunOrigin.position, direction * _rayDistance, Color.red, 2f);

        foreach (GameObject hook in hooks)
        {
            Vector3 toHook = hook.transform.position - _gunOrigin.position;
            Debug.DrawLine(_gunOrigin.position, hook.transform.position, Color.yellow, 2f);
            float dist = toHook.magnitude;

            // On check si la distance entre target et origine est bien la bonne sinon il y a obstacle
            if (Vector3.Dot(direction, toHook.normalized) > 0 && dist <= _rayDistance)
            {
                Ray ray = new Ray(_gunOrigin.position, toHook.normalized);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, dist, _raycastLayers))
                {
                    if (hit.transform != hook.transform)
                    {
                        Debug.Log($"Hook '{hook.name}' bloqué par : {hit.transform.name}");
                        //_fxhooked = hook.GetComponent<ParticleSystem>();
                        //_fxhooked.Play();
                        continue;
                    }
                }

                if (dist <= _rayDistance && dist < shortest)
                {
                    shortest = dist;
                    _closest = hook.transform;
                    Debug.DrawLine(_gunOrigin.position, _closest.position, Color.green, 2f);
                }
            }
        }
        return _closest;
    }

    private IEnumerator PRIVAnimerLigneAvecVague()
    {
        _animating = true;
        float timer = 0f;

        while (timer < _deployTime)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / _deployTime);
            _start = _gunOrigin.position;

            _end = _hookActif != null ? _hookActif.position : _targetPoint;
            _direction = (_end - _start).normalized;
            _normal = Vector3.Cross(_direction, Vector3.forward);

            for (int i = 0; i < _lineSegments; i++)
            {
                float delta = (float)i / (_lineSegments - 1);
                _point = Vector3.Lerp(_start, _end, delta * t);
                _point += _normal * Mathf.Sin(delta * _waveFrequency * Mathf.PI) * _waveAmplitude * (1f - t);
                _lineRenderer.SetPosition(i, _point);
            }

            yield return null;
        }

        _animating = false;

        if (!_lineVisible || _hookActif == null)
            yield break;

        // FX
        Transform fx = _hookActif.Find("FX - Hooked Particle System");
        if (fx != null) fx.gameObject.SetActive(true);

        Vector3 finalTarget = _hookActif.position;
        PRIVActiverRappel(finalTarget);
        _swingTimer = 0f;

        BON_Interactive_Actionnables interactive = _hookActif.GetComponent<BON_Interactive_Actionnables>();
        if (interactive != null && interactive.ActionnablesList != null)
        {
            interactive.Activate();
        }
    }


    private IEnumerator PRIVRetirerLigne()
    {
        PRIVSupprimerRappel();

        float timer = 0f;

        while (timer < _deployTime)
        {
            timer += Time.deltaTime;
            float t = 1f - Mathf.Clamp01(timer / _deployTime);

            Vector3 currentStart = _gunOrigin.position;
            Vector3 end = _hookActif != null ? _hookActif.position : _targetPoint;
            // Vector3 end = _hookActif != null ? _hookActif.position : _joint.connectedAnchor;

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
        _hookActif = null;
    }

    private void PRIVActiverRappel(Vector3 cible)
    {
        if (_joint != null) Destroy(_joint);

        _joint = gameObject.AddComponent<SpringJoint>();
        _joint.autoConfigureConnectedAnchor = false;
        _joint.connectedAnchor = cible;

        float distance = Vector3.Distance(transform.position, cible);
        _joint.minDistance = 0.2f;
        _joint.maxDistance = distance * 0.75f;
        _joint.spring = -_springForce;
        _joint.damper = _damping;
        _joint.massScale = 1f;
        Debug.Log($"Distance Cable : ({distance})");
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
