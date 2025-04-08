using UnityEngine;

public class BON_TriggerNotifier : MonoBehaviour
{
    /*
     *  FIELDS
     */

    [Tooltip("L'objet avec lequel la caméra fera un barycentre ou qu'elle suivra directement.")]
    [SerializeField] public Transform LinkedObject;

    [Tooltip("Référence vers le script CameraFollowMode à notifier.")]
    [SerializeField] private BON_CameraFollowMode _cameraFollow;

    [Tooltip("Le tag que le joueur doit avoir pour activer cette zone (par défaut : 'Player').")]
    [SerializeField] private string _playerTag = "Player";

    [Header("Options de suivi")]

    [Tooltip("Durée pendant laquelle la caméra reste focalisée. 0 = jusqu’à sortie du trigger.")]
    [SerializeField] private float _duration = 0f;

    [Tooltip("Active un offset personnalisé pour la caméra lorsque ce trigger est actif.")]
    [SerializeField] private bool _overrideOffset = false;

    [Tooltip("Décalage horizontal (X) de la caméra.")]
    [SerializeField] private float _offsetX = 0f;

    [Tooltip("Décalage vertical (Y) de la caméra.")]
    [SerializeField] private float _offsetY = 0f;

    [Tooltip("Décalage en profondeur (Z) de la caméra.")]
    [SerializeField] private float _offsetZ = 9f;

    [Tooltip("Si activé, la caméra se concentre uniquement sur l'objet au lieu du barycentre.")]
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
