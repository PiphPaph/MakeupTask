using UnityEngine;

public class ColorButton : MonoBehaviour
{
    public Color color; // Цвет, который применяется (выбирается в инспекторе).
    public BrushTool brushTool; // Ссылка на инструмент "Кисть".

    public void OnClick()
    {
        brushTool.selectedColor = color; // Передаем цвет в кисть.
        // Дополнительно: можно менять цвет кисти визуально.
    }
}