using System;
using UnityEngine;

public class ClashConstants
{
    public static float defaultCardScale = 0.45f;

    static float cardXDistanceApart = 2.3f;
    static float cardXStartingPoint = -5.75f;

    public static float player1CardY = -2.25f;
    public static float player2CardY = 2.25f;

    public static float player1DeckY = -3f;
    public static float player2DeckY = 3f;
    public static float deckX = cardXStartingPoint + (cardXDistanceApart * 6) + 0.35f;
    public static float discardX = cardXStartingPoint - cardXDistanceApart - 0.35f;

    public static float viewCardX = deckX - 1.5f;
    public static float viewCardScaleSize = 0.6f;
    public static float fightCardX = 0f;
    public static float fightCardY1 = -2.5f;
    public static float fightCardY2 = 2.5f;
    public static float fightCardScale = 0.6f;
    public static float viewCardAnimationTime = 750f;

    public static Vector3 player1Deck = new Vector3(ClashConstants.deckX, ClashConstants.player1DeckY);
    public static Vector3 player2Deck = new Vector3(ClashConstants.deckX, ClashConstants.player2DeckY);
    public static Vector3 player1Discard = new Vector3(ClashConstants.discardX, ClashConstants.player1DeckY);
    public static Vector3 player2Discard = new Vector3(ClashConstants.discardX, ClashConstants.player2DeckY);
    public static Vector3 player1ViewCard = new Vector3(ClashConstants.viewCardX, ClashConstants.player1CardY);
    public static Vector3 player2ViewCard = new Vector3(ClashConstants.viewCardX, ClashConstants.player2CardY);
    public static Vector3 player1FightDestination = new Vector3(ClashConstants.fightCardX, ClashConstants.fightCardY1);
    public static Vector3 player2FightDestination = new Vector3(ClashConstants.fightCardX, ClashConstants.fightCardY2);

    public static float getPlayCardX(int cardSlot)
    {
        return cardXStartingPoint + (cardXDistanceApart * (cardSlot - 1));
    }

    public static Vector3 getPlayer1CardLocation(int cardSlot)
    {
        return new Vector3(ClashConstants.getPlayCardX(cardSlot), ClashConstants.player1CardY);
    }

    public static Vector3 getPlayer2CardLocation(int cardSlot)
    {
        return new Vector3(ClashConstants.getPlayCardX(cardSlot), ClashConstants.player2CardY);
    }

    public static float dieX = 0f;
    public static float player1DieY = -2f;
    public static float player2DieY = 2f;

    public static float powerCounterStart = 1f * ClashConstants.defaultCardScale;
    public static float powerCounterEnd = -2.25f * ClashConstants.defaultCardScale;
    public static float powerCounterAnimationTime = 750f;

    public static Vector3 getPowerCounterStartingPoint(GameObject activeCard)
    {
        return new Vector3(activeCard.transform.position.x, activeCard.transform.position.y + ClashConstants.powerCounterStart);
    }

    public static Vector3 getPowerCounterEndingPoint(GameObject activeCard)
    {
        return new Vector3(activeCard.transform.position.x, activeCard.transform.position.y + ClashConstants.powerCounterEnd);
    }
}
