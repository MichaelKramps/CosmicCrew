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

    public bool timingIs(FaceOffCardEffectTiming timing)
    {
        return this.timing == timing;
    }

    public void activateEffect()
    {
        switch (this.effect)
        {
            case FaceOffCardEffectEffect.REDUCE_COST:
                reduceCost();
                break;
        }
    }

    private void reduceCost()
    {
        switch (this.target)
        {
            case FaceOffCardEffectTarget.RECRUITING_HAND:
                FandomForge.setOneTimeRecruitingCostReduction(this.effectAmount);
                break;
        }
    }
}
