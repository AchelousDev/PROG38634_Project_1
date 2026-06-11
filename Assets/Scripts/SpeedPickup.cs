using System.Collections;
using UnityEngine;

public class SpeedPickup : MonoBehaviour
{
    [Header("Speed Boost Settings")]
    public float boostedSpeed = 9f;
    public float boostDuration = 5f;

    [Header("Visual")]
    public GameObject visualObject;

    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (collected)
        {
            return;
        }

        if (!other.CompareTag("Player"))
        {
            return;
        }

        MovementInput movement = other.GetComponent<MovementInput>();

        if (movement == null)
        {
            Debug.LogWarning("MovementInput component not found on Player.");
            return;
        }

        collected = true;
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySpeedBoostSound();
        }

        if (visualObject != null)
        {
            visualObject.SetActive(false);
        }

        StartCoroutine(ApplySpeedBoost(movement));
    }

    private IEnumerator ApplySpeedBoost(MovementInput movement)
    {
        float originalSpeed = movement.Velocity;
        movement.Velocity = boostedSpeed;

        yield return new WaitForSeconds(boostDuration);

        movement.Velocity = originalSpeed;

        gameObject.SetActive(false);
    }
}