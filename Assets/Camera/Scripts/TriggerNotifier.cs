using UnityEngine;

public class TriggerNotifier : MonoBehaviour
{
    /*
     *  FIELDS
     */

    /* ---- Tooltips ---- */
    [Tooltip("L'objet avec lequel la caméra fera un barycentre ou qu'elle suivra directement.")]
    public Transform linkedObject;

    [Tooltip("Référence vers le script CameraFollowMode à notifier.")]
    public CameraFollowMode cameraFollow;

    [Tooltip("Le tag que le joueur doit avoir pour activer cette zone (par défaut : 'Player').")]
    public string playerTag = "Player";

    [Header("Options de suivi")]

    [Tooltip("Durée pendant laquelle la caméra reste focalisée. 0 = jusqu’à sortie du trigger.")]
    public float duration = 0f;

    [Tooltip("Active un offset personnalisé pour la caméra lorsque ce trigger est actif.")]
    public bool overrideOffset = false;

    [Tooltip("Décalage horizontal (X) de la caméra.")]
    public float offsetX = 0f;

    [Tooltip("Décalage vertical (Y) de la caméra.")]
    public float offsetY = 0f;

    [Tooltip("Décalage en profondeur (Z) de la caméra.")]
    public float offsetZ = 9f;

    [Tooltip("Si activé, la caméra se concentre uniquement sur l'objet au lieu du barycentre.")]
    public bool focusOnly = false;

    private bool _isActive = false;

    /*
     *  CLASS METHODS
     */

    /* ---- Trigger Zone Enter ---- */
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && !_isActive)
        {
            _isActive = true;

            cameraFollow?.RegisterTriggerTarget(linkedObject, overrideOffset, offsetX, offsetY, offsetZ, focusOnly);

            if (duration > 0f)
                Invoke(nameof(EndEffect), duration);
        }
    }

    /* ---- Trigger Zone Exit ---- */
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            CancelInvoke();
            _isActive = false;

            cameraFollow?.UnregisterTriggerTarget(linkedObject);
        }
    }

    /* ---- Trigger Zone End Effect ---- */
    private void EndEffect()
    {
        if (_isActive)
        {
            _isActive = false;
            cameraFollow?.UnregisterTriggerTarget(linkedObject);
        }
    }
}
