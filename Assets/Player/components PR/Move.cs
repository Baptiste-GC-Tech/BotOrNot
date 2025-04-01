using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{
    /*
     *  FIELDS
     */
    InputAction MoveAction;

    [SerializeField] private float _maxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        MoveAction = InputSystem.actions.FindAction("Player/Move");
    }

    // Update is called once per frame
    void Update()
    {
        // Left-right movement action handling
        Vector2 moveValue = MoveAction.ReadValue<Vector2>();
        GetComponent<Rigidbody>().AddForce(new Vector3(moveValue.x, moveValue.y, 0.0f), ForceMode.Acceleration);
        Mathf.Clamp(GetComponent<Rigidbody>().velocity.x, 0.0f, _maxSpeed);
        Mathf.Clamp(GetComponent<Rigidbody>().velocity.y, 0.0f, _maxSpeed);
        //transform.position += new Vector3(moveValue.x * Time.deltaTime, 0, 0);
    }
}
