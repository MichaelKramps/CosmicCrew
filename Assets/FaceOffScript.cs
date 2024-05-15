using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FaceOffScript : MonoBehaviour
{
    public GameObject crewCardPrefab;

    private FaceOffStatus faceOffStatus;
    private FaceOffGenerator faceOffGenerator;

    private GameObject selectedCard;

    // Start is called before the first frame update
    void Start()
    {
        faceOffGenerator = new FaceOffGenerator(
            setupDeck(FandomForge.getPlayer().getPlayerDeck(), -3f),
            setupDeck(FandomForge.getEnemyGenerator().generateEnemyFaceOffDeck(FandomForge.currentLevel), 3f));
        //deal top 6 cards of face off deck
        faceOffGenerator.shuffleDecks();
        List<GameObject> playerStartingHand = faceOffGenerator.drawPlayerStartingHand();
        List<GameObject> enemyStartingHand = faceOffGenerator.drawEnemyStartingHand();



        faceOffGenerator.repositionCards();

        faceOffStatus = FaceOffStatus.WaitingToPlayCard;
    }

    // Update is called once per frame
    void Update()
    {
        switch (faceOffStatus)
        {
            case FaceOffStatus.WaitingToPlayCard:
                waitingToPlayCard();
                break;
            default:
                break;
        }
    }

    private List<GameObject> setupDeck(List<CrewCard> deck, float yValue)
    {
        List<GameObject> instantiatedDeck = new List<GameObject>();
        foreach(CrewCard card in deck)
        {
            crewCardPrefab.GetComponent<CrewCardScript>().crewCard = card;
            instantiatedDeck.Add(Instantiate(
                crewCardPrefab,
                new Vector3(9.1f, yValue, transform.position.z),
                transform.rotation));
        }
        return instantiatedDeck;
    }

    private void waitingToPlayCard()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null && hit.transform.gameObject.tag == "Card")
        {
            if (selectedCard != hit.transform.gameObject)
            {
                unselectSelectedCard();
                selectedCard = hit.transform.gameObject;
            }
                
            //Check for mouse click 
            if (Input.GetMouseButtonDown(0))
            {
                //play card
                playSelectedCardFromHand();
            } else
            {
                enlargeSelectedCard();
            }
        } else
        {
            unselectSelectedCard();
        }
    }

    private void unselectSelectedCard()
    {
        if (selectedCard != null)
        {
            selectedCard.GetComponent<SortingGroup>().sortingLayerName = "Cards";
            selectedCard.GetComponent<Transform>().localScale = new Vector3(0.3f, 0.3f);
            selectedCard = null;
        }
    }

    private void enlargeSelectedCard()
    {
        if (selectedCard != null)
        {
            selectedCard.GetComponent<SortingGroup>().sortingLayerName = "Over Cards";
            selectedCard.GetComponent<Transform>().localScale = new Vector3(0.65f, 0.65f);
        }
    }

    private void playSelectedCardFromHand()
    {
        faceOffGenerator.playCardFromHand(selectedCard);
    }
}
