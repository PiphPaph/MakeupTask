using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform toolHolder;
    public Transform defaultPosition;
    public Transform middlePosition;
    public float moveSpeed = 4f;
    public RectTransform faceZoneRect;

    [HideInInspector] public MakeupTool currentTool;
    private bool isDragging = false;

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
        yield return MoveToPosition(tool.transform.position);
        tool.transform.SetParent(toolHolder);
        tool.transform.localPosition = Vector3.zero;
        yield return MoveToPosition(middlePosition.position);
        onMidReached?.Invoke();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentTool != null) isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        Vector3 worldPos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
                faceZoneRect, eventData.position, eventData.pressEventCamera, out worldPos))
        {
            transform.position = worldPos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        isDragging = false;

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
        tool.ReturnTool();
        yield return MoveToPosition(defaultPosition.position);
        currentTool = null;
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
        isDragging = true;
    }

}