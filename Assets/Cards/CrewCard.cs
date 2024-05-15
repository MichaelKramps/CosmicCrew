using UnityEngine;

[CreateAssetMenu(fileName = "CrewCard", menuName = "Cards/CrewCard")]
public class CrewCard : ScriptableObject
{
    public string cardName;
    public string cardText;

    public Sprite image;
    public Sprite backgroundTexture;

    public int power;
    public int powerCounters;
    public int numberAllowedInDeck;
    public int cardId;

    public FandomType fandomType;
}