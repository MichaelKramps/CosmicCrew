using System;
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
            CardFinder.getCrewCardFromId(0),
            CardFinder.getCrewCardFromId(0),
            CardFinder.getCrewCardFromId(1),
            CardFinder.getCrewCardFromId(1),
            CardFinder.getCrewCardFromId(2),
            CardFinder.getCrewCardFromId(2),
        };

        unspentRecruitingTokens = 3;
        tokensGenerated = 2;

        summarizeFaceOffDeck();
    }

    public List<CrewCard> getPlayerDeck()
    {
        return this.faceOffDeck;
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
        faceOffDeck.OrderBy(card => card.cardId).ToList();


        int currentId = -1;
        foreach(CrewCard card in faceOffDeck)
        {
            if (card.cardId == currentId)
            {
                faceOffDeckSummary[card] = faceOffDeckSummary[card] + 1;
            } else
            {
                faceOffDeckSummary.Add(card, 1);
                currentId = card.cardId;
            }
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
                faceOffDeck.RemoveAt(cardIndex);
                summarizeFaceOffDeck();
                return true;
            }
        }
        return false;
    }
}
