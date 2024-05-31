using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class FaceOffPlayer
{
    private List<FaceOffCard> deck;
    private List<FaceOffCard> hand;
    private List<FaceOffCard> team;
    private List<FaceOffCard> discard;
    private int startingHandSize = 6;
    private System.Random random = new System.Random();

    private FaceOffPlayer opponent;
    private FaceOffCard selectedGearCard;

    public FaceOffPlayer(List<FaceOffCard> deck, FaceOffPlayerPosition position)
    {
        this.deck = deck;
        this.hand = new List<FaceOffCard>();
        this.team = new List<FaceOffCard>();
        this.discard = new List<FaceOffCard>();
    }

    public void addPlayerToCards()
    {
        foreach(FaceOffCard card in deck)
        {
            card.addCardOwner(this);
        }
    }

    public void addOpponent(FaceOffPlayer opponent)
    {
        this.opponent = opponent;
    }

    public FaceOffPlayer getOpponent()
    {
        return this.opponent;
    }

    public List<FaceOffCard> getDeck()
    {
        return this.deck;
    }

    public List<FaceOffCard> getHand()
    {
        return this.hand;
    }

    public List<FaceOffCard> getTeam()
    {
        return this.team;
    }

    public List<FaceOffCard> getDiscard()
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
            foreach(FaceOffCard card in this.team)
            {
                card.activateEffectsFor(FaceOffCardEffectTiming.WHEN_YOU_DRAW_A_CARD, this);
            }
            this.hand.Add(this.deck[0]);
            this.deck.RemoveAt(0);
        }
    }

    public void playRandomCardFromHand()
    {
        this.playCardFromHand(this.hand[random.Next(this.hand.Count)]);
    }

    public void playCardFromHand(FaceOffCard cardToPlay)
    {
        if (this.hand.Contains(cardToPlay))
        {
            this.team.Add(cardToPlay);
            this.hand.Remove(cardToPlay);
            this.repositionTeam();
            this.repositionHand();
        }
    }

    public void playGameObjectCardFromHand(GameObject cardToPlay)
    {
        int indexOfCardToPlay = this.findIndexOfGameObjectInHand(cardToPlay);
        if (indexOfCardToPlay > -1)
        {
            this.team.Add(this.hand[indexOfCardToPlay]);
            this.hand.RemoveAt(indexOfCardToPlay);
            this.repositionTeam();
            this.repositionHand();
        }
    }

    public void selectGameObjectGearCardFromHand(GameObject gearCardToSelect)
    {
        int indexOfGearCard = this.findIndexOfGameObjectInHand(gearCardToSelect);
        if (indexOfGearCard > -1)
        {
            this.selectedGearCard = this.hand[indexOfGearCard];
            this.highlightTeam();
        }
    }

    public void selectFanaticForGearAttachment(GameObject fanaticSelected)
    {
        int indexOfFanaticSelected = this.findIndexOfGameObjectInTeam(fanaticSelected);
        if (indexOfFanaticSelected > -1)
        {
            FaceOffCard fanaticCardSelected = this.team[indexOfFanaticSelected];
            fanaticCardSelected.attachGear(this.selectedGearCard);
            this.hand.Remove(this.selectedGearCard);

            fanaticCardSelected.activateEffectsFor(FaceOffCardEffectTiming.WHEN_GEAR_IS_ATTACHED_TO_THIS_CARD, this);
            this.selectedGearCard.activateEffectsFor(FaceOffCardEffectTiming.WHEN_YOU_GIVE_THIS_TO_A_FANATIC, this);

            this.repositionTeam();
            this.repositionHand();
        }
    }

    public int findIndexOfGameObjectInHand(GameObject cardGameObjectToFind)
    {
        foreach(FaceOffCard card in this.hand)
        {
            if (card.getCardsGameObject() == cardGameObjectToFind)
            {
                return this.hand.IndexOf(card);
            }
        }
        return -1;
    }

    public int findIndexOfGameObjectInTeam(GameObject cardGameObjectToFind)
    {
        foreach (FaceOffCard card in this.team)
        {
            if (card.getCardsGameObject() == cardGameObjectToFind)
            {
                return this.team.IndexOf(card);
            }
        }
        return -1;
    }

    public FaceOffCard selectRandomFanaticFromTeam()
    {
        Debug.Assert(this.team.Count > 0);
        return this.team[random.Next(this.team.Count)];
    }

    public void repositionCards()
    {
        this.repositionHand();
        this.repositionTeam();
        this.repositionDiscard();
        this.repositionDeck();
    }

    private void repositionTeam()
    {
        int positionInTeam = 0;
        foreach(FaceOffCard teamCard in this.team)
        {
            teamCard.repositionCardInTeam(this.team.Count, positionInTeam);
            positionInTeam++;
        }
    }

    private void highlightTeam()
    {
        foreach (FaceOffCard teamCard in this.team)
        {
            teamCard.highlight();
        }
    }

    private void repositionHand()
    {
        int positionInHand = 0;
        foreach (FaceOffCard handCard in this.hand)
        {
            handCard.repositionCardInHand(this.hand.Count, positionInHand);
            positionInHand++;
        }
    }

    private void repositionDiscard()
    {
        int positionInDiscard = 0;
        foreach (FaceOffCard discardCard in this.discard)
        {
            discardCard.repositionCardInDiscard(positionInDiscard);
            positionInDiscard++;
        }
    }

    private void repositionDeck()
    {
        int positionInDeck = 0;
        foreach (FaceOffCard deckCard in this.deck)
        {
            deckCard.repositionCardInDeck(positionInDeck);
            positionInDeck++;
        }
    }

    public void handleDuelResult(FaceOffCard postDuelCard)
    {
        switch (postDuelCard.getDuelResult())
        {
            case DuelResult.Won:
                handleDuelWin(postDuelCard);
                break;
            case DuelResult.Lost:
            case DuelResult.Tied:
                handleDuelLoss(postDuelCard);
                break;
            default:
                break;
        }

        postDuelCard.setDuelResult(DuelResult.None);
    }

    public void handleDuelWin(FaceOffCard postDuelCard)
    {
        //remove card from team
        this.team.Remove(postDuelCard);

        //draw a card from deck before putting cards in
        this.drawXCards(1);

        //put card on bottom of deck
        this.deck.Add(postDuelCard);

        //put attached gear on bottom of deck
        foreach(FaceOffCard gearCard in postDuelCard.getAttachedGear())
        {
            this.deck.Add(gearCard);
        }
        postDuelCard.resetCard();
    }

    public void handleDuelLoss(FaceOffCard postDuelCard)
    {
        //put card on bottom of discard
        this.team.Remove(postDuelCard);
        this.discard.Add(postDuelCard);

        //put attached gear on bottom of discard
        foreach (FaceOffCard gearCard in postDuelCard.getAttachedGear())
        {
            this.discard.Add(gearCard);
        }
        postDuelCard.resetCard();
    }
}
