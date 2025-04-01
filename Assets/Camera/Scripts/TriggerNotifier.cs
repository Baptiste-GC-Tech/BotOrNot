using UnityEngine;

public class TriggerNotifier : MonoBehaviour
{
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

    [Tooltip("Décalage en profondeur (Z) de la caméra.")]
    public float offsetZ = 9f;

    [Tooltip("Si activé, la caméra se concentre uniquement sur l'objet au lieu du barycentre.")]
    public bool focusOnly = false;

    private bool isActive = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && !isActive)
        {
            isActive = true;

            cameraFollow?.RegisterTriggerTarget(linkedObject, overrideOffset, offsetX, offsetZ, focusOnly);

            if (duration > 0f)
                Invoke(nameof(EndEffect), duration);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            CancelInvoke();
            isActive = false;

            cameraFollow?.UnregisterTriggerTarget(linkedObject);
        }
    }

    private void EndEffect()
    {
        if (isActive)
        {
            isActive = false;
            cameraFollow?.UnregisterTriggerTarget(linkedObject);
        }
    }
}
