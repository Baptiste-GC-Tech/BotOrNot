using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fallingblock : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject _blockThatIsFalling;
    void Start()
    {
        _blockThatIsFalling.SetActive(false);
        StartCoroutine(BlockAppearCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator BlockAppearCoroutine()
    {
        yield return new WaitForSeconds(3);
        _blockThatIsFalling.SetActive(true);
    }
}
