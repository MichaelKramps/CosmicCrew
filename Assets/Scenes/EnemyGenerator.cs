using System;
using System.Collections.Generic;

public class EnemyGenerator
{
    public EnemyGenerator()
    {
    }

    public List<CrewCard> generateEnemyFaceOffDeck(int level)
    {
        return new List<CrewCard> {
            CardFinder.getCrewCardFromId(0),
            CardFinder.getCrewCardFromId(0),
            CardFinder.getCrewCardFromId(0),
            CardFinder.getCrewCardFromId(1),
            CardFinder.getCrewCardFromId(1),
            CardFinder.getCrewCardFromId(1),
            CardFinder.getCrewCardFromId(2),
            CardFinder.getCrewCardFromId(2),
            CardFinder.getCrewCardFromId(2),};
    }

    public List<int> getCardLevelsInFaceOffDeck(int level)
    {
        return new List<int>();
    }
}
