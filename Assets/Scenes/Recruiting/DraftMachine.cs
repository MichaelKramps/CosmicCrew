using System;
using System.Linq;
using System.Collections.Generic;

public class DraftMachine
{
    private CardFinder cardFinder;
    private List<CrewCard> draftPool;
    private List<CrewCard> recruitingHand;
    private FandomForgePlayer player;

    private System.Random random;

    public DraftMachine()
    {
        cardFinder = new CardFinder();
        draftPool = cardFinder.getStartingDraftPool();
        random = new System.Random();
    }

    public void draftCard(int cardIndex)
    {
        
    }

    public void refresh()
    {

        newRecruitingHand();
    }

    private void shuffleDraftPool()
    {
        draftPool = this.draftPool.OrderBy(x => random.Next()).ToList();
    }

    public void newRecruitingHand()
    {
        shuffleDraftPool();

        recruitingHand = new List<CrewCard>();
        recruitingHand.Add(draftPool[0]);
        recruitingHand.Add(draftPool[1]);
        recruitingHand.Add(draftPool[2]);
    }

    public List<CrewCard> currentRecruitingHand()
    {
        return recruitingHand;
    }

    public void recruitCard (int whichCard)
    {
        //remove card from recruitingHand
        recruitingHand[whichCard - 1] = null;
    }
}
