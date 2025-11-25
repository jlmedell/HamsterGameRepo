using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    private float fixedY;

    void Start()
    {
        fixedY = transform.position.y;
    }

    private Vector3 velocity = Vector3.zero;

    void FixedUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = new Vector3(
            target.position.x + offset.x,
            fixedY + offset.y,
            transform.position.z
        );

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref velocity,
            smoothSpeed
        );
    }
}
