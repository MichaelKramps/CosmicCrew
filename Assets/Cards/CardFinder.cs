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
            { 12, "domestique" },
            { 13, "points-leader" },
            { 14, "newbie-cyclist" },
            { 15, "ex-pro-biker" },
            { 16, "low-gear-guy" },
            { 17, "boom-box-player" },
            { 18, "no-hands-gal" },
            { 19, "squeaky-gears" },
            { 20, "safety-fanatic" },
            { 21, "electric-bike-user" },
            { 22, "bike-repair-kit" },
            { 23, "state-of-the-art-bike" },
            { 24, "bodacious-bibs" },
            { 25, "hair-protecting-helmet" },
            { 26, "bike-collector" },
            { 27, "unwanted-coach" },
            //{ 28, "modern-bohemian" },
            //{ 29, "sherlock-holmes" },
        };
    private static Dictionary<String, List<FaceOffCardEffect>> effectDictionary = new Dictionary<string, List<FaceOffCardEffect>>
        {
            { "hostile-dog", new List<FaceOffCardEffect>{
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_DISMISS_THIS_CARD,
                    FaceOffCardEffectEffect.REDUCE_COST,
                    FaceOffCardEffectTarget.RECRUITING_HAND)
                    .withConstantEffectAmount(99)
                }
            },
            { "rainbow-jersey", new List<FaceOffCardEffect>{
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_GIVE_THIS_TO_A_FANATIC,
                    FaceOffCardEffectEffect.SWAY_COUNTERS,
                    FaceOffCardEffectTarget.ENTIRE_TEAM)
                    .withConstantEffectAmount(2)
                    .withFandomFilter(FandomType.CYCLING)
                }
            },
            { "king-of-the-mountains", new List<FaceOffCardEffect>{
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_DRAW_A_CARD,
                    FaceOffCardEffectEffect.SWAY_COUNTERS,
                    FaceOffCardEffectTarget.SELF)
                    .withConstantEffectAmount(1)
                }
            },
            { "lady-cyclist", new List<FaceOffCardEffect>{
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_GEAR_IS_ATTACHED_TO_THIS_CARD,
                    FaceOffCardEffectEffect.SWAY_COUNTERS,
                    FaceOffCardEffectTarget.SELF)
                    .withConstantEffectAmount(2)
                }
            },
            { "expensive-wheels", new List<FaceOffCardEffect>{
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_DRAW_THIS_CARD,
                    FaceOffCardEffectEffect.SWAY_COUNTERS,
                    FaceOffCardEffectTarget.ENTIRE_TEAM)
                    .withConstantEffectAmount(1)
                }
            },
            { "well-fitted-seat", new List<FaceOffCardEffect>{
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_DRAW_THIS_CARD,
                    FaceOffCardEffectEffect.SWAY_COUNTERS,
                    FaceOffCardEffectTarget.RANDOM_FANATIC)
                    .withConstantEffectAmount(2)
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
                    .withConstantEffectAmount(3),
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_PLAY_THIS_CARD,
                    FaceOffCardEffectEffect.CYCLE,
                    FaceOffCardEffectTarget.NONE)
                    .withConstantEffectAmount(3)
                }
            },
            { "weight-loss-seeker", new List<FaceOffCardEffect>{
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_PLAY_THIS_CARD,
                    FaceOffCardEffectEffect.CYCLE,
                    FaceOffCardEffectTarget.NONE)
                    .withConstantEffectAmount(1)
                }
            },
            { "domestique", new List<FaceOffCardEffect>{
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_OPPONENT_DRAWS_A_CARD,
                    FaceOffCardEffectEffect.SWAY_COUNTERS,
                    FaceOffCardEffectTarget.LEFTMOST_FANATIC)
                    .withConstantEffectAmount(1),
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_OPPONENT_DRAWS_A_CARD,
                    FaceOffCardEffectEffect.SWAY_COUNTERS,
                    FaceOffCardEffectTarget.RIGHTMOST_FANATIC)
                    .withConstantEffectAmount(1)
                }
            },
            { "points-leader", new List<FaceOffCardEffect>{
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.AFTER_EACH_DUEL,
                    FaceOffCardEffectEffect.CYCLE,
                    FaceOffCardEffectTarget.NONE)
                    .withConstantEffectAmount(1)
                }
            },
            { "newbie-cyclist", new List<FaceOffCardEffect>{
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_PLAY_THIS_CARD,
                    FaceOffCardEffectEffect.SWAY_COUNTERS,
                    FaceOffCardEffectTarget.SELF)
                    .withConstantEffectAmount(1),
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_PLAY_THIS_CARD,
                    FaceOffCardEffectEffect.CYCLE,
                    FaceOffCardEffectTarget.NONE)
                    .withConstantEffectAmount(1)
                }
            },
            { "ex-pro-biker", new List<FaceOffCardEffect>{
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_RECRUIT_THIS_CARD,
                    FaceOffCardEffectEffect.REFRESH,
                    FaceOffCardEffectTarget.NONE)
                }
            },
            { "low-gear-guy", new List<FaceOffCardEffect>{
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_PLAY_THIS_CARD,
                    FaceOffCardEffectEffect.CYCLE,
                    FaceOffCardEffectTarget.NONE)
                    .withConstantEffectAmount(5)
                }
            },
            { "boom-box-player", new List<FaceOffCardEffect>{
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_DRAW_A_CARD,
                    FaceOffCardEffectEffect.SWAY_COUNTERS,
                    FaceOffCardEffectTarget.RIGHTMOST_FANATIC)
                    .withConstantEffectAmount(1)
                }
            },
            { "no-hands-gal", new List<FaceOffCardEffect>{
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_DRAW_A_CARD,
                    FaceOffCardEffectEffect.SWAY_COUNTERS,
                    FaceOffCardEffectTarget.LEFTMOST_FANATIC)
                    .withConstantEffectAmount(1)
                }
            },
            { "squeaky-gears", new List<FaceOffCardEffect>{
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_GIVE_THIS_TO_A_FANATIC,
                    FaceOffCardEffectEffect.DRAW_CARDS,
                    FaceOffCardEffectTarget.NONE)
                    .withConstantEffectAmount(1),
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_DISMISS_THIS_CARD,
                    FaceOffCardEffectEffect.GAIN_RECRUITING_TOKENS,
                    FaceOffCardEffectTarget.NONE)
                    .withConstantEffectAmount(1)
                }
            },
            { "safety-fanatic", new List<FaceOffCardEffect>{
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_PLAY_THIS_CARD,
                    FaceOffCardEffectEffect.SWAY_COUNTERS,
                    FaceOffCardEffectTarget.SELF)
                    .withVariableEffectAmount(VariableEffectAmount.NUMBER_CARDS_IN_DISCARD)
                }
            },
            { "electric-bike-user", new List<FaceOffCardEffect>{
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_PLAY_THIS_CARD,
                    FaceOffCardEffectEffect.SWAY_COUNTERS,
                    FaceOffCardEffectTarget.SELF)
                    .withVariableEffectAmount(VariableEffectAmount.SWAY_OF_TOP_CARD_IN_DECK),
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_PLAY_THIS_CARD,
                    FaceOffCardEffectEffect.CYCLE,
                    FaceOffCardEffectTarget.NONE)
                    .withConstantEffectAmount(1)
                }
            },
            { "bike-repair-kit", new List<FaceOffCardEffect>{
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_GIVE_THIS_TO_A_FANATIC,
                    FaceOffCardEffectEffect.MOVE_CARDS,
                    FaceOffCardEffectTarget.DISCARD)
                    .withSecondTarget(FaceOffCardEffectTarget.DECK)
                    .withFandomFilter(FandomType.CYCLING)
                    .withCardTypeFilter(CardType.GEAR)
                }
            },
            { "state-of-the-art-bike", new List<FaceOffCardEffect>{
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_DRAW_THIS_CARD,
                    FaceOffCardEffectEffect.SWAY_COUNTERS,
                    FaceOffCardEffectTarget.RIGHTMOST_FANATIC)
                    .withConstantEffectAmount(3)
                }
            },
            { "bodacious-bibs", new List<FaceOffCardEffect>{
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_GIVE_THIS_TO_A_FANATIC,
                    FaceOffCardEffectEffect.CYCLE,
                    FaceOffCardEffectTarget.NONE)
                    .withConstantEffectAmount(6)
                }
            },
            { "hair-protecting-helmet", new List<FaceOffCardEffect>{
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_DRAW_THIS_CARD,
                    FaceOffCardEffectEffect.DISMISS,
                    FaceOffCardEffectTarget.SELF),
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_RECRUIT_THIS_CARD,
                    FaceOffCardEffectEffect.DISMISS,
                    FaceOffCardEffectTarget.NONE)
                }
            },
            { "bike-collector", new List<FaceOffCardEffect>{
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_YOU_PLAY_THIS_CARD,
                    FaceOffCardEffectEffect.SWAY_COUNTERS,
                    FaceOffCardEffectTarget.SELF)
                    .withVariableEffectAmount(VariableEffectAmount.NUMBER_CARDS_IN_DECK)
                }
            },
            { "unwanted-coach", new List<FaceOffCardEffect>{
                new FaceOffCardEffect(
                    FaceOffCardEffectTiming.WHEN_ONE_OF_YOUR_FANATICS_WINS_A_DUEL,
                    FaceOffCardEffectEffect.SWAY_COUNTERS,
                    FaceOffCardEffectTarget.SELF)
                    .withConstantEffectAmount(1)
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
