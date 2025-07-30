using UnityEngine;
using UnityEngine.UI;

public class BookTabButton : MonoBehaviour
{
    public GameObject pageToShow;         // лист для активации
    public Sprite activeSprite;           // "активированная" вкладка (красная)"
    public Sprite inactiveSprite;         // "не активированаая" вкладка (синяя)
    public Image targetImage;             // Image в инспекторе, чтобы менять картинку
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