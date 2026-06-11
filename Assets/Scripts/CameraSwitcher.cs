using UnityEngine;
using Unity.Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Cinemachine Cameras")]
    public CinemachineCamera[] cameras;

    [Header("Priority Settings")]
    public int activePriority = 20;
    public int inactivePriority = 0;

    private int followCameraIndex = 0;

    private void Start()
    {
        SwitchCamera(followCameraIndex);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchCamera(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchCamera(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchCamera(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchCamera(3);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SwitchCamera(followCameraIndex);
        }
    }

    private void SwitchCamera(int index)
    {
        if (cameras == null || cameras.Length == 0)
        {
            return;
        }

        if (index < 0 || index >= cameras.Length)
        {
            return;
        }

        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].Priority = inactivePriority;
        }

        cameras[index].Priority = activePriority;
    }
}