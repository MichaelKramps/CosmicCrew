using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using TMPro;

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
        faceOffGenerator.addCardOwners();
        faceOffGenerator.shuffleDecks();
        List<FaceOffCard> playerStartingHand = faceOffGenerator.drawPlayerStartingHand();
        List<FaceOffCard> enemyStartingHand = faceOffGenerator.drawEnemyStartingHand();



        faceOffGenerator.repositionCards();

        faceOffGenerator.checkForCardsToPlay();
        updateDeckCounts();
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
                case FaceOffStatus.WaitingForPlayerToSelectFanaticForGearAttachment:
                    waitingForPlayerToSelectFanaticForGearAttachment();
                    break;
                case FaceOffStatus.WaitingForEnemyToPlayCard:
                    waitingForEnemyToPlayCard();
                    break;
                case FaceOffStatus.CheckForCardsToPlay:
                    faceOffGenerator.checkForCardsToPlay();
                    break;
                case FaceOffStatus.ChooseDuelCards:
                    if (FaceOffGlobalInformation.noAnimations())
                    {
                        selectCardsForDuel();
                    }
                    break;
                case FaceOffStatus.Duel:
                    if (FaceOffGlobalInformation.noAnimations())
                    {
                        duel();
                    }
                    break;
                case FaceOffStatus.AfterDuel:
                    if (FaceOffGlobalInformation.noAnimations())
                    {
                        afterDuel();
                    }
                    break;
                case FaceOffStatus.End:
                    if (FaceOffGlobalInformation.noAnimations())
                    {
                        faceOffGenerator.endFaceOff();
                        SceneManager.LoadScene("Recruiting Phase");
                    }
                    break;
                default:
                    break;
            }
            if (!waitingOnPlayer())
            {
                updateDeckCounts();
            }
        }
    }

    private bool waitingOnPlayer()
    {
        return this.faceOffGenerator.currentStatus() == FaceOffStatus.WaitingForPlayerToPlayCard ||
                this.faceOffGenerator.currentStatus() == FaceOffStatus.WaitingForPlayerToSelectFanaticForGearAttachment;
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
        if (!faceOffGenerator.playerHasPlayableCardsInHand())
        {
            faceOffGenerator.checkForCardsToPlay();
        }
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

    private void waitingForPlayerToSelectFanaticForGearAttachment()
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
                selectFanaticAndPlayGearCard();
            }
            else
            {
                enlargeSelectedCard();
            }
        }
        else
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
        if (selectedCard.GetComponent<CrewCardScript>().crewCard.cardType == CardType.FANATIC)
        {
            faceOffGenerator.playGameObjectCardFromHand(selectedCard);
            unselectSelectedCard();
            wait(Animate.cardMovementTime * 2);
        } else
        {
            //card attaches to something
            this.faceOffGenerator.selectGameObjectGearCardFromHand(selectedCard);
        }
        
    }

    private void selectFanaticAndPlayGearCard()
    {
        faceOffGenerator.selectFanaticForGearAttachment(selectedCard);
        unselectSelectedCard();
        wait(Animate.cardMovementTime * 2);
    }

    private void waitingForEnemyToPlayCard()
    {
        if (!faceOffGenerator.enemyHasPlayableCardsInHand())
        {
            faceOffGenerator.checkForCardsToPlay();
        }
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

    private void updateDeckCounts()
    {
        GameObject.Find("Deck2/NumberCards").GetComponent<TextMeshPro>().text = faceOffGenerator.getEnemy().getDeck().Count.ToString();
        GameObject.Find("Deck1/NumberCards").GetComponent<TextMeshPro>().text = faceOffGenerator.getPlayer().getDeck().Count.ToString();
    }

    private void wait(int milliseconds)
    {
        this.waitTracker.wait(milliseconds);
    }
}
