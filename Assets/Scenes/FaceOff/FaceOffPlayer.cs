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

    public int discardSize()
    {
        return this.discard.Count;
    }

    public int maxHandSize()
    {
        return this.startingHandSize;
    }

    public void shuffleDeck()
    {
        this.deck = this.deck.OrderBy(x => random.Next()).ToList();
    }

    public void handleTeamEffectsFor(FaceOffCardEffectTiming timing)
    {
        foreach (FaceOffCard teamMember in this.team)
        {
            teamMember.activateEffectsFor(timing);
        }
    }

    public void handleOpposingTeamEffectsFor(FaceOffCardEffectTiming timing)
    {
        foreach (FaceOffCard opposingTeamMember in this.opponent.team)
        {
            opposingTeamMember.activateEffectsFor(timing);
        }
    }

    public FaceOffCard drawACard()
    {
        FaceOffCard cardDrawn = this.deck[0];

        this.handleTeamEffectsFor(FaceOffCardEffectTiming.WHEN_YOU_DRAW_A_CARD);
        this.handleOpposingTeamEffectsFor(FaceOffCardEffectTiming.WHEN_OPPONENT_DRAWS_A_CARD);
        cardDrawn.activateEffectsFor(FaceOffCardEffectTiming.WHEN_YOU_DRAW_THIS_CARD);

        if (this.deck.Contains(cardDrawn))
        {
            this.deck.Remove(cardDrawn);
            return cardDrawn;
        }
        return null;
    }

    public void putCardInHand(FaceOffCard card)
    {
        this.hand.Add(card);
    }

    public void cycleCard(FaceOffCard card)
    {
        card.activateEffectsFor(FaceOffCardEffectTiming.WHEN_YOU_CYCLE_THIS_CARD);
        if (!this.team.Contains(card) && !this.hand.Contains(card))
        {
            card.getCardsGameObject().GetComponent<CrewCardBehavior>().cycleCard();
            this.deck.Add(card);
        }
    }

    public void drawXCardsIntoHand(int numberCardsToDraw)
    {
        int actualNumberOfCardsToDraw = numberCardsToDraw <= this.deckSize() ? numberCardsToDraw : this.deckSize();

        for (int cardNumber = 0; cardNumber < actualNumberOfCardsToDraw; cardNumber++)
        {
            if (this.deck.Count > 0)
            {
                FaceOffCard drawnCard = drawACard();
                if (drawnCard != null)
                {
                    putCardInHand(drawnCard);
                }
            }
        }
    }
    

    public void cycleXCards(int numberCardsToCycle)
    {
        int actualNumberOfCardsToCycle = this.deckSize() > 0 ? numberCardsToCycle : 0;

        for (int cardNumber = 0; cardNumber < actualNumberOfCardsToCycle; cardNumber++)
        {
            if (this.deck.Count > 0)
            {
                FaceOffCard drawnCard = drawACard();
                cycleCard(drawnCard);
            }
        }
    }

    public void playCardFromCycle(FaceOffCard cardToPlay)
    {
        if (this.team.Count < 6)
        {
            this.team.Add(cardToPlay);
            this.repositionTeam();
            this.repositionHand();
        }
    }

    public void playRandomCardFromHand()
    {
        if (this.hasPlayableCardInHand())
        {
            FaceOffCard selectedCard = this.hand[random.Next(this.hand.Count)];
            if (selectedCard.getCardType() == CardType.FANATIC)
            {
                this.playCardFromHand(selectedCard);
            } else
            {
                //card is gear
                if (this.getTeam().Count > 0)
                {
                    //can attach gear
                    this.attachGearCardToRandomFanatic(selectedCard);
                } else
                {
                    playRandomCardFromHand();
                }
            }
        }
        this.repositionCards();
    }

    public bool hasPlayableCardInHand()
    {
        bool hasPlayableCardInHand = false;
        bool hasFanaticInTeam = this.getTeam().Count > 0;
        foreach (FaceOffCard card in this.getHand())
        {
            if (card.isFanatic())
            {
                return this.getTeam().Count < 6;
            }
            if (card.isGear() && hasFanaticInTeam)
            {
                hasPlayableCardInHand = true;
            }
        }
        return hasPlayableCardInHand;
    }

    public void playCardFromHand(FaceOffCard cardToPlay)
    {
        if (this.hand.Contains(cardToPlay) && this.team.Count < 6)
        {
            this.team.Add(cardToPlay);
            this.hand.Remove(cardToPlay);

            this.repositionTeam();
            this.repositionHand();

            cardToPlay.activateEffectsFor(FaceOffCardEffectTiming.WHEN_YOU_PLAY_THIS_CARD);
        }
    }

    public void attachGearCardToRandomFanatic(FaceOffCard gearCard)
    {
        if (this.getTeam().Count > 0)
        {
            FaceOffCard selectedFanatic = this.team[random.Next(this.team.Count)];
            attachGearToFanatic(gearCard, selectedFanatic);
        }
    }

    public void playGameObjectCardFromHand(GameObject cardToPlay)
    {
        int indexOfCardToPlay = this.findIndexOfGameObjectInHand(cardToPlay);
        if (indexOfCardToPlay > -1 && this.team.Count < 6)
        {
            FaceOffCard playedCard = this.hand[indexOfCardToPlay];
            playCardFromHand(playedCard);
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

    public void attachGearToFanatic(FaceOffCard gear, FaceOffCard fanatic)
    {
        fanatic.attachGear(gear);
        this.hand.Remove(gear);

        fanatic.activateEffectsFor(FaceOffCardEffectTiming.WHEN_GEAR_IS_ATTACHED_TO_THIS_CARD);
        gear.activateEffectsFor(FaceOffCardEffectTiming.WHEN_YOU_GIVE_THIS_TO_A_FANATIC);

        this.repositionTeam();
        this.repositionHand();
    }

    public void selectFanaticForGearAttachment(GameObject fanaticSelected)
    {
        int indexOfFanaticSelected = this.findIndexOfGameObjectInTeam(fanaticSelected);
        if (indexOfFanaticSelected > -1)
        {
            FaceOffCard fanaticCardSelected = this.team[indexOfFanaticSelected];
            attachGearToFanatic(this.selectedGearCard, fanaticCardSelected);
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
        if (postDuelCard.getDuelResult() == DuelResult.Won)
        {
            this.drawXCardsIntoHand(1);
            this.handleTeamEffectsFor(FaceOffCardEffectTiming.WHEN_ONE_OF_YOUR_FANATICS_WINS_A_DUEL);
        }

        this.team.Remove(postDuelCard);
        this.deck.Add(postDuelCard);
        foreach (FaceOffCard gearCard in postDuelCard.getAttachedGear())
        {
            this.deck.Add(gearCard);
        }

        postDuelCard.resetCard();
        postDuelCard.setDuelResult(DuelResult.None);
    }

    //public void handleDuelWin(FaceOffCard postDuelCard)
    //{
    //    //remove card from team
    //    this.team.Remove(postDuelCard);

    //    //draw a card from deck first
    //    this.drawXCardsIntoHand(1);

    //    //put card on bottom of deck
    //    this.deck.Add(postDuelCard);

    //    //put attached gear on bottom of deck
    //    foreach(FaceOffCard gearCard in postDuelCard.getAttachedGear())
    //    {
    //        this.deck.Add(gearCard);
    //    }
    //    postDuelCard.resetCard();
    //}

    //public void handleDuelLoss(FaceOffCard postDuelCard)
    //{
    //    //put card on bottom of discard
    //    this.team.Remove(postDuelCard);
    //    this.discard.Add(postDuelCard);

    //    //put attached gear on bottom of discard
    //    foreach (FaceOffCard gearCard in postDuelCard.getAttachedGear())
    //    {
    //        this.discard.Add(gearCard);
    //    }
    //    postDuelCard.resetCard();
    //}

    public void dismissCard(FaceOffCard cardToDismiss)
    {
        if (this.deck.Contains(cardToDismiss))
        {
            this.deck.Remove(cardToDismiss);
        }

        if (this.discard.Contains(cardToDismiss))
        {
            this.deck.Remove(cardToDismiss);
        }

        if (this.hand.Contains(cardToDismiss))
        {
            this.hand.Remove(cardToDismiss);
        }
    }
}
