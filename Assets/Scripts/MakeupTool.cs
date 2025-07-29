using System.Collections;
using UnityEngine;

public abstract class MakeupTool : MonoBehaviour
{
    [SerializeField] public HandController handController;
    [HideInInspector] public Transform originalParent;
    [HideInInspector] public Vector3 originalLocalPosition;
    [HideInInspector] public bool isInUse = false;

    public abstract void ApplyEffect();

    public virtual void OnToolSelected()
    {
        // Если в руке уже другой инструмент
        if (handController.currentTool != null && handController.currentTool != this)
        {
            StartCoroutine(SwapAndSelect(handController.currentTool));
            return;
        }

        // Если повторный выбор — вернуть обратно
        if (isInUse)
        {
            handController.ResetHandAndTool(this);
            return;
        }

        TakeTool();
    }

    private IEnumerator SwapAndSelect(MakeupTool oldTool)
    {
        // Вернём старый инструмент
        yield return handController.ReturnToolWithHand(oldTool);

        // Обнулим currentTool вручную, если ReturnTool этого не делает
        handController.currentTool = null;

        // Берём новый
        TakeTool();
    }

    protected virtual void TakeTool()
    {
        isInUse = true;
        originalParent = transform.parent;
        originalLocalPosition = transform.localPosition;
        handController.SetToolInHand(this);
    }

    public virtual void ReturnTool()
    {
        isInUse = false;
        transform.SetParent(originalParent);
        transform.localPosition = originalLocalPosition;
    }
    
    /*private IEnumerator SwapTools(MakeupTool oldTool)
    {
        yield return handController.ReturnToolWithHand(oldTool);
        OnToolSelected(); // повторно вызвать себя уже после возврата
    }*/
}