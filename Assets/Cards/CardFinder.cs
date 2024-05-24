﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class CardFinder
{
    private static Dictionary<int, String> cardDictionary = new Dictionary<int, string>
        {
            { 0, "fandom-lurker" },
            { 1, "fandom-participant" },
            { 2, "fandom-club-member" },
            { 3, "hostile-dog" },
        };
    private static Dictionary<String, List<FaceOffCardEffect>> effectDictionary = new Dictionary<string, List<FaceOffCardEffect>>
        {
            { "fandom-lurker", new List<FaceOffCardEffect>() },
            { "fandom-participant", new List<FaceOffCardEffect>() },
            { "fandom-club-member", new List<FaceOffCardEffect>() },
            { "hostile-dog", new List<FaceOffCardEffect>{
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_DISMISS_THIS_CARD,
                    FaceOffCardEffectEffect.REDUCE_COST,
                    FaceOffCardEffectTarget.RECRUITING_HAND
                    ).withEffectAmount(99)
                }
            },
        };

    public static CrewCard getCrewCardFromId(int id)
    {
        return CardFinder.getCrewCardFromName(cardDictionary[id]);
    }

    public static CrewCard getCrewCardFromName(string cardName)
    {
        String assetName = cardName.ToLower().Replace(" ", "-");
        String crewCardPath = "CrewCards/" + assetName;
        CrewCard crewCardWithId = Resources.Load<CrewCard>(crewCardPath);
        return crewCardWithId;
    }

    public List<CrewCard> getStartingDraftPool()
    {
        List<CrewCard> startingPool = new List<CrewCard>();
        foreach (KeyValuePair<int, string> card in cardDictionary)
        {
            CrewCard thisCrewCard = getCrewCardFromId(card.Key);
            for (int numberToInclude = 0; numberToInclude < thisCrewCard.numberAllowedInDeck; numberToInclude++)
            {
                startingPool.Add(thisCrewCard);
            }
        }
        return startingPool;
    }

    public static List<FaceOffCardEffect> getCardEffectsFromName(string cardName)
    {
        if (effectDictionary.ContainsKey(cardName))
        {
            return effectDictionary[cardName];
        }
        return new List<FaceOffCardEffect>();
    }
}
