using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenMaterialSwapper : MonoBehaviour
{
    /*
     *  FIELDS
     */
    [SerializeField] BoxCollider _proximityTrigger;
    private bool _isInUse;
    public bool IsInUse
    {
        get { return _isInUse; }
        set {
            _isInUse = value;
            if (value) UseOnEffect();
            else UseOffEffect();
        }
    }


    /*
     *  CLASS METHODS
     */
    private void UseOnEffect()
    {
        Debug.LogWarning("UseOnEffect is TODO");
    }
    private void UseOffEffect()
    {
        Debug.LogWarning("UseOffEffect is TODO");
    }


    /*
     *  UNITY METHODS
     */
    void Start()
    {
        
    }
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Proximity Trigger was hit !");
    }
}
