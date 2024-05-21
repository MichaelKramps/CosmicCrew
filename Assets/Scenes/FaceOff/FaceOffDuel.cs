using System;
using UnityEngine;

public class FaceOffDuel
{
    private FaceOffCard playerFanatic;
    private FaceOffCard enemyFanatic;

    public FaceOffDuel(FaceOffCard playerFanatic, FaceOffCard enemyFanatic)
    {
        this.playerFanatic = playerFanatic;
        this.enemyFanatic = enemyFanatic;
    }

    public void duel()
    {
        this.activateGoesToDuelEffects();

        Debug.Log("player: " + playerFanatic.getTotalPower());
        Debug.Log("enemy: " + enemyFanatic.getTotalPower());
        //determine winner and loser
        if (playerFanatic.getTotalPower() > enemyFanatic.getTotalPower())
        {
            playerFanatic.setDuelResult(DuelResult.Won);
            enemyFanatic.setDuelResult(DuelResult.Lost);
        } else if (enemyFanatic.getTotalPower() > playerFanatic.getTotalPower())
        {
            enemyFanatic.setDuelResult(DuelResult.Won);
            playerFanatic.setDuelResult(DuelResult.Lost);
        } else
        {
            //tied
            playerFanatic.setDuelResult(DuelResult.Tied);
            enemyFanatic.setDuelResult(DuelResult.Tied);
        }
    }

    private void activateGoesToDuelEffects()
    {
        // activate card effects labeled as
        // "when this card goes to a duel..."
    }
}
