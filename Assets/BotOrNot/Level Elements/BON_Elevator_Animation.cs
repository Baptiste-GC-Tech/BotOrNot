using UnityEngine;

public class BON_Elevator_Animation : MonoBehaviour
{
    [Header("Transforms de référence")]
    [SerializeField] private Transform _startTransform;
    [SerializeField] private Transform _endTransform;
    [SerializeField] private Transform _elevatorTransform;

    [Header("Animation")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _blendParameterName = "Blend";

    private float _startY;
    private float _endY;
    private float _range;

    private void Start()
    {
        PRIV_InitialiserValeurs();
    }

    private void Update()
    {
        PRIV_MettreAJourBlend();
    }

    private void PRIV_InitialiserValeurs()
    {
        if (_startTransform == null || _endTransform == null || _elevatorTransform == null || _animator == null)
        {
            Debug.LogError("BON_Elevator_Animation : Un ou plusieurs champs ne sont pas assignés.");
            enabled = false;
            return;
        }

        _startY = _startTransform.position.y;
        _endY = _endTransform.position.y;
        _range = Mathf.Abs(_endY - _startY);

        if (_range < 0.001f)
        {
            Debug.LogWarning("BON_Elevator_Animation : La différence Y entre start et end est trop faible.");
            _range = 0.001f;
        }
    }

    private void PRIV_MettreAJourBlend()
    {
        float currentY = _elevatorTransform.position.y;
        float blendValue = Mathf.InverseLerp(_startY, _endY, currentY);
        _animator.SetFloat(_blendParameterName, blendValue);
    }
}
