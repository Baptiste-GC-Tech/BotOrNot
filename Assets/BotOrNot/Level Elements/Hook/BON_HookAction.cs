using System.Collections;
using System.Collections.Generic;
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
            if (_hook != null)
            {
                _hook.Activate();
            }
            else
            {
                Debug.LogWarning("Tentative de désactivation du hook, mais _hook est null !");
            }

            _isHooked = false;
            print("_isHooked :" + _isHooked);
        }
        else
        {
            foreach (GameObject hook in GameObject.FindGameObjectsWithTag("Hook"))
            {
                print(hook);

                bool bonneDirection = (gameObject.transform.forward.x <= 0 && hook.transform.position.x - gameObject.transform.position.x <= 0)
                                   || (gameObject.transform.forward.x >= 0 && hook.transform.position.x - gameObject.transform.position.x >= 0);

                float distance = (hook.transform.position - gameObject.transform.position).magnitude;

                if (bonneDirection && distance <= 10)
                {
                    _hook = hook.GetComponent<BON_Interactive>();

                    if (_hook != null)
                    {
                        _hook.Activate();
                        _isHooked = true;
                    }
                    else
                    {
                        Debug.LogWarning("Le GameObject '" + hook.name + "' n'a pas de BON_Interactive !");
                    }

                    break;
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
