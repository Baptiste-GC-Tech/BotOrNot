using UnityEngine;

public class SimplePlayerMovement : MonoBehaviour
{
    [Header("Paramètres de déplacement")]
    public float moveSpeed = 5f;

    public enum RotationAxis { X, Y, Z }
    public RotationAxis rotationAxis = RotationAxis.Y;
    public float rotationSpeed = 10f;
    public float rotationOffset = -90f;

    private float targetAngle = 0f;

    void Update()
    {
        float direction = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            direction = 1f;
            targetAngle = GetAxisRotationAngle(180f + rotationOffset); // gauche
        }

        if (Input.GetKey(KeyCode.D))
        {
            direction = -1f;
            targetAngle = GetAxisRotationAngle(0f + rotationOffset); // droite
        }

        // Mouvement
        transform.position += new Vector3(direction, 0f, 0f) * moveSpeed * Time.deltaTime;

        // Rotation Smooth
        Quaternion currentRotation = transform.rotation;
        Quaternion desiredRotation = GetTargetRotation(targetAngle);
        transform.rotation = Quaternion.Lerp(currentRotation, desiredRotation, Time.deltaTime * rotationSpeed);
    }

    private float GetAxisRotationAngle(float angle)
    {
        return angle;
    }

    private Quaternion GetTargetRotation(float angle)
    {
        switch (rotationAxis)
        {
            case RotationAxis.X: return Quaternion.Euler(angle, 0f, 0f);
            case RotationAxis.Y: return Quaternion.Euler(0f, angle, 0f);
            case RotationAxis.Z: return Quaternion.Euler(0f, 0f, angle);
            default: return Quaternion.identity;
        }
    }
}
