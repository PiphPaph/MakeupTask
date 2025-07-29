using System.Collections;
using UnityEngine;

public class Lipstick : MakeupTool
{
    [Header("Цветовой спрайт")]
    public GameObject lipstickSprite; // Активируемый спрайт этого цвета

    [Header("Ссылка на текущий активный спрайт")]
    public static GameObject currentLipstickSprite; // Статический — для сброса старого цвета

    public float applyTime = 1.2f;

    public override void ApplyEffect()
    {
        StartCoroutine(ApplyRoutine());
    }

    private IEnumerator ApplyRoutine()
    {
        // Анимация — горизонтальное движение по X
        Vector3 startPos = handController.transform.position;
        float timer = 0;

        while (timer < applyTime)
        {
            float offset = Mathf.Sin(timer * 10f) * 10f;
            handController.transform.position = startPos + new Vector3(offset, 0, 0);
            timer += Time.deltaTime;
            yield return null;
        }

        // Отключаем предыдущий цвет, если есть
        if (currentLipstickSprite != null)
            currentLipstickSprite.SetActive(false);

        // Включаем текущий спрайт
        if (lipstickSprite != null)
        {
            lipstickSprite.SetActive(true);
            currentLipstickSprite = lipstickSprite;
        }

        // Рука ставит помаду на место
        yield return handController.ReturnToolWithHand(this);

        // Рука уходит на дефолтную позицию
        yield return handController.MoveToDefaultPosition();

        // Обнуляем ссылку на текущий инструмент
        handController.currentTool = null;
    }
}