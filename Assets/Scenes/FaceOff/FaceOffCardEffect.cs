using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class FaceOffCardEffect
{
    FaceOffCardEffectTiming timing;
    FaceOffCardEffectEffect effect;
    FaceOffCardEffectTarget target;
    FaceOffCardEffectTarget secondTarget = FaceOffCardEffectTarget.NONE;
    FaceOffCard effectOwner;
    int constantEffectAmount = 0;
    VariableEffectAmount variableEffectAmount = VariableEffectAmount.NONE;
    FandomType fandomTypeFilter = FandomType.NONE;
    CardType cardTypeFilter = CardType.NONE;
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
            .withConstantEffectAmount(constantEffectAmount)
            .withVariableEffectAmount(variableEffectAmount)
            .withSecondTarget(secondTarget);
        
        return selfClone;
    }

    public FaceOffCardEffect withEffectOwner(FaceOffCard card)
    {
        this.effectOwner = card;
        return this;
    }

    public FaceOffCardEffect withConstantEffectAmount(int amount)
    {
        this.constantEffectAmount = amount;
        return this;
    }

    public FaceOffCardEffect withVariableEffectAmount(VariableEffectAmount variableEffectAmount)
    {
        this.variableEffectAmount = variableEffectAmount;
        return this;
    }

    public FaceOffCardEffect withFandomFilter(FandomType fandomType)
    {
        this.fandomTypeFilter = fandomType;
        return this;
    }

    public FaceOffCardEffect withCardTypeFilter(CardType cardType)
    {
        this.cardTypeFilter = cardType;
        return this;
    }

    public FaceOffCardEffect withSecondTarget(FaceOffCardEffectTarget target)
    {
        this.secondTarget = target;
        return this;
    }

    public bool timingIs(FaceOffCardEffectTiming timing)
    {
        return this.timing == timing;
    }

    private int determineEffectAmount()
    {
        if (this.variableEffectAmount != VariableEffectAmount.NONE)
        {
            switch (this.variableEffectAmount)
            {
                case VariableEffectAmount.NUMBER_CARDS_IN_DECK:
                    return this.effectOwner.getCardOwner().deckSize();
                case VariableEffectAmount.NUMBER_CARDS_IN_DISCARD:
                    return this.effectOwner.getCardOwner().discardSize();
                case VariableEffectAmount.SWAY_OF_TOP_CARD_IN_DECK:
                    FaceOffCard topCard = this.effectOwner.getCardOwner().getDeck()[0];
                    return topCard.getCardBaseSway();
                default:
                    return 1;
            }
        } else
        {
            return this.constantEffectAmount;
        }
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
            case FaceOffCardEffectEffect.MOVE_CARDS:
                moveCards();
                break;
            case FaceOffCardEffectEffect.DISMISS:
                dismiss();
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

        bool passesCardTypeFilter = this.cardTypeFilter == CardType.NONE ? true : card.getCardType() == this.cardTypeFilter;

        return passesFandomTypeFilter && passesCardTypeFilter;
    }

    private void reduceCost()
    {
        switch (this.target)
        {
            case FaceOffCardEffectTarget.RECRUITING_HAND:
                FandomForge.setOneTimeRecruitingCostReduction(this.determineEffectAmount());
                break;
        }
    }

    private void gainRecruitingTokens()
    {
        FandomForge.increaseDividends(this.determineEffectAmount());
    }

    private void drawCards()
    {
        this.effectOwner.getCardOwner().drawXCardsIntoHand(this.determineEffectAmount());
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
                this.effectOwner.addSwayCounters(this.determineEffectAmount());
                break;
            case FaceOffCardEffectTarget.ENTIRE_TEAM:
                foreach(FaceOffCard teamMember in effectOwner.getCardOwner().getTeam())
                {
                    if (qualifiesForEffect(teamMember))
                    {
                        teamMember.addSwayCounters(this.determineEffectAmount());
                    }
                }
                break;
            case FaceOffCardEffectTarget.RANDOM_FANATIC:
                if (effectOwner.getCardOwner().getTeam().Count > 0)
                {
                    FaceOffCard randomFanatic = effectOwner.getCardOwner().getTeam()[random.Next(effectOwner.getCardOwner().getTeam().Count)];
                    randomFanatic.addSwayCounters(this.determineEffectAmount());
                }
                break;
            case FaceOffCardEffectTarget.LEFTMOST_FANATIC:
                foreach (FaceOffCard teamMember in effectOwner.getCardOwner().getTeam())
                {
                    if (qualifiesForEffect(teamMember))
                    {
                        teamMember.addSwayCounters(this.determineEffectAmount());
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
                        teamMember.addSwayCounters(this.determineEffectAmount());
                        break;
                    }
                }
                break;
        }
    }

    private void cycle()
    {
        this.effectOwner.getCardOwner().cycleXCards(this.determineEffectAmount());
    }

    private void playCardFromCycle()
    {
        this.effectOwner.getCardOwner().playCardFromCycle(this.effectOwner);
    }

    private void moveCards()
    {
        switch (this.target)
        {
            case FaceOffCardEffectTarget.DECK:
                for (int index = this.effectOwner.getCardOwner().getDeck().Count - 1; index > -1; index--)
                {
                    FaceOffCard card = this.effectOwner.getCardOwner().getDeck()[index];
                    if (qualifiesForEffect(card))
                    {
                        this.effectOwner.getCardOwner().getDeck().RemoveAt(index);
                        this.moveCardToDestination(card);
                    }
                }
                break;
            case FaceOffCardEffectTarget.DISCARD:
                for (int index = this.effectOwner.getCardOwner().getDiscard().Count - 1; index > -1; index--)
                {
                    FaceOffCard card = this.effectOwner.getCardOwner().getDiscard()[index];
                    if (qualifiesForEffect(card))
                    {
                        this.effectOwner.getCardOwner().getDiscard().RemoveAt(index);
                        this.moveCardToDestination(card);
                    }
                }
                break;
        }
    }

    private void moveCardToDestination(FaceOffCard card)
    {
        switch (this.secondTarget)
        {
            case FaceOffCardEffectTarget.DECK:
                this.effectOwner.getCardOwner().getDeck().Add(card);
                break;
            case FaceOffCardEffectTarget.DISCARD:
                this.effectOwner.getCardOwner().getDiscard().Add(card);
                break;
        }
        this.effectOwner.getCardOwner().repositionCards();
    }

    private void dismiss()
    {
        switch (this.target)
        {
            case FaceOffCardEffectTarget.SELF:
                this.effectOwner.getCardOwner().dismissCard(this.effectOwner);
                break;
            case FaceOffCardEffectTarget.NONE:
                //we are in the recruiting phase
                FandomForge.currentRecruitingAction = CurrentRecruitingAction.Dismiss;
                FandomForge.freeDismiss();
                SceneManager.LoadScene("Cards In Deck");
                break;
        }
    }
}
