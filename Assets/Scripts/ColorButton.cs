using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{
    public GameObject effectSprite;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(NotifyBrush);
    }

    void NotifyBrush()
    {
        // Найдём текущий инструмент в руке
        HandController hand = FindObjectOfType<HandController>();
        if (hand == null || hand.currentTool == null) return;

        // Проверим, что это именно кисть
        BrushTool brush = hand.currentTool as BrushTool;
        if (brush != null)
        {
            brush.OnColorSelected(transform, effectSprite);
        }
    }
}