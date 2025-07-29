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
        if (isInUse) 
        {
            handController.ResetHandAndTool(this);
            return;
        }

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
}