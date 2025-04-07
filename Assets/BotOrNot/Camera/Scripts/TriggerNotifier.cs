using UnityEngine;

public class TriggerNotifier : MonoBehaviour
{
    /*
     *  FIELDS
     */

    /* ---- Tooltips ---- */
    [Tooltip("L'objet avec lequel la cam�ra fera un barycentre ou qu'elle suivra directement.")]
    public Transform LinkedObject;

    [Tooltip("R�f�rence vers le script CameraFollowMode � notifier.")]
    public CameraFollowMode CameraFollow;

    [Tooltip("Le tag que le joueur doit avoir pour activer cette zone (par d�faut : 'Player').")]
    public string PlayerTag = "Player";

    [Header("Options de suivi")]

    [Tooltip("Dur�e pendant laquelle la cam�ra reste focalis�e. 0 = jusqu�� sortie du trigger.")]
    public float Duration = 0f;

    [Tooltip("Active un offset personnalis� pour la cam�ra lorsque ce trigger est actif.")]
    public bool OverrideOffset = false;

    [Tooltip("D�calage horizontal (X) de la cam�ra.")]
    public float OffsetX = 0f;

    [Tooltip("D�calage vertical (Y) de la cam�ra.")]
    public float OffsetY = 0f;

    [Tooltip("D�calage en profondeur (Z) de la cam�ra.")]
    public float OffsetZ = 9f;

    [Tooltip("Si activ�, la cam�ra se concentre uniquement sur l'objet au lieu du barycentre.")]
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
