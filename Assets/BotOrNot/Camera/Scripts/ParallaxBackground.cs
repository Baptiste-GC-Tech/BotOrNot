using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParallaxBackground : MonoBehaviour
{
   /*
    *  FIELDS
    */

    public ParallaxCamera ParallaxCamera;
    List<ParallaxLayer> _parallaxLayers = new List<ParallaxLayer>();

    void Start()
    {
        if (ParallaxCamera == null)
            ParallaxCamera = Camera.main.GetComponent<ParallaxCamera>();

        if (ParallaxCamera != null)
            ParallaxCamera.OnCameraTranslate += Move;

        SetLayers();
    }

    /*
     *  CLASS METHODS
     */

    void SetLayers()
    {
        _parallaxLayers.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            ParallaxLayer layer = transform.GetChild(i).GetComponent<ParallaxLayer>();

            if (layer != null)
            {
                layer.name = "Layer-" + i;
                _parallaxLayers.Add(layer);
            }
        }
    }

    void Move(float delta)
    {
        foreach (ParallaxLayer layer in _parallaxLayers)
        {
            layer.Move(delta);
        }
    }
}