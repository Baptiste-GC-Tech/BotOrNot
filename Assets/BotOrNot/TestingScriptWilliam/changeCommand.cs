using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class changeCommand : MonoBehaviour
{
    [SerializeField] BON_Actionnable _freeCraneActionable;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("changing the controls");
            _freeCraneActionable.Toggle();
        }
    }
}
