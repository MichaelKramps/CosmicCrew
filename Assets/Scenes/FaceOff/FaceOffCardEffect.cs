using System;
public class FaceOffCardEffect
{
    FaceOffCardEffectTiming timing;
    FaceOffCardEffectEffect effect;
    FaceOffCardEffectTarget target;
    int effectAmount = 0;
    //FaceOffCardEffectTargetFilter targetFilter;

    public FaceOffCardEffect(
        FaceOffCardEffectTiming timing,
        FaceOffCardEffectEffect effect,
        FaceOffCardEffectTarget target)
    {
        this.timing = timing;
        this.effect = effect;
        this.target = target;
    }

    public FaceOffCardEffect withEffectAmount(int amount)
    {
        this.effectAmount = amount;
        return this;
    }
}
