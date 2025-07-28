using UnityEngine;
using UnityEngine.UI;

public class BookTabButton : MonoBehaviour
{
    public GameObject pageToShow;         // нужная страница
    public Sprite activeSprite;           // "включённая" вкладка
    public Sprite inactiveSprite;         // обычная вкладка
    public Image targetImage;             // чей Image меняем (например, фон кнопки)
    public TabGroup tabGroup;             // ссылка на общий контроллер

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        tabGroup.OnTabSelected(this);
    }

    public void SetActiveVisual(bool isActive)
    {
        if (targetImage != null)
            targetImage.sprite = isActive ? activeSprite : inactiveSprite;
    }
}