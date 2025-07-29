using UnityEngine;

public class SpongeTool : MakeupTool
{
    public GameObject Acne; // Спрайт прыщей (активируется при сбросе)

    public override void ApplyEffect()
    {
        /*base.ApplyEffect();
        Acne.SetActive(true);
        handController.ReturnToDefault();*/
    }
}