using UnityEngine;

public class MinimapBorderRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 4f;
    [SerializeField] private bool clockwise = true;

    private void Update()
    {
        float direction = clockwise ? -1f : 1f;

        transform.Rotate(
            0f,
            0f,
            direction * rotationSpeed * Time.unscaledDeltaTime
        );
    }
}