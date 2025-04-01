using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cable : MonoBehaviour
{
    /*
     *  FIELDS
     */
    InputAction CableAction;


    // Start is called before the first frame update
    void Start()
    {
        CableAction = InputSystem.actions.FindAction("Player/Cable");
    }

    // Update is called once per frame
    void Update()
    {
        // ## Currently serves as debug
        if (CableAction.WasPressedThisFrame()) //cable => ""jump""
        {
            //rien
        }
    }
}
