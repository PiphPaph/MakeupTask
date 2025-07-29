using System.Collections;
using UnityEngine;

public class BrushTool : MakeupTool
{
    private GameObject selectedEffectSprite;
    private Transform selectedColorTransform;

    private bool waitingForColor = false;
    private bool draggingEnabled = false;
    
    private static GameObject currentEffectSprite;

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

        handController.SetToolInHand(this, () =>
        {
            waitingForColor = true;
        });
    }

    private IEnumerator SwapAndSelect(MakeupTool oldTool)
    {
        yield return handController.ReturnToolWithHand(oldTool);
        OnToolSelected();
    }

    public void OnColorSelected(Transform colorTransform, GameObject effectSprite)
    {
        if (!waitingForColor) return;
        if (currentEffectSprite != null)
            currentEffectSprite.SetActive(false);

        selectedColorTransform = colorTransform;
        selectedEffectSprite = effectSprite;

        StopAllCoroutines();
        StartCoroutine(AnimateBrushAtColor());
    }
   
    private IEnumerator AnimateBrushAtColor()
    {
        yield return handController.MoveToPosition(selectedColorTransform.position);
        
        Vector3 start = handController.transform.position;
        float time = 0;
        float duration = 0.5f;

        while (time < duration)
        {
            float offset = Mathf.Sin(time * 10f) * 5f;
            handController.transform.position = start + new Vector3(offset, 0, 0);
            time += Time.deltaTime;
            yield return null;
        }

        yield return handController.MoveToPosition(handController.middlePosition.position);

        if (!draggingEnabled)
        {
            handController.EnableDragging(this);
            draggingEnabled = true;
        }
    }

    public override void ApplyEffect()
    {
        if (selectedEffectSprite == null) return;

        StartCoroutine(ApplyBrush());
    }

    private IEnumerator ApplyBrush()
    {
        Vector3 start = handController.transform.position;
        float timer = 0f;
        float duration = 0.7f;

        while (timer < duration)
        {
            float offset = Mathf.Sin(timer * 10f) * 10f;
            handController.transform.position = start + new Vector3(offset, 0, 0);
            timer += Time.deltaTime;
            yield return null;
        }

        selectedEffectSprite.SetActive(true);
        currentEffectSprite = selectedEffectSprite;

        yield return handController.ReturnToolWithHand(this);
        yield return handController.MoveToDefaultPosition();

        handController.currentTool = null;
        selectedEffectSprite = null;
        selectedColorTransform = null;
        waitingForColor = false;
        draggingEnabled = false;
        isInUse = false;
    }
}
