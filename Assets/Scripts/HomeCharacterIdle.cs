using UnityEngine;

public class HomeCharacterIdle : MonoBehaviour
{
    private Animator animator;
    private Renderer[] characterMaterials;

    private enum EyePosition
    {
        normal,
        happy,
        angry,
        dead
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        characterMaterials = GetComponentsInChildren<Renderer>();

        SetHappyIdle();
    }

    private void SetHappyIdle()
    {
        ChangeEyeOffset(EyePosition.happy);

        if (animator != null)
        {
            animator.SetTrigger("happy");
        }
    }

    private void ChangeEyeOffset(EyePosition pos)
    {
        Vector2 offset = Vector2.zero;

        switch (pos)
        {
            case EyePosition.normal:
                offset = new Vector2(0, 0);
                break;
            case EyePosition.happy:
                offset = new Vector2(.33f, 0);
                break;
            case EyePosition.angry:
                offset = new Vector2(.66f, 0);
                break;
            case EyePosition.dead:
                offset = new Vector2(.33f, .66f);
                break;
        }

        for (int i = 0; i < characterMaterials.Length; i++)
        {
            if (characterMaterials[i].transform.CompareTag("PlayerEyes"))
            {
                characterMaterials[i].material.SetTextureOffset("_MainTex", offset);
            }
        }
    }
}