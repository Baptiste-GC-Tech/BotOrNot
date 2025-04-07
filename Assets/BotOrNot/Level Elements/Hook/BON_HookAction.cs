using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class BON_HookAction : MonoBehaviour
{
    //FIELD
    private bool _isHooked;
    private BON_Interactive _hook;



    //CLASS METHODS
    public void OnClick()
    {
        if (_isHooked)
        {
            _hook.Activate();
            _isHooked = false;
        }
        else
        {
            foreach (GameObject hook in GameObject.FindGameObjectsWithTag("Hook"))
            {
                if ((gameObject.transform.forward.x <= 0 && hook.transform.position.x - gameObject.transform.position.x <= 0) || (gameObject.transform.forward.x >= 0 && hook.transform.position.x - gameObject.transform.position.x >= 0))
                {
                    if ((hook.transform.position - gameObject.transform.position).magnitude <= 10)
                    {
                        _hook = hook.GetComponent<BON_Interactive>();
                        _hook.Activate();
                        _isHooked = true;
                        break;
                    }
                }
            }
        }
    }


    //UNITY METHODS
    private void Start()
    {
       _isHooked = false;
    }
}
