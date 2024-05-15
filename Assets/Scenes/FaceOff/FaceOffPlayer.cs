using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class FaceOffPlayer
{
    private List<GameObject> deck;
    private List<GameObject> hand;
    private List<GameObject> team;
    private List<GameObject> discard;
    private int startingHandSize = 6;
    private System.Random random = new System.Random();

    private float teamY;
    private float handY;

    public FaceOffPlayer(List<GameObject> deck, FaceOffPlayerPosition position)
    {
        this.deck = deck;
        this.hand = new List<GameObject>();
        this.team = new List<GameObject>();
        this.discard = new List<GameObject>();

        this.teamY = position == FaceOffPlayerPosition.Top ? 1.5f : -1.5f;
        this.handY = position == FaceOffPlayerPosition.Top ? 4.3f : -4.3f;
    }

    public List<GameObject> getDeck()
    {
        return this.deck;
    }

    public List<GameObject> getHand()
    {
        return this.hand;
    }

    public List<GameObject> getTeam()
    {
        return this.team;
    }

    public List<GameObject> getDiscard()
    {
        return this.discard;
    }

    public int deckSize()
    {
        return this.deck.Count;
    }

    public int maxHandSize()
    {
        return this.startingHandSize;
    }

    public void shuffleDeck()
    {
        this.deck = this.deck.OrderBy(x => random.Next()).ToList();
    }

    public void drawXCards(int numberCardsToDraw)
    {
        int actualNumberOfCardsToDraw = numberCardsToDraw <= this.deckSize() ? numberCardsToDraw : this.deckSize();

        for (int cardNumber = 0; cardNumber < actualNumberOfCardsToDraw; cardNumber++)
        {
            this.hand.Add(this.deck[0]);
            this.deck.RemoveAt(0);
        }
    }

    public void playCardFromHand(GameObject cardToPlay)
    {
        if (this.hand.Contains(cardToPlay))
        {
            this.hand.RemoveAt(this.hand.IndexOf(cardToPlay));
            this.team.Add(cardToPlay);
            this.repositionTeam();
            this.repositionHand();
        }
    }

    public void repositionCards()
    {
        this.repositionHand();
        this.repositionTeam();
    }

    private void repositionTeam()
    {
        int cardIndex = 0;
        foreach(GameObject teamCard in this.team)
        {

            float xCoordinate = (float)-this.team.Count + 1f + (cardIndex * 2f);
            teamCard.GetComponent<Transform>().position = new Vector3(xCoordinate, teamY);
            teamCard.GetComponent<SortingGroup>().sortingLayerName = "Cards";
            cardIndex++;
        }
    }

    private void repositionHand()
    {
        int cardNumber = 0;
        foreach (GameObject handCard in this.hand)
        {
            float xCoordinate = (float)-this.hand.Count + 2f + (cardNumber * 1.5f);
            handCard.GetComponent<Transform>().position = new Vector3(xCoordinate, handY);
            handCard.GetComponent<SortingGroup>().sortingLayerName = "Over Cards";
            handCard.GetComponent<SortingGroup>().sortingOrder = -cardNumber;
            cardNumber++;
        }
    }
}
