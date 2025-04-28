using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BON_ScaleBehavior : MonoBehaviour
{
    /*
     * FIELDS
     */
    [SerializeField] Transform _position1;
    [SerializeField] Transform _position2;
    [SerializeField] float _scaleAngularVelocity;
    bool _isBoxCorretlyPlaced;
    bool _isBouncing = false;
    bool _shouldWait = false;

    /*
     * UNITY METHODS
     */
    void Start()
    {
        _isBoxCorretlyPlaced = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<Rigidbody>().angularVelocity = Vector3.ClampMagnitude(GetComponent<Rigidbody>().angularVelocity, _scaleAngularVelocity);
        if (_isBoxCorretlyPlaced && ((gameObject.transform.rotation.eulerAngles.z >= 359 && gameObject.transform.rotation.eulerAngles.z <= 360) || (gameObject.transform.rotation.eulerAngles.z >= 0 && gameObject.transform.rotation.eulerAngles.z <= 1)))
        {
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            _isBoxCorretlyPlaced = false;
        }
       if (_isBouncing)
        {
            Debug.Log(transform.eulerAngles);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 0, 340), 0.1f);
            if (transform.eulerAngles.z - 340 < 5 && transform.eulerAngles.z - 340 > -5)
            {
                _isBouncing = false;
            }
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetComponent<BON_FreeMovementCrane>() != null)
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
        if (collision.gameObject.GetComponent<BON_Bounce>() != null && !_shouldWait)
        {
            _isBouncing = true;
            _shouldWait = true;
            StartCoroutine(WaitToBounce());
            
        }
    }
    IEnumerator WaitToBounce()
    {
        yield return new WaitForSeconds(3);
        _isBouncing = false;
        _shouldWait = false;
    }
}
