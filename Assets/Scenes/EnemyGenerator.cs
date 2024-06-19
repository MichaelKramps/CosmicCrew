using System;
using System.Collections.Generic;

public class EnemyGenerator
{
    public EnemyGenerator()
    {
    }

    public List<CrewCard> generateEnemyFaceOffDeck(int level)
    {
        return generateDeck(level);
    }

    public List<int> getCardLevelsInFaceOffDeck(int level)
    {
        return new List<int>();
    }

    private List<List<CrewCard>> premadeDecks = new List<List<CrewCard>> { };

    private List<CrewCard> generateDeck(int level)
    {
        return new List<CrewCard> {
            CardFinder.getRandomCrewCard(),
            CardFinder.getRandomCrewCard(),
            CardFinder.getRandomCrewCard(),
            CardFinder.getRandomCrewCard(),
            CardFinder.getRandomCrewCard(),
            CardFinder.getRandomCrewCard(),
            CardFinder.getRandomCrewCard(),
            CardFinder.getRandomCrewCard(),
            CardFinder.getRandomCrewCard(),
            CardFinder.getRandomCrewCard(),
            CardFinder.getRandomCrewCard(),
            CardFinder.getRandomCrewCard(),};
    }

    private String generateNameForDeck(List<CrewCard> deck)
    {
        return "cool deck";
    }
}
