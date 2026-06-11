using UnityEngine;

public class EndPointAnimation : MonoBehaviour
{
    [Header("Rotation")]
    public float rotationSpeed = 60f;

    [Header("Floating")]
    public float floatHeight = 0.4f;
    public float floatSpeed = 2f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}