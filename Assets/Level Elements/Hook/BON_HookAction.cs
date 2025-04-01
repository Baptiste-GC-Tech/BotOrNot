using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class BON_HookAction : MonoBehaviour
{
    public void OnClick()
    {
        foreach (GameObject hook in GameObject.FindGameObjectsWithTag("Hook"))
        {
            if ((gameObject.transform.forward.x <= 0 && hook.transform.position.x - gameObject.transform.position.x <= 0) || (gameObject.transform.forward.x >= 0 && hook.transform.position.x - gameObject.transform.position.x >= 0))
            {
                if ((hook.transform.position - gameObject.transform.position).magnitude <= 10)
                {
                    hook.SetActive(true);
                    break;
                }
            }
        }
    }
}
