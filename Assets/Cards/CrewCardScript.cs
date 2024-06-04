using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CrewCardScript : MonoBehaviour
{
    public CrewCard crewCard;

    public TMPro.TextMeshPro cardName;
    public TMPro.TextMeshPro customText;

    public TMPro.TextMeshPro power;
    public TMPro.TextMeshPro powerCounters;
    public TMPro.TextMeshPro civilizationType;

    public SpriteRenderer artwork;
    public SpriteRenderer backgroundTexture;

    // Start is called before the first frame update
    void Start()
    {
        power.text = crewCard.cardType == CardType.FANATIC ? crewCard.power.ToString() : "";
        powerCounters.text = "0";

        artwork.sprite = crewCard.image;
    }

    // Update is called once per frame
    void Update()
    {

    }
}