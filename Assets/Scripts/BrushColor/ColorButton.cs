using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{
    public GameObject effectSprite; // нужный спрайт для активации с нужным цветом

    private Button _button;

    void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(NotifyBrush);
    }

    void NotifyBrush()
    {
        HandController hand = FindObjectOfType<HandController>(); // находим руку
        if (hand == null || hand.currentTool == null) return; // если не находим или в руке нет тулза - возврат
        
        BrushTool brush = hand.currentTool as BrushTool; // говорим, что кистью становится используемый инструмент кисти
        if (brush != null)
        {
            brush.OnColorSelected(transform, effectSprite); //если кисть есть - применяем нужный спрайт указанный в инспекторе
        }
    }
}