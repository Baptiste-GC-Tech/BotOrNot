using UnityEngine;

public class TriggerNotifier : MonoBehaviour
{
    [Tooltip("L'objet avec lequel la cam�ra fera un barycentre lorsqu'on entre dans cette zone.")]
    public Transform linkedObject;

    [Tooltip("R�f�rence vers le script CameraFollowMode � notifier.")]
    public CameraFollowMode cameraFollow;

    [Tooltip("Le tag que le joueur doit avoir pour activer cette zone (Ici 'Player').")]
    public string playerTag = "Player";

    [Header("Options de suivi")]

    [Tooltip("Dur�e pendant laquelle la cam�ra reste focalis�e sur le barycentre. 0 = jusqu�� sortie du trigger.")]
    public float duration = 0f;

    [Tooltip("Active un offset personnalis� pour la cam�ra lorsque ce trigger est actif.")]
    public bool overrideOffset = false;

    [Tooltip("D�calage horizontal (X) de la cam�ra lors du barycentre.")]
    public float offsetX = 0f;

    [Tooltip("D�calage en profondeur (Z) de la cam�ra lors du barycentre.")]
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
