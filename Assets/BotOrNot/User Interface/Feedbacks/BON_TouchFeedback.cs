using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_TouchFeedback : MonoBehaviour
{

    private IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {
        coroutine = Die();
        StartCoroutine(coroutine);
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
