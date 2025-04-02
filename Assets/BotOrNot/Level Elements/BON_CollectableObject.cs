using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BON_CollectableObject : MonoBehaviour
{
    [SerializeField] GameObject _giveTarget;
    [SerializeField] float _animationDuration;
    private bool _isCollected;
    private GameObject _player;
    private Animator _playerAnimator;

    private void Start()
    {
        //change to the player controller once created /////////////!!!\\\\\\\\\\\\\\\
        _player = GameObject.FindFirstObjectByType<BON_CCPlayer>().gameObject;
        _playerAnimator = _player.GetComponent<Animator>();
        _isCollected = false;
    }
    private void OnTriggerEnter(Collider other)
    {
    if (other.gameObject == _player && !_isCollected)
        {
            Take();
        }
    }
    public void Take()
    {
        //Change according to the trigger name
        _playerAnimator.SetTrigger("TakingObject");
        _isCollected = true;
        StartCoroutine(AnimationEndCoroutine());
    }

    IEnumerator AnimationEndCoroutine()
    {
        yield return new WaitForSeconds(_animationDuration);
        gameObject.SetActive(false);
    }

    public void Give()
    {
        gameObject.transform.position = _giveTarget.transform.position;
        gameObject.SetActive(true);
        //Change according to the trigger name
        _playerAnimator.SetTrigger("GivingObject");
    }

    private void Update()
    {
        float distanceWithTarget = Vector3.Distance(_player.transform.position, _giveTarget.transform.position);
        if (distanceWithTarget < 10 && _isCollected)
        {
            Give();
        }
    }
}
