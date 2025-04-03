using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_ScaleBehavior : MonoBehaviour
{
    [SerializeField] Transform _position1;
    [SerializeField] Transform _position2;
    bool _isBoxCorretlyPlaced;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent("piece") != null)
        {
            if (collision.gameObject.transform.position.x < _position2.position.x && collision.gameObject.transform.position.x > _position1.position.x)
            {
                _isBoxCorretlyPlaced = true;
            }
            else
            {
                _isBoxCorretlyPlaced = false;
            }
        }
    }
    void Start()
    {
        _isBoxCorretlyPlaced = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(gameObject.transform.rotation.eulerAngles.z);
        if (_isBoxCorretlyPlaced && ((gameObject.transform.rotation.eulerAngles.z >= 359 && gameObject.transform.rotation.eulerAngles.z <= 360) || (gameObject.transform.rotation.eulerAngles.z >= 0 && gameObject.transform.rotation.eulerAngles.z <= 1)))
        {
            //gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            gameObject.transform.Rotate(0,0, -gameObject.transform.rotation.eulerAngles.z);

            _isBoxCorretlyPlaced = false;
        }
    }
}
