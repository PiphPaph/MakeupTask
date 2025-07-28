using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Image handImage;
    public Transform defaultPosition;
    public Transform midPosition;
    public Transform toolHolder;
    public float moveSpeed = 5f;

    [HideInInspector] public MakeupTool currentTool;
    [HideInInspector] public bool isDragging;

    private Vector3 targetPosition;
    public bool isReturning;

    void Start()
    {
        targetPosition = defaultPosition.position;
        handImage.transform.position = defaultPosition.position; // Сразу ставим руку в дефолт.
    }

    void Update()
    {
        if (isReturning)
        {
            handImage.transform.position = Vector3.Lerp(handImage.transform.position, defaultPosition.position, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(handImage.transform.position, defaultPosition.position) < 0.1f)
            {
                isReturning = false;
                if (currentTool != null)
                {
                    currentTool.ReturnTool();
                    currentTool = null;
                }
            }
        }
        else if (!isDragging) // Только если не перетаскиваем
        {
            handImage.transform.position = Vector3.Lerp(handImage.transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        if (currentTool != null && !isDragging)
        {
            // Плавно перемещаем инструмент в руку.
            currentTool.transform.position = Vector3.Lerp(
                currentTool.transform.position, 
                toolHolder.position, 
                moveSpeed * Time.deltaTime
            );
        }
    }

    public void MoveToTool(MakeupTool tool)
    {
        currentTool = tool;
        targetPosition = tool.transform.position;
    }

    public void MoveToMidPosition()
    {
        if (isReturning) return; // Не прерываем возврат
        targetPosition = midPosition.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentTool != null && !isReturning)
        {
            isDragging = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)handImage.transform.parent, 
                eventData.position, 
                eventData.pressEventCamera, 
                out Vector2 localPoint
            );
            handImage.transform.localPosition = localPoint;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            isDragging = false;
            if (currentTool != null)
            {
                currentTool.CheckFaceZone();
            }
        }
    }

    public void ReturnToDefault()
    {
        // 1. Сбрасываем все состояния
        isDragging = false;
        isReturning = true;
        CancelInvoke(); // Отменяем все запланированные вызовы
    
        // 2. Возвращаем инструмент НЕМЕДЛЕННО
        if (currentTool != null) 
        {
            currentTool.ReturnTool();
            currentTool = null;
        }
    
        // 3. Плавный возврат руки без промежуточных точек
        StartCoroutine(SmoothReturn());
    }

    private IEnumerator SmoothReturn()
    {
        // Добавляем проверку на midPosition
        while (Vector3.Distance(transform.position, defaultPosition.position) > 0.1f)
        {
            // Плавное движение к defaultPosition
            transform.position = Vector3.Lerp(
                transform.position,
                isReturning ? defaultPosition.position : targetPosition,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }
        isReturning = false;
    }
    public void ForceReset()
    {
        isDragging = false;
        isReturning = true;
        targetPosition = defaultPosition.position;
    
        if (currentTool != null)
        {
            currentTool.ReturnTool();
            currentTool = null;
        }
    }
}