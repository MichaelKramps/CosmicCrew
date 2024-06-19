using System;
using System.Collections.Generic;

public class EnemyDeckGenerator
{
    public EnemyDeckGenerator()
    {
    }

    public FandomDeck generateEnemyFaceOffDeck(int level)
    {
        bool shouldGenerateDeck = new System.Random().Next(2) == 0 ? false : true; 

        if (shouldGenerateDeck)
        {
            List<CrewCard> generatedDeck = generateDeck(level);
            String generatedDeckName = generateNameForDeck(generatedDeck);
            int generatedDeckDifficulty = determineDeckDifficulty(generatedDeck);

            return new FandomDeck()
                .withName(generatedDeckName)
                .withDifficulty(generatedDeckDifficulty)
                .withDeck(generatedDeck);
        } else
        {
            return getPremadeDeck(level);
        }
        
    }

    private FandomDeck getPremadeDeck(int level)
    {
        //need list of premade decks
        List<CrewCard> deck = new List<CrewCard> {
            CardFinder.getCrewCardFromId(0),
            CardFinder.getCrewCardFromId(0),
            CardFinder.getCrewCardFromId(0),
            CardFinder.getCrewCardFromId(1),
            CardFinder.getCrewCardFromId(1),
            CardFinder.getCrewCardFromId(1),
            CardFinder.getCrewCardFromId(2),
            CardFinder.getCrewCardFromId(2),
            CardFinder.getCrewCardFromId(2)};

        return new FandomDeck()
            .withName("Fanatics for Hire")
            .withDifficulty(0)
            .withDeck(deck);
    }

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

    private int determineDeckDifficulty(List<CrewCard> deck)
    {
        return 3;
    }
}
