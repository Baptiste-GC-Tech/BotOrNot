using UnityEngine;

public class TriggerNotifier : MonoBehaviour
{
    [Tooltip("L'objet avec lequel la caméra fera un barycentre lorsqu'on entre dans cette zone.")]
    public Transform linkedObject;

    [Tooltip("Référence vers le script CameraFollowMode à notifier.")]
    public CameraFollowMode cameraFollow;

    [Tooltip("Le tag que le joueur doit avoir pour activer cette zone (Ici 'Player').")]
    public string playerTag = "Player";

    [Header("Options de suivi")]

    [Tooltip("Durée pendant laquelle la caméra reste focalisée sur le barycentre. 0 = jusqu’à sortie du trigger.")]
    public float duration = 0f;

    [Tooltip("Active un offset personnalisé pour la caméra lorsque ce trigger est actif.")]
    public bool overrideOffset = false;

    [Tooltip("Décalage horizontal (X) de la caméra lors du barycentre.")]
    public float offsetX = 0f;

    [Tooltip("Décalage en profondeur (Z) de la caméra lors du barycentre.")]
    public float offsetZ = 9f;

    private bool isActive = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && !isActive)
        {
            isActive = true;

            cameraFollow?.RegisterTriggerTarget(linkedObject, overrideOffset, offsetX, offsetZ);

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
