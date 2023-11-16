using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ClashScript : MonoBehaviour
{
    public GameObject crewCardPrefab;
    public List<CrewCard> crewCardList;

    private ClashPlayer player1;
    private ClashPlayer player2;

    private GameObject activeGameObject;

    private ClashAnimationQueue clashAnimationQueue;

    private AnimationHelper animationHelper = new AnimationHelper();

    // Start is called before the first frame update
    void Start()
    {
        // set player1 and player2
        player1 = new ClashPlayer("0,0,1,1,2,2");
        player2 = new ClashPlayer("1,0,2,1,0,2");
        // set animationQueue
        // clashAnimationQueue = new ClashAnimationQueue("");
        clashAnimationQueue = new ClashAnimationQueue("p,dws,1,0*s,dws,1,0");


        //create player 1 deck
        foreach (int cardId in player1.getStartingDeckIds())
        {
            crewCardPrefab.GetComponent<CrewCardScript>().crewCard = crewCardList[cardId];
            GameObject thisCard = Instantiate(
                    crewCardPrefab,
                    new Vector3(ClashConstants.deckX, ClashConstants.player1DeckY, transform.position.z),
                    transform.rotation);
            player1.addToDeck(thisCard);
        }

        //player player 2 deck
        foreach (int cardId in player2.getStartingDeckIds())
        {
            GameObject thisCard = Instantiate(
                    crewCardPrefab,
                    new Vector3(ClashConstants.deckX, ClashConstants.player2DeckY, transform.position.z),
                    transform.rotation);
            player2.addToDeck(thisCard);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (clashAnimationQueue.getCurrentAnimationType())
        {
            case ClashAnimationType.DrawCard:
                DrawCardAnimation();
                break;
            default:
                NextAnimation();
                break;
        }
    }

    void DrawCardAnimation()
    {
        switch (clashAnimationQueue.getCurrentActingPlayer())
        {
            case ActingPlayer.Primary:
                activeGameObject = player1.getTopCardInDeck();
                activeGameObject.GetComponent<SortingGroup>().sortingLayerName = "Featured";
                if (animationHelper.NotYetReachedDestination(new Vector3(ClashConstants.viewCardX, ClashConstants.player1CardY), activeGameObject) ||
                    animationHelper.NotYetReachedScale(ClashConstants.viewCardScaleSize, activeGameObject))
                {
                    animationHelper.ScaleTowardsSize(ClashConstants.viewCardScaleSize, ClashConstants.defaultCardScale, ClashConstants.viewCardAnimationTime, activeGameObject);

                    activeGameObject.transform.position = animationHelper.MoveTowardsPoint(new Vector3(ClashConstants.viewCardX, ClashConstants.player1CardY), new Vector3(ClashConstants.deckX, ClashConstants.player1DeckY), ClashConstants.viewCardAnimationTime, activeGameObject);
                }
                else
                {
                    player1.drawCard();
                    NextAnimation();
                }
                break;
            case ActingPlayer.Secondary:
                activeGameObject = player2.getTopCardInDeck();
                activeGameObject.GetComponent<SortingGroup>().sortingLayerName = "Featured";
                if (animationHelper.NotYetReachedDestination(new Vector3(ClashConstants.viewCardX, ClashConstants.player2CardY), activeGameObject) ||
                    animationHelper.NotYetReachedScale(ClashConstants.viewCardScaleSize, activeGameObject))
                {
                    animationHelper.ScaleTowardsSize(ClashConstants.viewCardScaleSize, ClashConstants.defaultCardScale, ClashConstants.viewCardAnimationTime, activeGameObject);

                    activeGameObject.transform.position = animationHelper.MoveTowardsPoint(new Vector3(ClashConstants.viewCardX, ClashConstants.player2CardY), new Vector3(ClashConstants.deckX, ClashConstants.player2DeckY), ClashConstants.viewCardAnimationTime, activeGameObject);
                }
                else
                {
                    player1.drawCard();
                    NextAnimation();
                }
                break;
            default:
                NextAnimation();
                break;
        }
    }

    void NextAnimation()
    {
        clashAnimationQueue.nextAnimation();
    }
}
