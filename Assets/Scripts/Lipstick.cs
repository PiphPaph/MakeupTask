using System.Collections;
using UnityEngine;

public class Lipstick : MakeupTool
{
    public GameObject lipstickSprite;
    public static GameObject currentLipstickSprite;

    public float applyTime = 1.2f;

    public override void OnToolSelected()
    {
        if (isInUse)
        {
            handController.ResetHandAndTool(this);
            return;
        }
        
        if (handController.currentTool != null && handController.currentTool != this)
        {
            StartCoroutine(SwapAndSelect(handController.currentTool));
            return;
        }

        isInUse = true;
        originalParent = transform.parent;
        originalLocalPosition = transform.localPosition;

        handController.SetToolInHand(this);
    }

    private IEnumerator SwapAndSelect(MakeupTool oldTool)
    {
        yield return handController.ReturnToolWithHand(oldTool);
        OnToolSelected();
    }

    public override void ApplyEffect()
    {
        StartCoroutine(ApplyRoutine());
    }

    private IEnumerator ApplyRoutine()
    {
        Vector3 start = handController.transform.position;
        float timer = 0f;

        while (timer < applyTime)
        {
            float offset = Mathf.Sin(timer * 10f) * 10f;
            handController.transform.position = start + new Vector3(offset, 0, 0);
            timer += Time.deltaTime;
            yield return null;
        }

        if (currentLipstickSprite != null)
            currentLipstickSprite.SetActive(false);

        if (lipstickSprite != null)
        {
            lipstickSprite.SetActive(true);
            currentLipstickSprite = lipstickSprite;
        }

        yield return handController.ReturnToolWithHand(this);
        yield return handController.MoveToDefaultPosition();
        handController.currentTool = null;
    }
}