using TMPro;
using UnityEngine;

public class LoadingInstructionTips : MonoBehaviour
{
    public TMP_Text instructionText;
    public float changeInterval = 3f;

    [TextArea]
    public string[] instructions =
    {
        "Tip: Use the left joystick to move.",
        "Tip: Swipe on the right side of the screen to rotate the camera.",
        "Tip: Follow the path and look for the glowing portal.",
        "Tip: Watch for particles and glowing effects near important objects.",
        "Tip: Reach the portal to complete the level."
    };

    private int currentIndex;
    private float timer;

    private void Start()
    {
        ShowInstruction(0);
    }

    private void Update()
    {
        if (instructions == null || instructions.Length == 0)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer >= changeInterval)
        {
            timer = 0f;
            currentIndex = (currentIndex + 1) % instructions.Length;
            ShowInstruction(currentIndex);
        }
    }

    private void ShowInstruction(int index)
    {
        if (instructionText == null || instructions == null || instructions.Length == 0)
        {
            return;
        }

        instructionText.text = instructions[index];
    }
}