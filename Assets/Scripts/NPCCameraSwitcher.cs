using UnityEngine;
using Unity.Cinemachine;

public class NPCCameraSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineCamera[] cameras;

    private int currentCameraIndex = 0;

    private void Start()
    {
        ActivateCamera(currentCameraIndex);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentCameraIndex++;

            if (currentCameraIndex >= cameras.Length)
            {
                currentCameraIndex = 0;
            }

            ActivateCamera(currentCameraIndex);
        }
    }

    private void ActivateCamera(int index)
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].Priority = i == index ? 20 : 0;
        }
    }
}