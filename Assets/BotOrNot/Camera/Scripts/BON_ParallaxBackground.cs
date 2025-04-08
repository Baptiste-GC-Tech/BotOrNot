using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BON_ParallaxBackground : MonoBehaviour
{
    /*
     *  FIELDS
     */

    [SerializeField] public BON_ParallaxCamera ParallaxCamera;
    private List<BON_ParallaxLayer> _parallaxLayers = new List<BON_ParallaxLayer>();

    /*
     *  UNITY METHODS
     */

    private void Start()
    {
        if (ParallaxCamera == null)
            ParallaxCamera = Camera.main.GetComponent<BON_ParallaxCamera>();

        if (ParallaxCamera != null)
            ParallaxCamera.OnCameraTranslate += PRIVDéplacer;

        PRIVInitialiserLayers();
    }

    /*
     *  CLASS METHODS
     */

    private void PRIVInitialiserLayers()
    {
        _parallaxLayers.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            BON_ParallaxLayer layer = transform.GetChild(i).GetComponent<BON_ParallaxLayer>();

            if (layer != null)
            {
                layer.name = "Layer-" + i;
                _parallaxLayers.Add(layer);
            }
        }
    }

    private void PRIVDéplacer(float delta)
    {
        foreach (BON_ParallaxLayer layer in _parallaxLayers)
        {
            layer.MéthodeDéplacer(delta);
        }
    }
}
