using System.Collections;
using UnityEngine;

public abstract class MakeupTool : MonoBehaviour
{
    [SerializeField] public HandController handController;
    [HideInInspector] public Transform originalParent;
    [HideInInspector] public Vector3 originalLocalPosition;
    [HideInInspector] public bool isInUse;

    public abstract void ApplyEffect();

    public virtual void OnToolSelected()
    {
        if (handController.currentTool != null && handController.currentTool != this)
        {
            StartCoroutine(SwapAndSelect(handController.currentTool));
            return;
        }

        TakeTool();
    }

    private IEnumerator SwapAndSelect(MakeupTool oldTool)
    {
        yield return handController.ReturnToolWithHand(oldTool);
        
        handController.currentTool = null;
        
        TakeTool();
    }

    private void TakeTool()
    {
        isInUse = true;
        originalParent = transform.parent;
        originalLocalPosition = transform.localPosition;
        handController.SetToolInHand(this);
    }

    public void ReturnTool()
    {
        isInUse = false;
        transform.SetParent(originalParent);
        transform.localPosition = originalLocalPosition;
    }
    
}