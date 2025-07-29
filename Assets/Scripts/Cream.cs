using System.Collections;
using UnityEngine;

public class Cream : MakeupTool
{
    public GameObject acneSprite;
    public GameObject cleanFaceSprite;
    public float applyTime = 1.2f;

    public override void ApplyEffect()
    {
        StartCoroutine(ApplyRoutine());
    }

    private IEnumerator ApplyRoutine()
    {
        // Анимация "вибрации" по X
        Vector3 startPos = handController.transform.position;
        float timer = 0;

        while (timer < applyTime)
        {
            float offset = Mathf.Sin(timer * 10f) * 10f;
            handController.transform.position = startPos + new Vector3(offset, 0, 0);
            timer += Time.deltaTime;
            yield return null;
        }

        // Меняем спрайты
        if (acneSprite != null) acneSprite.SetActive(false);
        if (cleanFaceSprite != null) cleanFaceSprite.SetActive(true);

        // Рука ставит крем на место
        yield return handController.ReturnToolWithHand(this);

        // Потом уходит сама
        yield return handController.MoveToDefaultPosition();

        // Снимаем ссылку
        handController.currentTool = null;
    }
}