using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FaceOffCard
{
    private GameObject cardsGameObject;
    private CrewCard crewCard;
    private FaceOffPlayerPosition cardOwner;

    private List<FaceOffCardEffect> cardEffects;

    private float teamY;
    private float handY;
    private float deckDiscardY;
    private float duelY;

    private DuelResult duelResult;

    public FaceOffCard(GameObject cardsGameObject, FaceOffPlayerPosition cardOwner)
    {
        this.cardsGameObject = cardsGameObject;
        this.crewCard = cardsGameObject.GetComponent<CrewCardScript>().crewCard;
        this.cardEffects = CardFinder.getCardEffectsFromName(this.crewCard.cardName);
        this.teamY = cardOwner == FaceOffPlayerPosition.Top ? 1.5f : -1.5f;
        this.handY = cardOwner == FaceOffPlayerPosition.Top ? 4.3f : -4.3f;
        this.deckDiscardY = cardOwner == FaceOffPlayerPosition.Top ? 3f : -3f;
        this.duelY = cardOwner == FaceOffPlayerPosition.Top ? this.teamY - 0.4f : this.teamY + 0.4f;
        this.duelResult = DuelResult.None;
    }

    public GameObject getCardsGameObject()
    {
        return this.cardsGameObject;
    }

    public void repositionCardInTeam(int teamCount, int positionInTeam)
    {
        float xCoordinate = (float)-teamCount + 1f + (positionInTeam * 2f);
        this.cardsGameObject.GetComponent<CrewCardBehavior>().moveTo(new Vector3(xCoordinate, teamY), Animate.cardMovementTime);
        this.cardsGameObject.GetComponent<SortingGroup>().sortingLayerName = "Cards";
    }

    public void repositionCardInHand(int handCount, int positionInHand)
    {
        float xCoordinate = (float)-handCount + 2f + (positionInHand * 1.5f);
        this.cardsGameObject.GetComponent<CrewCardBehavior>().moveTo(new Vector3(xCoordinate, handY), Animate.cardMovementTime);
        this.cardsGameObject.GetComponent<SortingGroup>().sortingLayerName = "Over Cards";
        this.cardsGameObject.GetComponent<SortingGroup>().sortingOrder = -positionInHand;
    }

    public void repositionCardInDiscard(int positionInDiscard)
    {
        float xCoordinate = -9f;
        this.cardsGameObject.GetComponent<CrewCardBehavior>().moveTo(new Vector3(xCoordinate, deckDiscardY), Animate.cardMovementTime);
        this.cardsGameObject.GetComponent<SortingGroup>().sortingLayerName = "Cards";
        this.cardsGameObject.GetComponent<SortingGroup>().sortingOrder = positionInDiscard;
    }

    public void repositionCardInDeck(int positionInDeck)
    {
        float xCoordinate = 9f;
        this.cardsGameObject.GetComponent<CrewCardBehavior>().moveTo(new Vector3(xCoordinate, deckDiscardY), Animate.cardMovementTime);
        this.cardsGameObject.GetComponent<SortingGroup>().sortingLayerName = "Cards";
        this.cardsGameObject.GetComponent<SortingGroup>().sortingOrder = -positionInDeck;
    }

    public void selectForDuel()
    {
        float xCoordinate = this.cardsGameObject.GetComponent<Transform>().position.x;
        this.cardsGameObject.GetComponent<Transform>().position = new Vector3(xCoordinate, duelY);
    }

    public void setDuelResult(DuelResult duelResult)
    {
        this.duelResult = duelResult;
    }

    public DuelResult getDuelResult()
    {
        return this.duelResult;
    }

    public int getTotalPower()
    {
        return this.crewCard.power + this.crewCard.powerCounters;
    }

    public void activateEffectsFor(FaceOffCardEffectTiming timing)
    {
        foreach(FaceOffCardEffect effect in cardEffects)
        {
            if (effect.timingIs(timing))
            {
                effect.activateEffect();
            }
        }
    }
}
