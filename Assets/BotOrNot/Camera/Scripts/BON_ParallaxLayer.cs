using UnityEngine;

[ExecuteInEditMode]
public class BON_ParallaxLayer : MonoBehaviour
{
    /*
     *  FIELDS
     */

    [SerializeField] private float _parallaxFactor = 0.5f;

    /*
     *  CLASS METHODS
     */

    public void M�thodeD�placer(float delta)
    {
        Vector3 newPos = transform.localPosition;
        newPos.x -= delta * _parallaxFactor;
        transform.localPosition = newPos;
    }
}
