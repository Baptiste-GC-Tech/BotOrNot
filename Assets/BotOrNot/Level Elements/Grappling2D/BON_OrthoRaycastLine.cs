using UnityEngine;
using UnityEngine.InputSystem;

public class BON_OrthoRaycastLine : MonoBehaviour
{
    /*
     *  FIELDS
     */

    [Header("References")]
    [SerializeField] private Camera _mainCam;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _gunOrigin;
    [SerializeField] private Transform _fallbackTarget;
    [SerializeField] private LayerMask _raycastLayers;

    [Header("Raycast Settings")]
    [SerializeField] private float _rayDistance = 100f;

    private bool _lineVisible = false;
    private InputAction _clickAction;
    private Vector3 _hitPoint;

    /*
     *  UNITY METHODS
     */

    private void Start()
    {
        _clickAction = InputSystem.actions.FindAction("ActionsMapPR/Cable");

        if (_clickAction == null)
        {
            Debug.LogError("L'action 'ActionsMapPR/Cable' est introuvable dans l'Input System.");
        }
    }

    private void Update()
    {
        if (_clickAction != null && _clickAction.triggered)
        {
            PRIVGérerClic();
        }

        if (_lineVisible)
        {
            PRIVMettreAJourOrigine();
        }
    }

    /*
     *  CLASS METHODS
     */

    private void PRIVGérerClic()
    {
        Ray rayon = _mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (!_lineVisible)
        {
            if (Physics.Raycast(rayon, out hit, _rayDistance, _raycastLayers))
            {
                _hitPoint = hit.point;
                _lineVisible = true;
                _lineRenderer.enabled = true;
                _lineRenderer.positionCount = 2;
                _lineRenderer.SetPosition(0, _gunOrigin.position);
                _lineRenderer.SetPosition(1, _hitPoint);
            }
        }
        else
        {
            if (Physics.Raycast(rayon, out hit, _rayDistance))
            {
                PRIVCacherLigne();
            }
        }
    }

    private void PRIVMettreAJourOrigine()
    {
        _lineRenderer.SetPosition(0, _gunOrigin.position);
        _lineRenderer.SetPosition(1, _hitPoint);
    }

    private void PRIVCacherLigne()
    {
        _lineVisible = false;
        _lineRenderer.enabled = false;
    }
}
