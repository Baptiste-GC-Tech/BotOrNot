using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_TouchFeedback : MonoBehaviour
{

    private IEnumerator _coroutine;

    IEnumerator Die()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        _coroutine = Die();
        StartCoroutine(_coroutine);
    }

}
