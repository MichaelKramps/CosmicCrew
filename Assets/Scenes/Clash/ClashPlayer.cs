using System;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class ClashPlayer
{
    List<int> startingDeckIds = new List<int>();

    List<GameObject> deck = new List<GameObject>();
    List<GameObject> team = new List<GameObject> { null, null, null, null, null, null };
    List<GameObject> discard = new List<GameObject>();

    private GameObject activeCard = null;
    private GameObject currentFighter = null;

    public ClashPlayer(String deckString)
    {
        string[] allIdStrings = deckString.Split(",");
        foreach (string idString in allIdStrings)
        {
            try
            {
                startingDeckIds.Add(Int32.Parse(idString));
            }
            catch
            {
                startingDeckIds.Add(0);
            }
        }
    }

    public List<int> getStartingDeckIds()
    {
        return this.startingDeckIds;
    }

    public void addToDeck(GameObject cardToAdd)
    {
        deck.Add(cardToAdd);
    }

    public int numberOfCardsInDeck()
    {
        return deck.Count;
    }

    public void addToDiscard(GameObject cardToAdd)
    {
        discard.Add(cardToAdd);
    }

    public int numberOfCardsInDiscard()
    {
        return discard.Count;
    }

    public void selectFighter(int slotOfFighter)
    {

        activeCard = fighterFromRoll(slotOfFighter);
        currentFighter = fighterFromRoll(slotOfFighter);
        team[fighterNumberFromRoll(slotOfFighter) - 1] = null;
    }

    public void winsFight()
    {
        addToDeck(currentFighter);
        activeCard = null;
    }

    public void losesFight()
    {
        addToDiscard(currentFighter);
        activeCard = null;
    }

    public void destroyFighter(int slotOfFighter)
    {
        GameObject fighterToDestroy = fighterInSlot(slotOfFighter);
        team[slotOfFighter - 1] = null;
        addToDiscard(fighterToDestroy);
    }

    public GameObject getTopCardInDeck()
    {
        return deck[0];
    }

    public GameObject getCurrentFighter()
    {
        return currentFighter;
    }

    public GameObject fighterFromDiscard(int indexOfFighter)
    {
        Debug.Log(indexOfFighter);
        return discard[indexOfFighter];
    }

    public void playFromDiscard(int teamSlot, int indexInDiscard)
    {
        GameObject newFighter = discard[indexInDiscard];
        discard.RemoveAt(indexInDiscard);
        team[teamSlot - 1] = newFighter;
    }

    public void drawCard()
    {
        activeCard = this.getTopCardInDeck();
        activeCard.GetComponent<SortingGroup>().sortingOrder = 0;
        deck.Remove(this.getTopCardInDeck());
    }

    public GameObject getActiveCard()
    {
        return activeCard;
    }

    public void putCardOnBottomOfDeck()
    {
        deck.Add(activeCard);
        activeCard = null;
    }

    public void playCard(int slotToPlayCardIn)
    {
        team[slotToPlayCardIn - 1] = activeCard;
        activeCard = null;
    }

    public GameObject removeFighterInSlot(int cardSlot)
    {
        GameObject cardToRemove = team[cardSlot - 1];
        team[cardSlot - 1] = null;
        return cardToRemove;
    }

    public GameObject fighterInSlot(int slotNumber)
    {
        return team[slotNumber - 1];
    }

    public GameObject fighterFromRoll(int rolledNumber)
    {
        return team[fighterNumberFromRoll(rolledNumber) - 1];
    }

    public int fighterNumberFromRoll(int rolledNumber)
    {
        if (team[rolledNumber - 1] != null)
        {
            return rolledNumber;
        }
        else
        {
            if (rolledNumber == 6)
            {
                return fighterNumberFromRoll(1);
            }
            else
            {
                return fighterNumberFromRoll(rolledNumber + 1);
            }
        }
    }
}
