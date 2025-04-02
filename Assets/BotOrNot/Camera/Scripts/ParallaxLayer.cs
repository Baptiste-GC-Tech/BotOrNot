using UnityEngine;

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    /*
     *  FIELDS
     */
    public float parallaxFactor;

    /*
     *  CLASS METHODS
     */
    public void Move(float delta)
    {
        Vector3 newPos = transform.localPosition;
        newPos.x -= delta * parallaxFactor;

        transform.localPosition = newPos;
    }

}