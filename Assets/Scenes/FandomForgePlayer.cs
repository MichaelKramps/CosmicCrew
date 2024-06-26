﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FandomForgePlayer
{
    private List<CrewCard> faceOffDeck;
    private Dictionary<CrewCard, int> faceOffDeckSummary;
    private int unspentRecruitingTokens;
    private int tokensGenerated;

    public FandomForgePlayer()
    {
        faceOffDeck = new List<CrewCard> {
            CardFinder.getCrewCardFromId(14),
            CardFinder.getCrewCardFromId(0),
            CardFinder.getCrewCardFromId(19),
            CardFinder.getCrewCardFromId(27),
            CardFinder.getCrewCardFromId(21),
            CardFinder.getCrewCardFromId(2),
            CardFinder.getCrewCardFromId(26)
        };

        unspentRecruitingTokens = 30;
        tokensGenerated = 2;

        summarizeFaceOffDeck();
    }

    public List<CrewCard> getPlayerDeck()
    {
        return this.faceOffDeck;
    }

    public void payoutDividends()
    {
        this.unspentRecruitingTokens += tokensGenerated;
    }

    public void increaseDividends(int amountToIncrease)
    {
        this.tokensGenerated += amountToIncrease;
    }

    public void invest()
    {
        tokensGenerated += 1;
    }

    public void pay(int cost)
    {
        unspentRecruitingTokens -= cost;
    }

    public void addTokens(int tokens)
    {
        unspentRecruitingTokens += tokens;
    }

    public bool canAffordToPay(int price)
    {
        return unspentRecruitingTokens >= price;
    }

    public void addToDeck(CrewCard cardToAdd)
    {
        faceOffDeck.Add(cardToAdd);
        summarizeFaceOffDeck();
    }

    public int currentRecruitingTokens()
    {
        return unspentRecruitingTokens;
    }

    public int currentTokensGenerated()
    {
        return tokensGenerated;
    }

    private void summarizeFaceOffDeck()
    {
        faceOffDeckSummary = new Dictionary<CrewCard, int>();


        List<int> foundIds = new List<int>();
        foreach(CrewCard card in faceOffDeck)
        {
            if (foundIds.Contains(card.cardId))
            {
                faceOffDeckSummary[card] = faceOffDeckSummary[card] + 1;
            } else
            {
                faceOffDeckSummary.Add(card, 1);
                foundIds.Add(card.cardId)
;            }
        }
    }

    public Dictionary<CrewCard, int> getFaceOffDeckSummary()
    {
        return faceOffDeckSummary;
    }

    public bool dismissCard(CrewCard cardToDismiss)
    {
        for(int cardIndex = 0; cardIndex < faceOffDeck.Count; cardIndex++)
        {
            CrewCard thisCard = faceOffDeck[cardIndex];
            if (thisCard.cardId == cardToDismiss.cardId)
            {
                //cannot use FaceOffCard methods because no GameObject exists here
                this.activateEffectsFor(CardFinder.getCardEffectsFromCrewCard(thisCard), FaceOffCardEffectTiming.WHEN_YOU_DISMISS_THIS_CARD);
                faceOffDeck.RemoveAt(cardIndex);
                summarizeFaceOffDeck();
                return true;
            }
        }
        return false;
    }

    public void activateEffectsFor(List<FaceOffCardEffect> effects, FaceOffCardEffectTiming timing)
    {
        foreach(FaceOffCardEffect effect in effects)
        {
            if (effect.timingIs(timing))
            {
                effect.activateEffect();
            }
        }
    }
}
