﻿using System;
using System.Collections.Generic;
using UnityEngine;

public static class FandomForge
{
    private static DraftMachine draftMachine = new DraftMachine();
    private static FandomForgePlayer player = new FandomForgePlayer();
    private static EnemyGenerator enemyGenerator = new EnemyGenerator();
    public static CurrentRecruitingAction currentRecruitingAction;
    public static CrewCard selectedCrewCard;
    public static int currentLevel;

    private static int oneTimeRecruitingCostReduction = 0;

    public static void resetForge()
    {
        //reset stuff
        FandomForge.draftMachine = new DraftMachine();
        FandomForge.player = new FandomForgePlayer();
        FandomForge.enemyGenerator = new EnemyGenerator();
        FandomForge.currentRecruitingAction = CurrentRecruitingAction.Recruiting;
        FandomForge.selectedCrewCard = null;
        FandomForge.currentLevel = 0;
    }

    public static FandomForgePlayer getPlayer()
    {
        return FandomForge.player;
    }

    public static EnemyGenerator getEnemyGenerator()
    {
        return FandomForge.enemyGenerator;
    }

    public static bool recruitCard(int whichCard)
    {
        CrewCard cardToRecruit = draftMachine.currentRecruitingHand()[whichCard - 1];
        int recruitmentCost = FandomForge.determineCostOfCard(cardToRecruit);
        if (player.canAffordToPay(recruitmentCost) && cardToRecruit != null)
        {
            //pay the cost
            player.pay(recruitmentCost);
            //add card to face off deck
            player.addToDeck(cardToRecruit);
            //remove card from recruiting hand
            draftMachine.recruitCard(whichCard);
            //reset oneTimeCostReduction
            FandomForge.resetOneTimeRecruitingCostReduction();

            return true;
        }
        return false;
    }

    public static int determineCostOfCard(CrewCard card)
    {
        int rawCost = card.cost - FandomForge.oneTimeRecruitingCostReduction;
        return rawCost > 0 ? rawCost : 0;
    }

    private static void resetOneTimeRecruitingCostReduction()
    {
        FandomForge.oneTimeRecruitingCostReduction = 0;
    }

    public static void setOneTimeRecruitingCostReduction(int reduction)
    {
        FandomForge.oneTimeRecruitingCostReduction = reduction;
    }

    public static bool refresh()
    {
        int refreshCost = 1;
        if (player.canAffordToPay(refreshCost))
        {
            //refresh recruiting hand from draft machine
            player.pay(refreshCost);
            draftMachine.newRecruitingHand();
            return true;
        }
        //return false if can't afford
        return false;
    }

    public static bool payToPlace()
    {
        int placeCost = 1;
        if (player.canAffordToPay(placeCost))
        {
            player.pay(placeCost);
            return true;
        }
        //return false if can't afford
        return false;
    }

    public static bool place()
    {
        return true;
    }

    public static bool invest()
    {
        int investCost = 3;
        if (player.canAffordToPay(investCost))
        {
            player.pay(investCost);
            player.invest();
            return true;
        }
        return false;
    }

    public static bool dismissScreen()
    {
        int dismissCost = 3;
        if (player.canAffordToPay(dismissCost))
        {
            FandomForge.currentRecruitingAction = CurrentRecruitingAction.Dismiss;
            return true;
        }
        return false;
    }

    public static bool dismissCard(CrewCard cardToDismiss)
    {
        int dismissCost = 3;
        player.pay(dismissCost);
        return player.dismissCard(cardToDismiss);
    }

    public static DraftMachine getDraftMachine()
    {
        return draftMachine;
    }

    public static void newRecruitingHand()
    {
        draftMachine.newRecruitingHand();
    }

    public static List<CrewCard> currentRecruitingHand()
    {
        return draftMachine.currentRecruitingHand();
    }

    public static int currentRecruitingTokens()
    {
        return player.currentRecruitingTokens();
    }

    public static int currentTokensGenerated()
    {
        return player.currentTokensGenerated();
    }

    public static Dictionary<CrewCard, int> getFaceOffDeckSummary()
    {
        return player.getFaceOffDeckSummary();
    }

    public static void increaseLevel()
    {
        FandomForge.currentLevel += 1;
    }

    public static void payoutDividends()
    {
        player.payoutDividends();
    }
}
