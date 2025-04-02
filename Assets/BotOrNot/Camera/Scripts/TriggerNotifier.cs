using UnityEngine;

public class TriggerNotifier : MonoBehaviour
{
    /*
     *  FIELDS
     */

    /* ---- Tooltips ---- */
    [Tooltip("L'objet avec lequel la cam�ra fera un barycentre ou qu'elle suivra directement.")]
    public Transform linkedObject;

    [Tooltip("R�f�rence vers le script CameraFollowMode � notifier.")]
    public CameraFollowMode cameraFollow;

    [Tooltip("Le tag que le joueur doit avoir pour activer cette zone (par d�faut : 'Player').")]
    public string playerTag = "Player";

    [Header("Options de suivi")]

    [Tooltip("Dur�e pendant laquelle la cam�ra reste focalis�e. 0 = jusqu�� sortie du trigger.")]
    public float duration = 0f;

    [Tooltip("Active un offset personnalis� pour la cam�ra lorsque ce trigger est actif.")]
    public bool overrideOffset = false;

    [Tooltip("D�calage horizontal (X) de la cam�ra.")]
    public float offsetX = 0f;

    [Tooltip("D�calage vertical (Y) de la cam�ra.")]
    public float offsetY = 0f;

    [Tooltip("D�calage en profondeur (Z) de la cam�ra.")]
    public float offsetZ = 9f;

    [Tooltip("Si activ�, la cam�ra se concentre uniquement sur l'objet au lieu du barycentre.")]
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
