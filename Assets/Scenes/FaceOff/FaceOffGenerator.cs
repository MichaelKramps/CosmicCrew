using System;
using System.Collections.Generic;
using UnityEngine;

public class FaceOffGenerator
{
    private FaceOffPlayer player;
    private FaceOffPlayer enemy;
    private FaceOffStatus faceOffStatus;

    private FaceOffCard playerDuelCard;
    private FaceOffCard enemyDuelCard;

    public FaceOffGenerator(List<FaceOffCard> playerDeck, List<FaceOffCard> enemyDeck)
    {
        this.player = new FaceOffPlayer(playerDeck, FaceOffPlayerPosition.Bottom);
        this.enemy = new FaceOffPlayer(enemyDeck, FaceOffPlayerPosition.Top);
    }

    public void updateStatus(FaceOffStatus status)
    {
        this.faceOffStatus = status;
    }

    public FaceOffStatus currentStatus()
    {
        return this.faceOffStatus;
    }

    public List<FaceOffCard> drawPlayerStartingHand()
    {
        this.player.drawXCards(6);
        return this.player.getHand();
    }

    public List<FaceOffCard> drawEnemyStartingHand()
    {
        this.enemy.drawXCards(6);
        return this.enemy.getHand();
    }

    public void shuffleDecks()
    {
        this.player.shuffleDeck();
        this.enemy.shuffleDeck();
    }

    public void playGameObjectCardFromHand(GameObject cardToPlay)
    {
        this.player.playGameObjectCardFromHand(cardToPlay);
        this.faceOffStatus = FaceOffStatus.CheckForCardsToPlay;
    }

    public void selectFanaticForGearAttachment(GameObject fanaticSelected)
    {
        this.player.selectFanaticForGearAttachment(fanaticSelected);
        this.faceOffStatus = FaceOffStatus.CheckForCardsToPlay;
    }

    public void selectGameObjectGearCardFromHand(GameObject gearCardToSelect)
    {
        this.player.selectGameObjectGearCardFromHand(gearCardToSelect);
        this.faceOffStatus = FaceOffStatus.WaitingForPlayerToSelectFanaticForGearAttachment;
    }

    public void playRandomCardFromEnemyHand()
    {
        this.enemy.playRandomCardFromHand();
    }

    public void repositionCards()
    {
        this.player.repositionCards();
        this.enemy.repositionCards();
    }

    public int playerNumberCardsInHand()
    {
        return this.player.getHand().Count;
    }

    public bool playerHasCardsInHand()
    {
        return this.player.getHand().Count > 0;
    }

    public int enemyNumberCardsInHand()
    {
        return this.enemy.getHand().Count;
    }

    public bool enemyHasCardsInHand()
    {
        return this.enemy.getHand().Count > 0;
    }

    public void checkForCardsToPlay()
    {
        if (this.playerHasCardsInHand() && this.playerNumberCardsInHand() >= this.enemyNumberCardsInHand())
        {
            this.faceOffStatus = FaceOffStatus.WaitingForPlayerToPlayCard;
        }
        else if (this.enemyHasCardsInHand())
        {
            this.faceOffStatus = FaceOffStatus.WaitingForEnemyToPlayCard;
        }
        else
        {
            this.faceOffStatus = FaceOffStatus.ChooseDuelCards;
        }
    }

    public void selectCardsForDuel()
    {
        //check if face off is over
        if (this.player.getTeam().Count == 0 || this.enemy.getTeam().Count == 0)
        {
            this.faceOffStatus = FaceOffStatus.End;
        }
        else
        {
            playerDuelCard = this.player.selectRandomFanaticFromTeam();
            enemyDuelCard = this.enemy.selectRandomFanaticFromTeam();

            playerDuelCard.selectForDuel();
            enemyDuelCard.selectForDuel();

            this.faceOffStatus = FaceOffStatus.Duel;
        }
    }

    public void duel()
    {
        new FaceOffDuel(playerDuelCard, enemyDuelCard).duel();

        this.player.handleDuelResult(playerDuelCard);
        this.enemy.handleDuelResult(enemyDuelCard);

        this.faceOffStatus = FaceOffStatus.AfterDuel;
    }

    public void afterDuel()
    {
        playerDuelCard = null;
        enemyDuelCard = null;

        if (this.player.getHand().Count > 0)
        {
            this.faceOffStatus = FaceOffStatus.WaitingForPlayerToPlayCard;
        } else if (this.enemy.getHand().Count > 0)
        {
            this.faceOffStatus = FaceOffStatus.WaitingForEnemyToPlayCard;
        } else
        {
            this.faceOffStatus = FaceOffStatus.ChooseDuelCards;
        }
    }

    public void endFaceOff()
    {
        if (this.player.getTeam().Count > 0 || this.enemy.getTeam().Count == 0)
        {
            //player survives
            FandomForge.increaseLevel();
            FandomForge.payoutDividends();
        } else
        {
            //player loses life
        }

    }
}
