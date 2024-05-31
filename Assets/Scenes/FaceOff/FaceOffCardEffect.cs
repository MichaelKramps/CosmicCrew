using System;
using System.Collections.Generic;
using UnityEngine;

public class FaceOffCardEffect
{
    FaceOffCardEffectTiming timing;
    FaceOffCardEffectEffect effect;
    FaceOffCardEffectTarget target;
    FaceOffCard effectOwner;
    int effectAmount = 0;
    FandomType fandomTypeFilter = FandomType.NONE;
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

    public FaceOffCardEffect clone()
    {
        FaceOffCardEffect selfClone = new FaceOffCardEffect(timing, effect, target)
            .withFandomFilter(fandomTypeFilter)
            .withEffectAmount(effectAmount);
        
        return selfClone;
    }

    public FaceOffCardEffect withEffectOwner(FaceOffCard card)
    {
        this.effectOwner = card;
        return this;
    }

    public FaceOffCardEffect withEffectAmount(int amount)
    {
        this.effectAmount = amount;
        return this;
    }

    public FaceOffCardEffect withFandomFilter(FandomType fandomType)
    {
        this.fandomTypeFilter = fandomType;
        return this;
    }

    public bool timingIs(FaceOffCardEffectTiming timing)
    {
        return this.timing == timing;
    }

    //should be called when there is no player (outside of Face-Off)
    //most likely when in the recruitment phase
    public void activateEffect()
    {
        switch (this.effect)
        {
            case FaceOffCardEffectEffect.REDUCE_COST:
                reduceCost();
                break;
            case FaceOffCardEffectEffect.SWAY_COUNTERS:
                swayCounters();
                break;
        }
    }

    private bool qualifiesForEffect(FaceOffCard card)
    {
        bool passesFandomTypeFilter = this.fandomTypeFilter == FandomType.NONE ? true : card.getFandomType() == this.fandomTypeFilter;

        //check for attached gear with the fandom type
        if (!passesFandomTypeFilter && card.getAttachedGear().Count > 0)
        {
            foreach(FaceOffCard attachedGear in card.getAttachedGear())
            {
                if (attachedGear.getFandomType() == this.fandomTypeFilter)
                {
                    passesFandomTypeFilter = true;
                    break;
                }
            }
        }

        return passesFandomTypeFilter;
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

    private void swayCounters()
    {
        switch (this.target)
        {
            case FaceOffCardEffectTarget.SELF:
                this.effectOwner.addSwayCounters(this.effectAmount);
                break;
            case FaceOffCardEffectTarget.ENTIRE_TEAM:
                foreach(FaceOffCard teamMember in effectOwner.getCardOwner().getTeam())
                {
                    if (qualifiesForEffect(teamMember))
                    {
                        teamMember.addSwayCounters(this.effectAmount);
                    }
                }
                break;
        }
    }
}
