using System;
using UnityEngine;
using System.Collections.Generic;

public class ClashPlayer
{
    List<int> startingDeckIds = new List<int>();
    List<GameObject> deck;

    public ClashPlayer(String deckString)
    {
        string[] allIdStrings = deckString.Split(",");
        for (int index = 0; index < allIdStrings.Length; index++)
        {
            string thisIdString = allIdStrings[index];
            try
            {
                startingDeckIds.Add(Int32.Parse(thisIdString));
            }
            catch
            {
                startingDeckIds.Add(0);
            }
        }
    }

    public void createDeck(List<CrewCard> allCrewCards)
    {

    }
}
