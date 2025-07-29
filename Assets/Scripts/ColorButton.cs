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
        HandController hand = FindObjectOfType<HandController>();
        if (hand == null || hand.currentTool == null) return;
        
        BrushTool brush = hand.currentTool as BrushTool;
        if (brush != null)
        {
            brush.OnColorSelected(transform, effectSprite);
        }
    }
}