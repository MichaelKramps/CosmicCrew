using System;
public class ClashConstants
{
    public static float defaultCardScale = 0.5f;

    static float cardXDistanceApart = 2.8f;
    static float cardXStartingPoint = -7f;
    public static float cardX1 = cardXStartingPoint;
    public static float cardX2 = cardXStartingPoint + cardXDistanceApart;
    public static float cardX3 = cardXStartingPoint + (cardXDistanceApart * 2);
    public static float cardX4 = cardXStartingPoint + (cardXDistanceApart * 3);
    public static float cardX5 = cardXStartingPoint + (cardXDistanceApart * 4);
    public static float cardX6 = cardXStartingPoint + (cardXDistanceApart * 5);

    public static float player1CardY = -2.25f;
    public static float player2CardY = 2.25f;

    public static float player1DeckY = -2.5f;
    public static float player2DeckY = 2.5f;
    public static float deckX = cardXStartingPoint + (cardXDistanceApart * 6) + 0.2f;
    public static float discardX = cardXStartingPoint - cardXDistanceApart - 0.2f;

    public static float viewCardX = deckX - 0.5f;
    public static float viewCardScaleSize = 0.6f;
    public static float viewCardAnimationTime = 750f;

}
