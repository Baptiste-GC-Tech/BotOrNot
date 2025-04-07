using UnityEngine;

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    /*
     *  FIELDS
     */
    public float ParallaxFactor;

    /*
     *  CLASS METHODS
     */
    public void Move(float delta)
    {
        Vector3 newPos = transform.localPosition;
        newPos.x -= delta * ParallaxFactor;

        transform.localPosition = newPos;
    }

}