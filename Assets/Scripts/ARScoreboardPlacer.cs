using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARScoreboardPlacer : MonoBehaviour
{
    [Header("AR")]
    [SerializeField] private ARRaycastManager raycastManager;

    [Header("Scoreboard")]
    [SerializeField] private GameObject scoreboardPrefab;

    private static readonly List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private GameObject spawnedScoreboard;

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    private void Update()
    {
        if (spawnedScoreboard != null)
        {
            return;
        }

        if (UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count == 0)
        {
            return;
        }

        var touch = UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches[0];

        if (touch.phase != UnityEngine.InputSystem.TouchPhase.Began)
        {
            return;
        }

        if (raycastManager.Raycast(
                touch.screenPosition,
                hits,
                TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            spawnedScoreboard = Instantiate(
                scoreboardPrefab,
                hitPose.position,
                hitPose.rotation
            );

            UpdateScoreText();
        }
    }

    private void UpdateScoreText()
    {
        if (spawnedScoreboard == null)
        {
            return;
        }

        TMP_Text scoreText =
            spawnedScoreboard.GetComponentInChildren<TMP_Text>();

        if (scoreText != null)
        {
            int finalScore = StatsManager.GetLastScore();

            scoreText.text = $"FINAL SCORE\n{finalScore}";
        }
        else
        {
            Debug.LogWarning("Scoreboard prefab has no TMP_Text component.");
        }
    }
    
    private void FaceScoreboardToCamera()
    {
        if (spawnedScoreboard == null || Camera.main == null)
        {
            return;
        }

        Vector3 direction =
            Camera.main.transform.position - spawnedScoreboard.transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
        {
            return;
        }

        spawnedScoreboard.transform.rotation =
            Quaternion.LookRotation(-direction);
    }
}