public class LipstickTool : MakeupTool
{
    public override void ApplyEffect()
    {
        base.ApplyEffect();
        affectedSprites[0].SetActive(true); // Активируем нужный спрайт помады
        handController.ReturnToDefault();
    }
}