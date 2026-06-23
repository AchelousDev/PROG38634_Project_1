using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CutsceneCameraManager : MonoBehaviour
{
    private enum CutsceneState
    {
        WorldPan,
        CharacterZoom,
        TargetZoom,
        Gameplay
    }

    [Header("Cinemachine Cameras")]
    public CinemachineCamera followCamera;
    public CinemachineCamera worldPanCamera;
    public CinemachineCamera characterZoomCamera;
    public CinemachineCamera targetZoomCamera;

    [Header("Priority Settings")]
    public int activePriority = 30;
    public int inactivePriority = 0;

    [Header("Cutscene Timing")]
    public float worldPanDuration = 4f;
    public float characterZoomDuration = 4f;
    public float targetZoomDuration = 4f;

    [Header("World Pan Settings")]
    public float worldPanRotationSpeed = 2f;

    [Header("Zoom Settings")]
    public float characterWideFov = 60f;
    public float characterCloseFov = 35f;
    public float targetWideFov = 65f;
    public float targetCloseFov = 30f;

    private CutsceneState currentState;

    private void Start()
    {
        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        currentState = CutsceneState.WorldPan;
        SetActiveCamera(worldPanCamera);
        yield return StartCoroutine(PlayWorldPan());

        currentState = CutsceneState.CharacterZoom;
        SetActiveCamera(characterZoomCamera);
        yield return StartCoroutine(PlayCharacterZoom());

        currentState = CutsceneState.TargetZoom;
        SetActiveCamera(targetZoomCamera);
        yield return StartCoroutine(PlayTargetZoom());

        currentState = CutsceneState.Gameplay;
        SetActiveCamera(followCamera);
    }

    private IEnumerator PlayWorldPan()
    {
        float timer = 0f;

        if (worldPanCamera == null)
        {
            yield break;
        }

        Vector3 startRotation = worldPanCamera.transform.eulerAngles;

        while (timer < worldPanDuration)
        {
            timer += Time.deltaTime;
            float panAmount = -Mathf.Abs(worldPanRotationSpeed) * timer;

            worldPanCamera.transform.rotation = Quaternion.Euler(
                startRotation.x,
                startRotation.y + panAmount,
                startRotation.z
            );

            yield return null;
        }
    }

    private IEnumerator PlayCharacterZoom()
    {
        float timer = 0f;

        while (timer < characterZoomDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / characterZoomDuration;

            float zoomProgress = progress <= 0.5f
                ? progress / 0.5f
                : 1f - ((progress - 0.5f) / 0.5f);

            if (characterZoomCamera != null)
            {
                characterZoomCamera.Lens.FieldOfView = Mathf.Lerp(characterWideFov, characterCloseFov, zoomProgress);
            }

            yield return null;
        }
    }

    private IEnumerator PlayTargetZoom()
    {
        float timer = 0f;

        while (timer < targetZoomDuration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / targetZoomDuration);

            if (targetZoomCamera != null)
            {
                targetZoomCamera.Lens.FieldOfView = Mathf.Lerp(targetWideFov, targetCloseFov, progress);
            }

            yield return null;
        }
    }

    private void SetActiveCamera(CinemachineCamera activeCamera)
    {
        SetCameraPriority(followCamera, inactivePriority);
        SetCameraPriority(worldPanCamera, inactivePriority);
        SetCameraPriority(characterZoomCamera, inactivePriority);
        SetCameraPriority(targetZoomCamera, inactivePriority);

        SetCameraPriority(activeCamera, activePriority);
    }

    private void SetCameraPriority(CinemachineCamera cameraToSet, int priority)
    {
        if (cameraToSet != null)
        {
            cameraToSet.Priority = priority;
        }
    }
}
