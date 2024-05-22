using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class FaceOffScript : MonoBehaviour
{
    public GameObject crewCardPrefab;

    private FaceOffGenerator faceOffGenerator;

    private GameObject selectedCard;
    private WaitTracker waitTracker = new WaitTracker();

    // Start is called before the first frame update
    void Start()
    {
        faceOffGenerator = new FaceOffGenerator(
            setupDeck(FandomForge.getPlayer().getPlayerDeck(), FaceOffPlayerPosition.Bottom),
            setupDeck(FandomForge.getEnemyGenerator().generateEnemyFaceOffDeck(FandomForge.currentLevel), FaceOffPlayerPosition.Top));
        //deal top 6 cards of face off deck
        faceOffGenerator.shuffleDecks();
        List<FaceOffCard> playerStartingHand = faceOffGenerator.drawPlayerStartingHand();
        List<FaceOffCard> enemyStartingHand = faceOffGenerator.drawEnemyStartingHand();



        faceOffGenerator.repositionCards();

        faceOffGenerator.checkForCardsToPlay();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.waitTracker.isWaiting())
        {
            this.waitTracker.updateTime(Time.deltaTime);
        } else
        {
            switch (this.faceOffGenerator.currentStatus())
            {
                case FaceOffStatus.WaitingForPlayerToPlayCard:
                    waitingForPlayerToPlayCard();
                    break;
                case FaceOffStatus.WaitingForEnemyToPlayCard:
                    waitingForEnemyToPlayCard();
                    break;
                case FaceOffStatus.CheckForCardsToPlay:
                    faceOffGenerator.checkForCardsToPlay();
                    break;
                case FaceOffStatus.ChooseDuelCards:
                    selectCardsForDuel();
                    break;
                case FaceOffStatus.Duel:
                    duel();
                    break;
                case FaceOffStatus.AfterDuel:
                    afterDuel();
                    break;
                case FaceOffStatus.End:
                    faceOffGenerator.endFaceOff();
                    SceneManager.LoadScene("Recruiting Phase");
                    break;
                default:
                    break;
            }
        }
    }

    private List<FaceOffCard> setupDeck(List<CrewCard> deck, FaceOffPlayerPosition faceOffPlayerPosition)
    {
        List<FaceOffCard> instantiatedDeck = new List<FaceOffCard>();
        foreach(CrewCard card in deck)
        {
            float yValue = faceOffPlayerPosition == FaceOffPlayerPosition.Top ? 3f : -3f;
            crewCardPrefab.GetComponent<CrewCardScript>().crewCard = card;
            GameObject cardGameObject = Instantiate(
                crewCardPrefab,
                new Vector3(9.1f, yValue, transform.position.z),
                transform.rotation);
            instantiatedDeck.Add(new FaceOffCard(cardGameObject, faceOffPlayerPosition));
        }
        return instantiatedDeck;
    }

    private void waitingForPlayerToPlayCard()
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
        faceOffGenerator.playGameObjectCardFromHand(selectedCard);
        unselectSelectedCard();
        wait(Animate.cardMovementTime * 2);
    }

    private void waitingForEnemyToPlayCard()
    {
        faceOffGenerator.playRandomCardFromEnemyHand();
        faceOffGenerator.checkForCardsToPlay();
        wait(Animate.cardMovementTime * 2);
    }

    private void selectCardsForDuel()
    {
        faceOffGenerator.selectCardsForDuel();
        wait(Animate.cardMovementTime * 2);
    }

    private void duel()
    {
        faceOffGenerator.duel();
        faceOffGenerator.repositionCards();
        wait(Animate.cardMovementTime * 2);
    }

    private void afterDuel()
    {
        //figure out if card(s) need to be played
        faceOffGenerator.afterDuel();
    }

    private void wait(int milliseconds)
    {
        this.waitTracker.wait(milliseconds);
    }
}
