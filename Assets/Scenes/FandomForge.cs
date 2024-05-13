﻿using System;
using System.Collections.Generic;

public static class FandomForge
{
    private static DraftMachine draftMachine = new DraftMachine();
    private static FandomForgePlayer player = new FandomForgePlayer();

    public static bool recruitCard(int whichCard)
    {
        int recruitmentCost = 2;
        CrewCard cardToRecruit = draftMachine.currentRecruitingHand()[whichCard - 1];
        if (player.canAffordToPay(recruitmentCost) && cardToRecruit != null)
        {
            //pay the cost
            player.pay(recruitmentCost);
            //add card to face off deck
            player.addToDeck(cardToRecruit);
            //remove card from recruiting hand
            draftMachine.recruitCard(whichCard);

            return true;
        }
        return false;
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
        return true;
    }

    public static bool payToDismiss()
    {
        return true;
    }

    public static bool dismiss()
    {
        return true;
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

    public static Dictionary<CrewCard, int> getFaceOffDeckSummary()
    {
        return player.getFaceOffDeckSummary();
    }
}
