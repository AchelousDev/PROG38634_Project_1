using UnityEngine;

public class MinimapCameraFollow : MonoBehaviour
{
    public Transform target;
    public float height = 40f;

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        transform.position = new Vector3(
            target.position.x,
            target.position.y + height,
            target.position.z
        );
    }
}