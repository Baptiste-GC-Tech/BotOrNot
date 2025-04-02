using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BON_Cable : MonoBehaviour
{
    /*
     *  FIELDS
     */
    InputAction CableAction;


    // Start is called before the first frame update
    void Start()
    {
        CableAction = InputSystem.actions.FindAction("Cable");
    }

    // Update is called once per frame
    void Update()
    {
        
        if (CableAction.WasPressedThisFrame()) //cable => ""jump""
        {
            
        }
    }
}
