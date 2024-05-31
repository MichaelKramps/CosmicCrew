using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;

public class FaceOffCard
{
    private GameObject cardsGameObject;
    private CrewCard crewCard;
    public int powerCounters = 0;
    private FaceOffPlayer cardOwner;
    //private FaceOffPlayerPosition cardOwner;

    private List<FaceOffCardEffect> cardEffects;
    private List<FaceOffCard> attachedGear = new List<FaceOffCard>();

    private float teamY;
    private float handY;
    private float deckDiscardY;
    private float duelY;

    private DuelResult duelResult;

    public FaceOffCard(GameObject cardsGameObject, FaceOffPlayerPosition cardOwner)
    {
        this.cardsGameObject = cardsGameObject;
        this.crewCard = cardsGameObject.GetComponent<CrewCardScript>().crewCard;
        this.cardEffects = CardFinder.getCardEffectsFromCrewCard(this.crewCard, this);
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

    public void addCardOwner(FaceOffPlayer player)
    {
        this.cardOwner = player;
    }

    public FaceOffPlayer getCardOwner()
    {
        return this.cardOwner;
    }

    public List<FaceOffCard> getAttachedGear()
    {
        return this.attachedGear;
    }

    public void removeGear()
    {
        this.attachedGear = new List<FaceOffCard>();
    }

    public void attachGear(FaceOffCard gearCardToAttach)
    {
        this.attachedGear.Add(gearCardToAttach);
    }

    public void resetCard()
    {
        this.removeGear();
        this.powerCounters = 0;

        //reset power visually
        GameObject powerCountersObject = cardsGameObject.transform.Find("Power Counters").gameObject;
        powerCountersObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
        GameObject numberCountersObject = powerCountersObject.transform.Find("Number Counters").gameObject;
        numberCountersObject.GetComponent<SortingGroup>().sortingOrder = 0;
        int numberPowerCounters = Int32.Parse(numberCountersObject.GetComponent<TextMeshPro>().text);
        numberCountersObject.GetComponent<TextMeshPro>().text = "0";
        GameObject powerObject = cardsGameObject.transform.Find("Power").gameObject;
        int currentPower = Int32.Parse(powerObject.GetComponent<TextMeshPro>().text);
        int newPower = currentPower - numberPowerCounters;
        powerObject.GetComponent<TextMeshPro>().text = newPower.ToString();
        powerObject.GetComponent<TextMeshPro>().fontStyle = FontStyles.Normal;
        powerObject.GetComponent<TextMeshPro>().color = Color.black;
    }

    public void highlight()
    {
        this.cardsGameObject.transform.Find("Highlight").GetComponent<Renderer>().enabled = true;

    }

    public void unHighlight()
    {
        this.cardsGameObject.transform.Find("Highlight").GetComponent<Renderer>().enabled = false;
    }

    public void repositionCardInTeam(int teamCount, int positionInTeam)
    {
        this.unHighlight();
        float xCoordinate = (float)-teamCount + 1f + (positionInTeam * 2f);
        this.cardsGameObject.GetComponent<CrewCardBehavior>().moveTo(new Vector3(xCoordinate, teamY), Animate.cardMovementTime);
        this.cardsGameObject.GetComponent<SortingGroup>().sortingLayerName = "Cards";
        if (this.attachedGear.Count > 0)
        {
            this.repositionGearOnCard(xCoordinate);
        }
    }

    public void repositionGearOnCard(float futureXCoordinateOfThisCard)
    {
        float yCoordinate = this.cardsGameObject.GetComponent<Transform>().position.y;
        int index = 1;
        foreach(FaceOffCard gear in this.attachedGear)
        {
            yCoordinate += (index * -0.4f) - 0.4f;
            gear.getCardsGameObject().GetComponent<CrewCardBehavior>().moveTo(new Vector3(futureXCoordinateOfThisCard, yCoordinate), Animate.cardMovementTime);
            gear.getCardsGameObject().GetComponent<SortingGroup>().sortingLayerName = "Cards";
            gear.getCardsGameObject().GetComponent<SortingGroup>().sortingOrder = index;
            index++;
        }
    }

    public void repositionCardInHand(int handCount, int positionInHand)
    {
        this.unHighlight();
        float xCoordinate = (float)-handCount + 2f + (positionInHand * 1.5f);
        this.cardsGameObject.GetComponent<CrewCardBehavior>().moveTo(new Vector3(xCoordinate, handY), Animate.cardMovementTime);
        this.cardsGameObject.GetComponent<SortingGroup>().sortingLayerName = "Over Cards";
        this.cardsGameObject.GetComponent<SortingGroup>().sortingOrder = -positionInHand;
    }

    public void repositionCardInDiscard(int positionInDiscard)
    {
        this.unHighlight();
        float xCoordinate = -9f;
        this.cardsGameObject.GetComponent<CrewCardBehavior>().moveTo(new Vector3(xCoordinate, deckDiscardY), Animate.cardMovementTime);
        this.cardsGameObject.GetComponent<SortingGroup>().sortingLayerName = "Cards";
        this.cardsGameObject.GetComponent<SortingGroup>().sortingOrder = positionInDiscard;
    }

    public void repositionCardInDeck(int positionInDeck)
    {
        this.unHighlight();
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
        return this.crewCard.power + this.powerCounters;
    }

    public void activateEffectsFor(FaceOffCardEffectTiming timing)
    {
        foreach (FaceOffCardEffect effect in this.cardEffects)
        {
            if (effect.timingIs(timing))
            {
                effect.activateEffect();
            }
        }
    }

    public FandomType getFandomType()
    {
        return this.crewCard.fandomType;
    }

    public String getCardName()
    {
        return this.crewCard.cardName;
    }

    public void addSwayCounters(int numberSwayCounters)
    {
        this.powerCounters += numberSwayCounters;
        this.cardsGameObject.GetComponent<CrewCardBehavior>().addSwayCounters(this.powerCounters);
    }

    public bool isFanatic()
    {
        return this.crewCard.cardType == CardType.FANATIC;
    }

    public bool isGear()
    {
        return this.crewCard.cardType == CardType.GEAR || this.crewCard.cardType == CardType.ETERNAL_GEAR;
    }
}
