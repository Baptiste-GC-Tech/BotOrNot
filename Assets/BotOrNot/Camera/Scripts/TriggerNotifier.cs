using UnityEngine;

public class TriggerNotifier : MonoBehaviour
{
    /*
     *  FIELDS
     */

    /* ---- Tooltips ---- */
    [Tooltip("L'objet avec lequel la caméra fera un barycentre ou qu'elle suivra directement.")]
    public Transform LinkedObject;

    [Tooltip("Référence vers le script CameraFollowMode à notifier.")]
    public CameraFollowMode CameraFollow;

    [Tooltip("Le tag que le joueur doit avoir pour activer cette zone (par défaut : 'Player').")]
    public string PlayerTag = "Player";

    [Header("Options de suivi")]

    [Tooltip("Durée pendant laquelle la caméra reste focalisée. 0 = jusqu’à sortie du trigger.")]
    public float Duration = 0f;

    [Tooltip("Active un offset personnalisé pour la caméra lorsque ce trigger est actif.")]
    public bool OverrideOffset = false;

    [Tooltip("Décalage horizontal (X) de la caméra.")]
    public float OffsetX = 0f;

    [Tooltip("Décalage vertical (Y) de la caméra.")]
    public float OffsetY = 0f;

    [Tooltip("Décalage en profondeur (Z) de la caméra.")]
    public float OffsetZ = 9f;

    [Tooltip("Si activé, la caméra se concentre uniquement sur l'objet au lieu du barycentre.")]
    public bool FocusOnly = false;

    private bool _isActive = false;

    /*
     *  CLASS METHODS
     */

    /* ---- Trigger Zone Enter ---- */
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerTag) && !_isActive)
        {
            _isActive = true;

            CameraFollow?.RegisterTriggerTarget(LinkedObject, OverrideOffset, OffsetX, OffsetY, OffsetZ, FocusOnly);

            if (Duration > 0f)
                Invoke(nameof(EndEffect), Duration);
        }
    }

    /* ---- Trigger Zone Exit ---- */
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(PlayerTag))
        {
            CancelInvoke();
            _isActive = false;

            CameraFollow?.UnregisterTriggerTarget(LinkedObject);
        }
    }

    /* ---- Trigger Zone End Effect ---- */
    private void EndEffect()
    {
        if (_isActive)
        {
            _isActive = false;
            CameraFollow?.UnregisterTriggerTarget(LinkedObject);
        }
    }
}
