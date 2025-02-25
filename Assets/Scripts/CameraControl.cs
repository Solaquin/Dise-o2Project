using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField] private Transform defaultTarget; // El cubo original
    private Transform target;
    private float distance;
    [SerializeField] private float maxDistance = 5.0f;
    [SerializeField] private float minDistance = 2.0f;
    [SerializeField] private float xSpeed = 1.0f;
    [SerializeField] private float ySpeed = 1.0f;

    private float x = 0.0f;
    private float y = 0.0f;

    void Start()
    {
        target = defaultTarget; // Inicialmente, el objetivo es el cubo original
        distance = maxDistance;
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    void Update()
    {
        if (target)
        {
            if (Input.GetMouseButton(1))
            {
                x += Input.GetAxis("Mouse X") * xSpeed * distance * Time.deltaTime;
                y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;
            }

            distance -= Input.GetAxis("Mouse ScrollWheel") * 5;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);

            Quaternion rotation = Quaternion.Euler(y, x, 0);

            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;

            transform.rotation = rotation;
            transform.position = position;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            target = defaultTarget; // Cambiar el objetivo al cubo original
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
