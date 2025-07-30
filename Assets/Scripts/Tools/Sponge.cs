using System.Collections;
using UnityEngine;

public class Sponge : MakeupTool
{
    public GameObject[] allMakeupSpritesToClear;
    public GameObject acne;

    public override void ApplyEffect()
    {

    }

    public override void OnToolSelected()
    {
        isInUse = true;
        originalParent = transform.parent;
        originalLocalPosition = transform.localPosition;

        handController.SetToolInHand(this, () =>
        {
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
        yield return handController.ReturnToolWithHand(this);
        
        yield return handController.MoveToDefaultPosition();
    }
}