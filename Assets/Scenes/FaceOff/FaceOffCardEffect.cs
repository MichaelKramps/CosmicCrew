using System;
using System.Collections.Generic;
using UnityEngine;

public class FaceOffCardEffect
{
    FaceOffCardEffectTiming timing;
    FaceOffCardEffectEffect effect;
    FaceOffCardEffectTarget target;
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
        }
    }

    //should be called when a player exists (in a Face-Off)
    public void activateEffect(FaceOffPlayer player)
    {
        switch (this.effect)
        {
            case FaceOffCardEffectEffect.SWAY_COUNTERS:
                swayCounters(player);
                break;
        }
    }

    private bool qualifiesForEffect(FaceOffCard card)
    {
        bool passesFandomTypeFilter = this.fandomTypeFilter == FandomType.NONE ? true : card.getFandomType() == this.fandomTypeFilter;

        //check for attached gear with the fandom type
        if (!passesFandomTypeFilter && card.getAttachedGear().Count > 0)
        {
            Debug.Log("gear found!");
            foreach(FaceOffCard attachedGear in card.getAttachedGear())
            {
                Debug.Log(attachedGear.getCardName() + " has type " + attachedGear.getFandomType());
                if (attachedGear.getFandomType() == this.fandomTypeFilter)
                {
                    passesFandomTypeFilter = true;
                    break;
                }
            }
        }

        Debug.Log(card.getCardName() + " " + passesFandomTypeFilter);

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

    private void swayCounters(FaceOffPlayer player)
    {
        switch (this.target)
        {
            case FaceOffCardEffectTarget.ENTIRE_TEAM:
                foreach(FaceOffCard teamMember in player.getTeam())
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
