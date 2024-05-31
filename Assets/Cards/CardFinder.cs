using System;
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
            { 4, "rainbow-jersey" },
            { 5, "king-of-the-mountains" },
            { 6, "lady-cyclist" },
            { 7, "expensive-wheels" },
            { 8, "well-fitted-seat" },
            { 9, "gc-rider" },
            { 10, "best-young-rider" },
            { 11, "weight-loss-seeker" },
        };
    private static Dictionary<String, List<FaceOffCardEffect>> effectDictionary = new Dictionary<string, List<FaceOffCardEffect>>
        {
            { "hostile-dog", new List<FaceOffCardEffect>{
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_DISMISS_THIS_CARD,
                    FaceOffCardEffectEffect.REDUCE_COST,
                    FaceOffCardEffectTarget.RECRUITING_HAND)
                    .withEffectAmount(99)
                }
            },
            { "rainbow-jersey", new List<FaceOffCardEffect>{
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_GIVE_THIS_TO_A_FANATIC,
                    FaceOffCardEffectEffect.SWAY_COUNTERS,
                    FaceOffCardEffectTarget.ENTIRE_TEAM)
                    .withEffectAmount(2)
                    .withFandomFilter(FandomType.CYCLING)
                }
            },
            { "king-of-the-mountains", new List<FaceOffCardEffect>{
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_DRAW_A_CARD,
                    FaceOffCardEffectEffect.SWAY_COUNTERS,
                    FaceOffCardEffectTarget.SELF)
                    .withEffectAmount(1)
                }
            },
            { "lady-cyclist", new List<FaceOffCardEffect>{
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_GEAR_IS_ATTACHED_TO_THIS_CARD,
                    FaceOffCardEffectEffect.SWAY_COUNTERS,
                    FaceOffCardEffectTarget.SELF)
                    .withEffectAmount(2)
                }
            },
            { "expensive-wheels", new List<FaceOffCardEffect>{
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_DRAW_THIS_CARD,
                    FaceOffCardEffectEffect.SWAY_COUNTERS,
                    FaceOffCardEffectTarget.ENTIRE_TEAM)
                    .withEffectAmount(1)
                }
            },
            { "well-fitted-seat", new List<FaceOffCardEffect>{
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_DRAW_THIS_CARD,
                    FaceOffCardEffectEffect.SWAY_COUNTERS,
                    FaceOffCardEffectTarget.RANDOM_FANATIC)
                    .withEffectAmount(2)
                }
            },
            { "gc-rider", new List<FaceOffCardEffect>{
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_CYCLE_THIS_CARD,
                    FaceOffCardEffectEffect.PLAY_CARD_FROM_CYCLE,
                    FaceOffCardEffectTarget.NONE)
                }
            },
            { "best-young-rider", new List<FaceOffCardEffect>{
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_PLAY_THIS_CARD,
                    FaceOffCardEffectEffect.SWAY_COUNTERS,
                    FaceOffCardEffectTarget.SELF)
                    .withEffectAmount(3),
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_PLAY_THIS_CARD,
                    FaceOffCardEffectEffect.CYCLE,
                    FaceOffCardEffectTarget.NONE)
                    .withEffectAmount(3)
                }
            },
            { "weight-loss-seeker", new List<FaceOffCardEffect>{
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_PLAY_THIS_CARD,
                    FaceOffCardEffectEffect.CYCLE,
                    FaceOffCardEffectTarget.NONE)
                    .withEffectAmount(1)
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

    public static List<FaceOffCardEffect> getCardEffectsFromCrewCard(CrewCard crewCard)
    {
        string dictionaryKey = crewCard.cardName.ToLower().Replace(" ", "-");
        if (effectDictionary.ContainsKey(dictionaryKey))
        {
            return effectDictionary[dictionaryKey];
        }
        return new List<FaceOffCardEffect>();
    }

    public static List<FaceOffCardEffect> getCardEffectsFromCrewCard(CrewCard crewCard, FaceOffCard card)
    {
        string dictionaryKey = crewCard.cardName.ToLower().Replace(" ", "-");
        if (effectDictionary.ContainsKey(dictionaryKey))
        {
            List<FaceOffCardEffect> ownedCardEffects = new List<FaceOffCardEffect>();
            foreach(FaceOffCardEffect effect in effectDictionary[dictionaryKey])
            {
                ownedCardEffects.Add(effect.clone().withEffectOwner(card));
            }
            return ownedCardEffects;
        }
        return new List<FaceOffCardEffect>();
    }
}
