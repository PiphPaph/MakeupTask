using UnityEngine;
using UnityEngine.UI;

public class MakeupTool : MonoBehaviour
{
    public HandController handController;
    public Transform originalPosition;
    public GameObject[] affectedSprites; // Спрайты, которые меняются (например, lipstick01, shadow01 и т.д.)
    public bool isColorTool; // Для теней (нужен выбор цвета)
    public Transform originalParent;
    public Vector3 originalLocalPosition;

    
    void Start()
    {
        originalParent = transform.parent;
        originalLocalPosition = transform.localPosition;
    }
    public void OnToolClicked()
    {
        if (handController.currentTool != null) 
            handController.currentTool.ReturnTool();

        handController.currentTool = this;
        transform.SetParent(handController.toolHolder);
        transform.localPosition = Vector3.zero;
    
        // Возвращаем этот вызов!
        handController.MoveToMidPosition(); // ← Вот это строка
    }

    private void MoveToMidAfterDelay()
    {
        handController.MoveToMidPosition();
    }

    public void CheckFaceZone()
    {
        // Получаем RectTransform FaceZone
        RectTransform faceZoneRect = GameObject.FindGameObjectWithTag("FaceZone").GetComponent<RectTransform>();
    
        // Позиция руки в экранных координатах
        Vector2 handScreenPos = RectTransformUtility.WorldToScreenPoint(
            null, 
            handController.handImage.transform.position
        );

        // Проверяем, попадает ли точка в Rect FaceZone
        bool isOnFace = RectTransformUtility.RectangleContainsScreenPoint(
            faceZoneRect, 
            handScreenPos, 
            null
        );

        Debug.Log($"Рука в зоне лица: {isOnFace} | Позиция: {handScreenPos}");

        if (isOnFace)
        {
            ApplyEffect();
        }
        else
        {
            handController.ForceReset();
        }
    }

    public virtual void ApplyEffect()
    {
        // Логика применения эффекта (переопределяется в дочерних классах)
        handController.ReturnToDefault();
    }

    public virtual void ResetTool()
    {
        /*foreach (var sprite in affectedSprites)
        {
            sprite.SetActive(false);
        }
        Debug.Log("ResetTool вызван! Проверь, не отключаются ли прыщи здесь.");
        // Убираем отключение спрайтов - они должны сбрасываться только через ApplyEffect
        Debug.Log("ResetTool вызван, но ничего не отключаем");*/
        
        
    }

    public virtual void ReturnTool()
    {
        if (this == null) return;
    
        // Возвращаем крем в исходного родителя (например, "ToolsContainer")
        transform.SetParent(originalParent); 
        transform.localPosition = originalLocalPosition;
        transform.localRotation = Quaternion.identity;
    }
}