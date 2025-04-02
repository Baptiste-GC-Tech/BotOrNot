using UnityEngine;

public class SimplePlayerMovement : MonoBehaviour
{
    /*
    *  FIELDS
    */

    [Header("Paramètres de déplacement")]
    public float moveSpeed = 5f;

    public enum RotationAxis { X, Y, Z }
    public RotationAxis rotationAxis = RotationAxis.Y;
    public float rotationSpeed = 10f;
    public float rotationOffset = -90f;

    private float _targetAngle = 0f;

    void Update()
    {
        float direction = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            direction = 1f;
            /* GAUCHE */
            _targetAngle = GetAxisRotationAngle(180f + rotationOffset);
        }

        if (Input.GetKey(KeyCode.D))
        {
            direction = -1f;
            /* DROITE */
            _targetAngle = GetAxisRotationAngle(0f + rotationOffset);
        }

        /* Mouvement */
        transform.position += new Vector3(direction, 0f, 0f) * moveSpeed * Time.deltaTime;

        /* Rotation Smooth */
        Quaternion currentRotation = transform.rotation;
        Quaternion desiredRotation = GetTargetRotation(_targetAngle);
        transform.rotation = Quaternion.Lerp(currentRotation, desiredRotation, Time.deltaTime * rotationSpeed);
    }

    private float GetAxisRotationAngle(float _angle)
    {
        return _angle;
    }

    private Quaternion GetTargetRotation(float _angle)
    {
        switch (rotationAxis)
        {
            case RotationAxis.X: return Quaternion.Euler(_angle, 0f, 0f);
            case RotationAxis.Y: return Quaternion.Euler(0f, _angle, 0f);
            case RotationAxis.Z: return Quaternion.Euler(0f, 0f, _angle);
            default: return Quaternion.identity;
        }
    }
}
