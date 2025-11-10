using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    private float fixedY; // store the starting Y position

    void Start()
    {
        fixedY = transform.position.y; // remember the initial camera height
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Follow player only on X, but keep camera’s starting Y
            Vector3 desiredPosition = new Vector3(target.position.x + offset.x, fixedY + offset.y, transform.position.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
