using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BrushTool : MakeupTool
{
    public Color selectedColor;
    public Image brushTip;

    public override void ApplyEffect()
    {
        StartCoroutine(ApplyShadowEffect());
    }

    private IEnumerator ApplyShadowEffect()
    {
        Vector3 startPos = handController.transform.position;
        float timer = 0;
        float applyTime = 1.2f;

        while (timer < applyTime)
        {
            float offset = Mathf.Sin(timer * 10f) * 10f;
            handController.transform.position = startPos + new Vector3(offset, 0, 0);
            timer += Time.deltaTime;
            yield return null;
        }

        /*foreach (var sprite in affectedSprites)
        {
            sprite.SetActive(true);
            if (sprite.TryGetComponent(out Image img))
                img.color = selectedColor;
        }*/

        StartCoroutine(ReturnToolSmooth());
    }

    private IEnumerator ReturnToolSmooth()
    {
        Vector3 start = transform.position;
        Vector3 end = originalParent.position;
        float t = 0;

        while (t < 1f)
        {
            transform.position = Vector3.Lerp(start, end, t);
            t += Time.deltaTime * 2f;
            yield return null;
        }

        transform.SetParent(originalParent);
        transform.localPosition = originalLocalPosition;
    }

    public override void ReturnTool()
    {
        StopAllCoroutines();
        StartCoroutine(ReturnToolSmooth());
    }
}