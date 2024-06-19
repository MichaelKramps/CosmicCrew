using System;
using System.Collections.Generic;

public class FandomDeck
{
    private String deckName;
    private int difficulty;
    List<CrewCard> deck;


    public FandomDeck(){}

    public FandomDeck withName(String name)
    {
        this.deckName = name;
        return this;
    }

    public FandomDeck withDifficulty(int difficulty)
    {
        this.difficulty = difficulty;
        return this;
    }

    public FandomDeck withDeck(List<CrewCard> deck)
    {
        this.deck = deck;
        return this;
    }

    public List<CrewCard> getDeck()
    {
        return this.deck;
    }
}
