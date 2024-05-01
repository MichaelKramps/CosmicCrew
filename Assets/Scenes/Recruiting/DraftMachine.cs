using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class DraftMachine
{
    private CardFinder cardFinder;
    private List<CrewCard> draftPool;
    private System.Random random;

    public DraftMachine()
    {
        cardFinder = new CardFinder();
        draftPool = cardFinder.getStartingDraftPool();
        random = new System.Random();
    }

    private void shuffleDraftPool()
    {
        draftPool = cardFinder.getStartingDraftPool().OrderBy(x => random.Next()).ToList();
    }

    public List<CrewCard> newRecruitingHand()
    {
        Debug.Log(draftPool.Count);
        shuffleDraftPool();
        Debug.Log(draftPool.Count);

        List<CrewCard> recruitingHand = new List<CrewCard>();
        recruitingHand.Add(draftPool[0]);
        recruitingHand.Add(draftPool[1]);
        recruitingHand.Add(draftPool[2]);

        return recruitingHand;
    }
}
