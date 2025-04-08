using UnityEngine;

[ExecuteInEditMode]
public class BON_ParallaxCamera : MonoBehaviour
{
    /*
     *  FIELDS
     */

    public delegate void ParallaxCameraDelegate(float deltaMovement);
    public ParallaxCameraDelegate OnCameraTranslate;

    private float _oldPositionX;

    /*
     *  UNITY METHODS
     */

    private void Start()
    {
        _oldPositionX = transform.position.x;
    }

    private void Update()
    {
        if (!Mathf.Approximately(transform.position.x, _oldPositionX))
        {
            float delta = _oldPositionX - transform.position.x;
            OnCameraTranslate?.Invoke(delta);
            _oldPositionX = transform.position.x;
        }
    }
}
