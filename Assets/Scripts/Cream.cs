using System.Collections;
using UnityEngine;

public class Cream : MakeupTool 
{
    public float applyTime = 1.5f;
    private bool isApplying;

    public override void ApplyEffect()
    {
        if (isApplying) return;
        StartCoroutine(ApplyAnimation());
    }

    private IEnumerator ApplyAnimation()
    {
        isApplying = true;
        
        // 1. Анимация движения руки (горизонтальные колебания)
        Vector3 startPos = handController.transform.position;
        float timer = 0;
        
        while (timer < applyTime)
        {
            float offset = Mathf.Sin(timer * 8f) * 15f; // 8 - скорость, 15 - амплитуда
            handController.transform.position = startPos + new Vector3(offset, 0, 0);
            timer += Time.deltaTime;
            yield return null;
        }

        // 2. Применяем эффект
        affectedSprites[0].SetActive(false);
        
        // 3. Возвращаем руку и инструмент
        handController.ReturnToDefault();
        isApplying = false;
    }

    public override void ReturnTool()
    {
        // Жесткий возврат на место без Lerp
        transform.SetParent(originalParent);
        transform.localPosition = originalLocalPosition;
    }
}