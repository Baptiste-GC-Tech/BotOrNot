using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class BOT_InteractibleGlow : MonoBehaviour
{

    [SerializeField] GameObject GlowObject;

    void Start()
    {
        GlowObject.SetActive(false);
    }

    public void ToggleGlow()
    {
        switch(GlowObject.activeSelf) 
        {
            case true:
                GlowObject.SetActive(false); break;
            case false:
                GlowObject.SetActive(true); break;
        }
    }
}
