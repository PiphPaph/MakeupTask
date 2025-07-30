using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform toolHolder; // пустой дочерний объект руки, где должен быть выбранный инструмент
    public Transform defaultPosition; // дефолтная позиция руки, куда она должна возвращаться
    public Transform middlePosition; // позиция руки куда она приходит после выбора инструмента
    public float moveSpeed = 4f; // скорость передвижения руки
    public RectTransform faceZoneRect; // пустой объект зоны лица

    [HideInInspector] public MakeupTool currentTool; // выбранный инструмент
    private bool _isDragging;

    public void SetToolInHand(MakeupTool tool, System.Action onMidReached)
    {
        currentTool = tool;
        StartCoroutine(MoveToToolRoutine(tool, onMidReached));
    }
    public void SetToolInHand(MakeupTool tool)
    {
        SetToolInHand(tool, null);
    }

    private IEnumerator MoveToToolRoutine(MakeupTool tool, System.Action onMidReached)
    {
        yield return MoveToPosition(tool.transform.position); // перемещение к выбранному тулзу
        tool.transform.SetParent(toolHolder); // изменение родителя тулза на держатель
        tool.transform.localPosition = Vector3.zero; // центровка тулза относительно нового родителя
        yield return MoveToPosition(middlePosition.position); // перемещение руки в мид позицию
        onMidReached?.Invoke();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentTool != null) _isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDragging) return;

        Vector3 worldPos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
                faceZoneRect, eventData.position, eventData.pressEventCamera, out worldPos))
        {
            transform.position = worldPos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        if (IsInFaceZone())
            currentTool.ApplyEffect();
        else
            StartCoroutine(ResetHandAndTool(currentTool));
    }

    private bool IsInFaceZone()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(
            faceZoneRect,
            transform.position,
            null
        );
    }

    public IEnumerator ResetHandAndTool(MakeupTool tool) 
    {
        tool.ReturnTool(); // возврат тулза на место
        yield return MoveToPosition(defaultPosition.position); // перемещение руки в дефолтную позицию
        currentTool = null; // очистка текущего выбранного тулза
    }

    public IEnumerator MoveToPosition(Vector3 target)
    {
        Vector3 start = transform.position;
        float t = 0;

        while (t < 1f)
        {
            transform.position = Vector3.Lerp(start, target, t);
            t += Time.deltaTime * moveSpeed;
            yield return null;
        }
    }

    public IEnumerator MoveToDefaultPosition()
    {
        Vector3 start = transform.position;
        Vector3 end = defaultPosition.position;

        float t = 0;
        while (t < 1f)
        {
            transform.position = Vector3.Lerp(start, end, t);
            t += Time.deltaTime * moveSpeed;
            yield return null;
        }
    }
    public IEnumerator ReturnToolWithHand(MakeupTool tool)
    {
        yield return MoveToPosition(tool.originalParent.position);

        tool.transform.SetParent(tool.originalParent);
        tool.transform.localPosition = tool.originalLocalPosition;

        tool.ReturnTool();

        currentTool = null;
    }
    public void EnableDragging(MakeupTool tool)
    {
        currentTool = tool;
        _isDragging = true;
    }

}