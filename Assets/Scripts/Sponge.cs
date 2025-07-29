using System.Collections;
using UnityEngine;

public class SpongeTool : MakeupTool
{
    public GameObject[] allMakeupSpritesToClear;
    public GameObject acne;

    public override void ApplyEffect()
    {
        // Ничего не делаем тут, всё произойдёт сразу после SetToolInHand
    }

    public override void OnToolSelected()
    {
        if (isInUse)
        {
            handController.ResetHandAndTool(this);
            return;
        }

        isInUse = true;
        originalParent = transform.parent;
        originalLocalPosition = transform.localPosition;

        handController.SetToolInHand(this, () =>
        {
            // После прибытия в middle position — сразу чистим
            ClearMakeup();
            StartCoroutine(CleanupAndReturn());
        });
    }

    private void ClearMakeup()
    {
        foreach (GameObject sprite in allMakeupSpritesToClear)
        {
            if (sprite != null) sprite.SetActive(false);
        }
        acne.SetActive(true);
    }

    private IEnumerator CleanupAndReturn()
    {
        // Возвращаем спонж
        yield return handController.ReturnToolWithHand(this);

        // Возвращаем руку
        yield return handController.MoveToDefaultPosition();
    }
}