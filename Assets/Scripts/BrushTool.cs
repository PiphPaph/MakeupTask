using UnityEngine;
using UnityEngine.UI;

public class BrushTool : MakeupTool
{
    public Color selectedColor; // Выбранный цвет (заполняется через ColorButton).
    public Image brushTip; // Визуал кончика кисти (меняем цвет).

    public override void ApplyEffect()
    {
        if (selectedColor != null)
        {
            foreach (var sprite in affectedSprites)
            {
                sprite.SetActive(true); // Активируем спрайт теней.
                sprite.GetComponent<Image>().color = selectedColor; // Красим.
            }
        }
        handController.ReturnToDefault();
    }
}