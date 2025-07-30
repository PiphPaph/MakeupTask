using System.Collections;
using UnityEngine;

public class BrushTool : MakeupTool
{
    private GameObject _selectedEffectSprite;
    private Transform _selectedColorTransform;

    private bool _waitingForColor;
    private bool _draggingEnabled;
    
    private static GameObject _currentEffectSprite;

    public override void OnToolSelected()
    {
        if (isInUse)
        {
            handController.ResetHandAndTool(this);
            return;
        }
        
        if (handController.currentTool != null && handController.currentTool != this)
        {
            StartCoroutine(SwapAndSelect(handController.currentTool));
            return;
        }

        isInUse = true;
        originalParent = transform.parent;
        originalLocalPosition = transform.localPosition;

        handController.SetToolInHand(this, () =>
        {
            _waitingForColor = true;
        });
    }

    private IEnumerator SwapAndSelect(MakeupTool oldTool)
    {
        yield return handController.ReturnToolWithHand(oldTool);
        OnToolSelected();
    }

    public void OnColorSelected(Transform colorTransform, GameObject effectSprite)
    {
        if (!_waitingForColor) return;
        if (_currentEffectSprite != null)
            _currentEffectSprite.SetActive(false);

        _selectedColorTransform = colorTransform;
        _selectedEffectSprite = effectSprite;

        StopAllCoroutines();
        StartCoroutine(AnimateBrushAtColor());
    }
   
    private IEnumerator AnimateBrushAtColor()
    {
        yield return handController.MoveToPosition(_selectedColorTransform.position);
        
        Vector3 start = handController.transform.position;
        float time = 0;
        float duration = 0.5f;

        while (time < duration)
        {
            float offset = Mathf.Sin(time * 10f) * 5f;
            handController.transform.position = start + new Vector3(offset, 0, 0);
            time += Time.deltaTime;
            yield return null;
        }

        yield return handController.MoveToPosition(handController.middlePosition.position);

        if (!_draggingEnabled)
        {
            handController.EnableDragging(this);
            _draggingEnabled = true;
        }
    }

    public override void ApplyEffect()
    {
        if (_selectedEffectSprite == null) return;

        StartCoroutine(ApplyBrush());
    }

    private IEnumerator ApplyBrush()
    {
        Vector3 start = handController.transform.position;
        float timer = 0f;
        float duration = 0.7f;

        while (timer < duration)
        {
            float offset = Mathf.Sin(timer * 10f) * 10f;
            handController.transform.position = start + new Vector3(offset, 0, 0);
            timer += Time.deltaTime;
            yield return null;
        }

        _selectedEffectSprite.SetActive(true);
        _currentEffectSprite = _selectedEffectSprite;

        yield return handController.ReturnToolWithHand(this);
        yield return handController.MoveToDefaultPosition();

        handController.currentTool = null;
        _selectedEffectSprite = null;
        _selectedColorTransform = null;
        _waitingForColor = false;
        _draggingEnabled = false;
        isInUse = false;
    }
}
