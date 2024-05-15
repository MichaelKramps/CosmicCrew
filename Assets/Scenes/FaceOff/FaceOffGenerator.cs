using System;
using System.Collections.Generic;
using UnityEngine;

public class FaceOffGenerator
{
    private FaceOffPlayer player;
    private FaceOffPlayer enemy;

    public FaceOffGenerator(List<GameObject> playerDeck, List<GameObject> enemyDeck)
    {
        this.player = new FaceOffPlayer(playerDeck, FaceOffPlayerPosition.Bottom);
        this.enemy = new FaceOffPlayer(enemyDeck, FaceOffPlayerPosition.Top);
    }

    public List<GameObject> drawPlayerStartingHand()
    {
        player.drawXCards(6);
        return player.getHand();
    }

    public List<GameObject> drawEnemyStartingHand()
    {
        enemy.drawXCards(6);
        return enemy.getHand();
    }

    public void shuffleDecks()
    {
        player.shuffleDeck();
        enemy.shuffleDeck();
    }

    public void playCardFromHand(GameObject cardToPlay)
    {
        player.playCardFromHand(cardToPlay);
    }

    public void repositionCards()
    {
        player.repositionCards();
        enemy.repositionCards();
    }
}
