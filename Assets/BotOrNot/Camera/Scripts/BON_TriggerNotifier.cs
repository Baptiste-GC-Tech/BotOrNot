using UnityEngine;

public class BON_TriggerNotifier : MonoBehaviour
{
    /*
     *  FIELDS
     */

    [Tooltip("L'objet avec lequel la cam�ra fera un barycentre ou qu'elle suivra directement.")]
    [SerializeField] public Transform LinkedObject;

    [Tooltip("R�f�rence vers le script CameraFollowMode � notifier.")]
    [SerializeField] private BON_CameraFollowMode _cameraFollow;

    [Tooltip("Le tag que le joueur doit avoir pour activer cette zone (par d�faut : 'Player').")]
    [SerializeField] private string _playerTag = "Player";

    [Header("Options de suivi")]

    [Tooltip("Dur�e pendant laquelle la cam�ra reste focalis�e. 0 = jusqu�� sortie du trigger.")]
    [SerializeField] private float _duration = 0f;

    [Tooltip("Active un offset personnalis� pour la cam�ra lorsque ce trigger est actif.")]
    [SerializeField] private bool _overrideOffset = false;

    [Tooltip("D�calage horizontal (X) de la cam�ra.")]
    [SerializeField] private float _offsetX = 0f;

    [Tooltip("D�calage vertical (Y) de la cam�ra.")]
    [SerializeField] private float _offsetY = 0f;

    [Tooltip("D�calage en profondeur (Z) de la cam�ra.")]
    [SerializeField] private float _offsetZ = 9f;

    [Tooltip("Si activ�, la cam�ra se concentre uniquement sur l'objet au lieu du barycentre.")]
    [SerializeField] private bool _focusOnly = false;

    private bool _isActive = false;

    /*
     *  UNITY METHODS
     */

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_playerTag) && !_isActive)
        {
            _isActive = true;

            _cameraFollow?.RegisterTriggerTarget(LinkedObject, _overrideOffset, _offsetX, _offsetY, _offsetZ, _focusOnly);

            if (_duration > 0f)
                Invoke(nameof(PRIVTerminerEffet), _duration);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_playerTag))
        {
            CancelInvoke();
            _isActive = false;

            _cameraFollow?.UnregisterTriggerTarget(LinkedObject);
        }
    }

    /*
     *  CLASS METHODS
     */

    private void PRIVTerminerEffet()
    {
        if (_isActive)
        {
            _isActive = false;
            _cameraFollow?.UnregisterTriggerTarget(LinkedObject);
        }
    }
}
