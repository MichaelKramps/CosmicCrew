using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FaceOffCardEffect
{
    FaceOffCardEffectTiming timing;
    FaceOffCardEffectEffect effect;
    FaceOffCardEffectTarget target;
    FaceOffCard effectOwner;
    int effectAmount = 0;
    FandomType fandomTypeFilter = FandomType.NONE;
    //FaceOffCardEffectTargetFilter targetFilter;

    private System.Random random = new System.Random();

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
            case FaceOffCardEffectEffect.SWAY_COUNTERS:
                swayCounters();
                break;
            case FaceOffCardEffectEffect.REDUCE_COST:
                reduceCost();
                break;
            case FaceOffCardEffectEffect.GAIN_RECRUITING_TOKENS:
                gainRecruitingTokens();
                break;
            case FaceOffCardEffectEffect.DRAW_CARDS:
                drawCards();
                break;
            case FaceOffCardEffectEffect.CYCLE:
                cycle();
                break;
            case FaceOffCardEffectEffect.REFRESH:
                refresh();
                break;
            case FaceOffCardEffectEffect.PLAY_CARD_FROM_CYCLE:
                playCardFromCycle();
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

    private void gainRecruitingTokens()
    {
        FandomForge.increaseDividends(this.effectAmount);
    }

    private void drawCards()
    {
        this.effectOwner.getCardOwner().drawXCardsIntoHand(this.effectAmount);
        this.effectOwner.getCardOwner().repositionCards();
    }

    private void refresh()
    {
        FandomForge.freeRefresh();
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
            case FaceOffCardEffectTarget.RANDOM_FANATIC:
                if (effectOwner.getCardOwner().getTeam().Count > 0)
                {
                    FaceOffCard randomFanatic = effectOwner.getCardOwner().getTeam()[random.Next(effectOwner.getCardOwner().getTeam().Count)];
                    randomFanatic.addSwayCounters(this.effectAmount);
                }
                break;
            case FaceOffCardEffectTarget.LEFTMOST_FANATIC:
                foreach (FaceOffCard teamMember in effectOwner.getCardOwner().getTeam())
                {
                    if (qualifiesForEffect(teamMember))
                    {
                        teamMember.addSwayCounters(this.effectAmount);
                        break;
                    }
                }
                break;
            case FaceOffCardEffectTarget.RIGHTMOST_FANATIC:
                //iterate backwards through team
                for (int index = effectOwner.getCardOwner().getTeam().Count - 1; index > -1; index++)
                {
                    FaceOffCard teamMember = effectOwner.getCardOwner().getTeam()[index];
                    if (qualifiesForEffect(teamMember))
                    {
                        teamMember.addSwayCounters(this.effectAmount);
                        break;
                    }
                }
                break;
        }
    }

    private void cycle()
    {
        this.effectOwner.getCardOwner().cycleXCards(this.effectAmount);
    }

    private void playCardFromCycle()
    {
        this.effectOwner.getCardOwner().playCardFromCycle(this.effectOwner);
    }
}
