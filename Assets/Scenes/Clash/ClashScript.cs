using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;

public class ClashScript : MonoBehaviour
{
    public GameObject crewCardPrefab;
    public GameObject diePrefab;
    public List<CrewCard> crewCardList;

    private ClashPlayer player1;
    private ClashPlayer player2;

    private GameObject activeCard1;
    private GameObject activeCard2;
    private GameObject die1;
    private GameObject die2;

    private ClashAnimationQueue clashAnimationQueue;

    private float waitTimeInMilliseconds = 0;

    private AnimationHelper animationHelper = new AnimationHelper();

    // Start is called before the first frame update
    void Start()
    {
        // set player1 and player2
        player1 = new ClashPlayer("0,8,9,11,13,12,12,8,7");
        player2 = new ClashPlayer("6,13,7,8,5,4,1,9,2,11,12,3,0,10");
        // set animationQueue
        // clashAnimationQueue = new ClashAnimationQueue("");
        clashAnimationQueue = new ClashAnimationQueue("p,dws,1,0*p,p,1,0*s,dws,1,0*s,p,1,0*s,pow,1,1*p,dws,1,0*p,p,2,0*s,dws,1,0*s,p,2,0*s,pow,1,2*p,dws,1,0*p,pow,1,2*p,p,3,0*s,dws,1,0*s,p,3,0*s,dws,1,0*s,uns,1,0*p,dws,1,0*p,pow,1,2*p,pow,1,1*p,p,4,0*s,dws,1,0*s,p,4,0*s,pow,3,4*p,dws,1,0*p,pow,1,2*p,pow,1,1*p,p,5,0*p,pow,1,2*p,pow,1,3*p,pow,1,4*p,pow,1,5*s,dws,1,0*s,p,5,0*s,pow,2,5*p,dws,1,0*p,pow,1,2*p,pow,1,1*p,p,6,0*s,dws,1,0*s,p,6,0*b,r,5,1*b,g1,5,1*p,dws,1,0*p,pow,1,2*p,pow,1,1*p,p,5,0*b,r,1,2*b,g1,1,2*p,dws,1,0*p,pow,1,2*p,pow,1,2*p,pow,1,2*p,pow,1,3*p,pow,1,4*p,pow,1,5*p,pow,1,6*p,p,1,0*b,r,6,4*b,g2,6,4*p,dws,1,0*p,pow,1,1*p,pow,1,2*p,pow,1,1*p,uns,1,0*p,dws,1,0*p,pow,1,1*p,pow,1,2*p,pow,1,1*p,uns,1,0*p,dws,1,0*p,pow,1,1*p,pow,1,2*p,pow,1,1*p,pow,1,1*p,pow,1,2*p,pow,1,3*p,pow,1,4*p,pow,1,5*p,pow,1,6*p,uns,1,0*s,dws,1,0*s,p,4,0*b,r,6,5*b,g1,6,5*p,dws,1,0*p,pow,1,2*p,pow,1,2*p,p,1,0*p,dws,1,0*p,pow,1,2*p,pow,1,1*p,uns,1,0*b,r,2,1*b,g1,2,1*p,dws,1,0*p,pow,1,1*p,pow,1,1*p,pow,1,3*p,pow,1,4*p,pow,1,5*p,p,2,0*b,r,2,5*b,g2,2,5*s,dws,1,0*s,pow,1,4*s,p,6,0*b,r,3,2*b,g1,3,2*p,dws,1,0*p,pow,1,1*p,pow,1,4*p,pow,1,5*p,p,3,0*b,r,5,2*b,g1,5,2*p,dws,1,0*p,pow,1,3*p,p,5,0*p,pow,1,1*p,pow,1,3*p,pow,1,4*p,pow,1,5*p,1w,0,0");
        //

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
            crewCardPrefab.GetComponent<CrewCardScript>().crewCard = crewCardList[cardId];
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
            case ClashAnimationType.Wait:
                WaitAnimation();
                break;
            case ClashAnimationType.GetTopCard:
                GetTopCardAnimation();
                break;
            case ClashAnimationType.DrawCard:
                DrawCardAnimation();
                break;
            case ClashAnimationType.CycleCard:
                CycleCardAnimation();
                break;
            case ClashAnimationType.ResetActiveCard:
                ResetActiveCardAnimation();
                break;
            case ClashAnimationType.PlayCard:
                PlayCardAnimation();
                break;
            case ClashAnimationType.ShowDice:
                ShowDiceAnimation();
                break;
            case ClashAnimationType.RollDice:
                RollDiceAnimation();
                break;
            case ClashAnimationType.HideDice:
                HideDiceAnimation();
                break;
            case ClashAnimationType.SelectFighters:
                SelectFightersAnimation();
                break;
            case ClashAnimationType.FighterOneWins:
                FighterOneWinsAnimation();
                break;
            case ClashAnimationType.FighterTwoWins:
                FighterTwoWinsAnimation();
                break;
            case ClashAnimationType.FightersTie:
                FightersTieAnimation();
                break;
            case ClashAnimationType.PreparePowerCounter:
                PreparePowerCounterAnimation();
                break;
            case ClashAnimationType.PowerCounter:
                PowerCounterAnimation();
                break;
            default:
                NextAnimation();
                break;
        }
    }

    void GetTopCardAnimation()
    {
        switch (clashAnimationQueue.getCurrentActingPlayer())
        {
            case ActingPlayer.Primary:
                activeCard1 = player1.getTopCardInDeck();
                break;
            case ActingPlayer.Secondary:
                activeCard2 = player2.getTopCardInDeck();
                break;
        }
        NextAnimation();
    }

    void DrawCardAnimation()
    {
        switch (clashAnimationQueue.getCurrentActingPlayer())
        {
            case ActingPlayer.Primary:
                activeCard1.GetComponent<SortingGroup>().sortingLayerName = "Featured";
                if (animationHelper.NotYetReachedDestination(ClashConstants.player1ViewCard, activeCard1) ||
                    animationHelper.NotYetReachedScale(ClashConstants.viewCardScaleSize, activeCard1))
                {
                    animationHelper.ScaleTowardsSize(ClashConstants.viewCardScaleSize, ClashConstants.defaultCardScale, ClashConstants.viewCardAnimationTime, activeCard1);

                    animationHelper.MoveTowardsPoint(ClashConstants.player1ViewCard, ClashConstants.player1Deck, ClashConstants.viewCardAnimationTime, activeCard1);
                }
                else
                {
                    player1.drawCard();
                    NextAnimation();
                }
                break;
            case ActingPlayer.Secondary:
                activeCard2.GetComponent<SortingGroup>().sortingLayerName = "Featured";
                if (animationHelper.NotYetReachedDestination(ClashConstants.player2ViewCard, activeCard2) ||
                    animationHelper.NotYetReachedScale(ClashConstants.viewCardScaleSize, activeCard2))
                {
                    animationHelper.ScaleTowardsSize(ClashConstants.viewCardScaleSize, ClashConstants.defaultCardScale, ClashConstants.viewCardAnimationTime, activeCard2);

                    animationHelper.MoveTowardsPoint(ClashConstants.player2ViewCard, ClashConstants.player2Deck, ClashConstants.viewCardAnimationTime, activeCard2);
                }
                else
                {
                    player2.drawCard();
                    NextAnimation();
                }
                break;
            default:
                NextAnimation();
                break;
        }
    }

    void ResetActiveCardAnimation()
    {
        switch (clashAnimationQueue.getCurrentActingPlayer())
        {
            case ActingPlayer.Primary:
                activeCard1 = player1.getActiveCard();
                break;
            case ActingPlayer.Secondary:
                activeCard2 = player2.getActiveCard();
                break;
        }
        NextAnimation();
    }

    void PlayCardAnimation()
    {
        float cardX = ClashConstants.getPlayCardX(clashAnimationQueue.getCurrentAnimation().getPrimaryInteger());
        switch (clashAnimationQueue.getCurrentActingPlayer())
        {
            case ActingPlayer.Primary:
                Vector3 cardDestination1 = new Vector3(cardX, ClashConstants.player1CardY);
                if (animationHelper.NotYetReachedDestination(cardDestination1, activeCard1) ||
                    animationHelper.NotYetReachedScale(ClashConstants.defaultCardScale, activeCard1))
                {
                    animationHelper.ScaleTowardsSize(ClashConstants.defaultCardScale, ClashConstants.viewCardScaleSize, ClashConstants.viewCardAnimationTime, activeCard1);

                    animationHelper.MoveTowardsPoint(cardDestination1, ClashConstants.player1ViewCard, ClashConstants.viewCardAnimationTime, activeCard1);
                }
                else
                {
                    activeCard1.GetComponent<SortingGroup>().sortingLayerName = "Cards";
                    player1.playCard(clashAnimationQueue.getCurrentAnimation().getPrimaryInteger());
                    NextAnimation();
                }
                break;
            case ActingPlayer.Secondary:
                Vector3 cardDestination2 = new Vector3(cardX, ClashConstants.player2CardY);
                if (animationHelper.NotYetReachedDestination(cardDestination2, activeCard2) ||
                    animationHelper.NotYetReachedScale(ClashConstants.defaultCardScale, activeCard2))
                {
                    animationHelper.ScaleTowardsSize(ClashConstants.defaultCardScale, ClashConstants.viewCardScaleSize, ClashConstants.viewCardAnimationTime, activeCard2);

                    animationHelper.MoveTowardsPoint(cardDestination2, ClashConstants.player2ViewCard, ClashConstants.viewCardAnimationTime, activeCard2);
                }
                else
                {
                    activeCard2.GetComponent<SortingGroup>().sortingLayerName = "Cards";
                    player2.playCard(clashAnimationQueue.getCurrentAnimation().getPrimaryInteger());
                    NextAnimation();
                }
                break;
            default:
                NextAnimation();
                break;
        }
    }

    void ShowDiceAnimation()
    {
        die1 = Instantiate(
                    diePrefab,
                    new Vector3(ClashConstants.dieX, ClashConstants.player1DieY, transform.position.z),
                    transform.rotation);
        die2 = Instantiate(
                    diePrefab,
                    new Vector3(ClashConstants.dieX, ClashConstants.player2DieY, transform.position.z),
                    transform.rotation);
        NextAnimation();
    }

    void RollDiceAnimation()
    {
        if (waitTimeInMilliseconds < 1000)
        {
            waitTimeInMilliseconds += Time.deltaTime * 1000f;
            int randomRollableNumber1 = UnityEngine.Random.Range(1, 6);
            int randomRollableNumber2 = UnityEngine.Random.Range(1, 6);
            die1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("die-" + randomRollableNumber1.ToString());
            die2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("die-" + randomRollableNumber2.ToString());
        } else
        {
            waitTimeInMilliseconds = 0;
            die1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("die-" + clashAnimationQueue.getCurrentAnimation().getPrimaryInteger().ToString());
            die2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("die-" + clashAnimationQueue.getCurrentAnimation().getSecondaryInteger().ToString());
            NextAnimation();
        }
    }

    void HideDiceAnimation()
    {
        Destroy(die1);
        Destroy(die2);
        NextAnimation();
    }

    void WaitAnimation()
    {
        if (waitTimeInMilliseconds < clashAnimationQueue.getCurrentAnimation().getPrimaryInteger())
        {
            waitTimeInMilliseconds += Time.deltaTime * 1000f;
        } else
        {
            waitTimeInMilliseconds = 0;
            NextAnimation();
        }
    }

    void SelectFightersAnimation()
    {
        int fighterNumber1 = player1.fighterNumberFromRoll(clashAnimationQueue.getCurrentAnimation().getPrimaryInteger());
        int fighterNumber2 = player2.fighterNumberFromRoll(clashAnimationQueue.getCurrentAnimation().getSecondaryInteger());
        activeCard1 = player1.fighterFromRoll(fighterNumber1);
        activeCard1.GetComponent<SortingGroup>().sortingLayerName = "Featured";
        activeCard2 = player2.fighterFromRoll(fighterNumber2);
        activeCard2.GetComponent<SortingGroup>().sortingLayerName = "Featured";

        if (animationHelper.NotYetReachedDestination(ClashConstants.player1FightDestination, activeCard1) ||
            animationHelper.NotYetReachedDestination(ClashConstants.player2FightDestination, activeCard2) ||
                    animationHelper.NotYetReachedScale(ClashConstants.fightCardScale, activeCard1))
        {
            animationHelper.ScaleTowardsSize(ClashConstants.fightCardScale, ClashConstants.defaultCardScale, ClashConstants.viewCardAnimationTime, activeCard1);
            animationHelper.ScaleTowardsSize(ClashConstants.fightCardScale, ClashConstants.defaultCardScale, ClashConstants.viewCardAnimationTime, activeCard2);

            animationHelper.MoveTowardsPoint(ClashConstants.player1FightDestination, ClashConstants.getPlayer1CardLocation(fighterNumber1), ClashConstants.viewCardAnimationTime, activeCard1);
            animationHelper.MoveTowardsPoint(ClashConstants.player2FightDestination, ClashConstants.getPlayer2CardLocation(fighterNumber2), ClashConstants.viewCardAnimationTime, activeCard2);
        }
        else
        {
            player1.selectFighter(clashAnimationQueue.getCurrentAnimation().getPrimaryInteger());
            player2.selectFighter(clashAnimationQueue.getCurrentAnimation().getSecondaryInteger());
            //need to do this now before cards move again
            activeCard1.GetComponent<SortingGroup>().sortingOrder = player1.numberOfCardsInDiscard();
            activeCard2.GetComponent<SortingGroup>().sortingOrder = player2.numberOfCardsInDiscard();
            NextAnimation();
        }
    }

    void FighterOneWinsAnimation()
    {
        if (animationHelper.NotYetReachedDestination(ClashConstants.player1Deck, activeCard1) ||
            animationHelper.NotYetReachedDestination(ClashConstants.player2Discard, activeCard2) ||
                    animationHelper.NotYetReachedScale(ClashConstants.defaultCardScale, activeCard1))
        {
            animationHelper.ScaleTowardsSize(ClashConstants.defaultCardScale, ClashConstants.fightCardScale, ClashConstants.viewCardAnimationTime, activeCard1);
            animationHelper.ScaleTowardsSize(ClashConstants.defaultCardScale, ClashConstants.fightCardScale, ClashConstants.viewCardAnimationTime, activeCard2);

            animationHelper.MoveTowardsPoint(ClashConstants.player1Deck, ClashConstants.player1FightDestination, ClashConstants.viewCardAnimationTime, activeCard1);
            animationHelper.MoveTowardsPoint(ClashConstants.player2Discard, ClashConstants.player2FightDestination, ClashConstants.viewCardAnimationTime, activeCard2);
        }
        else
        {
            resetPower(activeCard1);
            resetPower(activeCard2);
            activeCard1.GetComponent<SortingGroup>().sortingLayerName = "Cards";
            activeCard2.GetComponent<SortingGroup>().sortingLayerName = "Cards";
            player1.winsFight();
            player2.losesFight();
            NextAnimation();
        }
    }

    void FighterTwoWinsAnimation()
    {
        if (animationHelper.NotYetReachedDestination(ClashConstants.player1Discard, activeCard1) ||
            animationHelper.NotYetReachedDestination(ClashConstants.player2Deck, activeCard2) ||
                    animationHelper.NotYetReachedScale(ClashConstants.defaultCardScale, activeCard1))
        {
            animationHelper.ScaleTowardsSize(ClashConstants.defaultCardScale, ClashConstants.fightCardScale, ClashConstants.viewCardAnimationTime, activeCard1);
            animationHelper.ScaleTowardsSize(ClashConstants.defaultCardScale, ClashConstants.fightCardScale, ClashConstants.viewCardAnimationTime, activeCard2);

            animationHelper.MoveTowardsPoint(ClashConstants.player1Discard, ClashConstants.player1FightDestination, ClashConstants.viewCardAnimationTime, activeCard1);
            animationHelper.MoveTowardsPoint(ClashConstants.player2Deck, ClashConstants.player2FightDestination, ClashConstants.viewCardAnimationTime, activeCard2);
        }
        else
        {
            resetPower(activeCard1);
            resetPower(activeCard2);
            activeCard1.GetComponent<SortingGroup>().sortingLayerName = "Cards";
            activeCard2.GetComponent<SortingGroup>().sortingLayerName = "Cards";
            player1.losesFight();
            player2.winsFight();
            NextAnimation();
        }
    }

    void FightersTieAnimation()
    {
        if (animationHelper.NotYetReachedDestination(ClashConstants.player1Discard, activeCard1) ||
            animationHelper.NotYetReachedDestination(ClashConstants.player2Discard, activeCard2) ||
                    animationHelper.NotYetReachedScale(ClashConstants.defaultCardScale, activeCard1))
        {
            animationHelper.ScaleTowardsSize(ClashConstants.defaultCardScale, ClashConstants.fightCardScale, ClashConstants.viewCardAnimationTime, activeCard1);
            animationHelper.ScaleTowardsSize(ClashConstants.defaultCardScale, ClashConstants.fightCardScale, ClashConstants.viewCardAnimationTime, activeCard2);

            animationHelper.MoveTowardsPoint(ClashConstants.player1Discard, ClashConstants.player1FightDestination, ClashConstants.viewCardAnimationTime, activeCard1);
            animationHelper.MoveTowardsPoint(ClashConstants.player2Discard, ClashConstants.player2FightDestination, ClashConstants.viewCardAnimationTime, activeCard2);
        }
        else
        {
            resetPower(activeCard1);
            resetPower(activeCard2);
            activeCard1.GetComponent<SortingGroup>().sortingLayerName = "Cards";
            activeCard2.GetComponent<SortingGroup>().sortingLayerName = "Cards";
            player1.losesFight();
            player2.losesFight();
            NextAnimation();
        }
    }

    void PreparePowerCounterAnimation()
    {
        int fighterSlot = clashAnimationQueue.getCurrentAnimation().getSecondaryInteger();
        switch (clashAnimationQueue.getCurrentActingPlayer())
        {
            case ActingPlayer.Primary:
                activeCard1 = player1.fighterFromRoll(fighterSlot);
                GameObject powerCountersObject1 = activeCard1.transform.Find("Power Counters").gameObject;
                SpriteRenderer powerCountersSprite1 = powerCountersObject1.GetComponent<SpriteRenderer>();
                powerCountersSprite1.sortingOrder = 3;
                powerCountersSprite1.transform.position = ClashConstants.getPowerCounterStartingPoint(activeCard1);
                powerCountersObject1.transform.Find("Number Counters").gameObject.GetComponent<SortingGroup>().sortingOrder = 3;
                break;
            case ActingPlayer.Secondary:
                activeCard2 = player2.fighterFromRoll(fighterSlot);
                GameObject powerCountersObject2 = activeCard2.transform.Find("Power Counters").gameObject;
                SpriteRenderer powerCountersSprite2 = powerCountersObject2.GetComponent<SpriteRenderer>();
                powerCountersSprite2.sortingOrder = 3;
                powerCountersSprite2.transform.position = ClashConstants.getPowerCounterStartingPoint(activeCard2);
                powerCountersObject2.transform.Find("Number Counters").gameObject.GetComponent<SortingGroup>().sortingOrder = 3;
                break;
        }
        NextAnimation();
    }

    void PowerCounterAnimation()
    {
        int numberPowerCounters = clashAnimationQueue.getCurrentAnimation().getPrimaryInteger();
        switch (clashAnimationQueue.getCurrentActingPlayer())
        {
            case ActingPlayer.Primary:
                GameObject activeObject1 = activeCard1.transform.Find("Power Counters").gameObject;
                if (animationHelper.NotYetReachedDestination(ClashConstants.getPowerCounterEndingPoint(activeCard1), activeObject1))
                {
                    animationHelper.MoveTowardsPoint(ClashConstants.getPowerCounterEndingPoint(activeCard1), ClashConstants.getPowerCounterStartingPoint(activeCard1), ClashConstants.powerCounterAnimationTime, activeObject1);
                }
                else
                {
                    GameObject numberCountersObject1 = activeCard1.transform.Find("Power Counters").Find("Number Counters").gameObject;
                    int currentPowerCounters1 = Int32.Parse(numberCountersObject1.GetComponent<TextMeshPro>().text);
                    int newPowerCounters1 = currentPowerCounters1 + numberPowerCounters;
                    numberCountersObject1.GetComponent<TextMeshPro>().text = newPowerCounters1.ToString();
                    GameObject powerObject1 = activeCard1.transform.Find("Power").gameObject;
                    int currentPower1 = Int32.Parse(powerObject1.GetComponent<TextMeshPro>().text);
                    int newPower1 = currentPower1 + numberPowerCounters;
                    powerObject1.GetComponent<TextMeshPro>().text = newPower1.ToString();
                    powerObject1.GetComponent<TextMeshPro>().fontStyle = FontStyles.Bold;
                    powerObject1.GetComponent<TextMeshPro>().color = new Color32(0, 90, 0, 255);
                    NextAnimation();
                }
                break;
            case ActingPlayer.Secondary:
                GameObject activeObject2 = activeCard2.transform.Find("Power Counters").gameObject;
                if (animationHelper.NotYetReachedDestination(ClashConstants.getPowerCounterEndingPoint(activeCard2), activeObject2))
                {
                    animationHelper.MoveTowardsPoint(ClashConstants.getPowerCounterEndingPoint(activeCard2), ClashConstants.getPowerCounterStartingPoint(activeCard2), ClashConstants.powerCounterAnimationTime, activeObject2);
                }
                else
                {
                    GameObject numberCountersObject2 = activeCard2.transform.Find("Power Counters").Find("Number Counters").gameObject;
                    int currentPowerCounters2 = Int32.Parse(numberCountersObject2.GetComponent<TextMeshPro>().text);
                    int newPowerCounters2 = currentPowerCounters2 + numberPowerCounters;
                    numberCountersObject2.GetComponent<TextMeshPro>().text = newPowerCounters2.ToString();
                    GameObject powerObject2 = activeCard2.transform.Find("Power").gameObject;
                    int currentPower2 = Int32.Parse(powerObject2.GetComponent<TextMeshPro>().text);
                    int newPower2 = currentPower2 + numberPowerCounters;
                    powerObject2.GetComponent<TextMeshPro>().text = newPower2.ToString();
                    powerObject2.GetComponent<TextMeshPro>().fontStyle = FontStyles.Bold;
                    powerObject2.GetComponent<TextMeshPro>().color = new Color32(0, 90, 0, 255);
                    NextAnimation();
                }
                break;
        }
    }
    void CycleCardAnimation()
    {
        switch (clashAnimationQueue.getCurrentActingPlayer())
        {
            case ActingPlayer.Primary:
                
                if (animationHelper.NotYetReachedDestination(ClashConstants.player1Deck, activeCard1) ||
                    animationHelper.NotYetReachedScale(ClashConstants.defaultCardScale, activeCard1))
                {
                    animationHelper.ScaleTowardsSize(ClashConstants.defaultCardScale, ClashConstants.viewCardScaleSize, ClashConstants.viewCardAnimationTime, activeCard1);

                    animationHelper.MoveTowardsPoint(ClashConstants.player1Deck, ClashConstants.player1ViewCard,ClashConstants.viewCardAnimationTime, activeCard1);
                }
                else
                {
                    player1.putCardOnBottomOfDeck();
                    activeCard1.GetComponent<SortingGroup>().sortingLayerName = "Cards";
                    NextAnimation();
                }
                break;
            case ActingPlayer.Secondary:
                if (animationHelper.NotYetReachedDestination(ClashConstants.player2Deck, activeCard2) ||
                    animationHelper.NotYetReachedScale(ClashConstants.defaultCardScale, activeCard2))
                {
                    animationHelper.ScaleTowardsSize(ClashConstants.defaultCardScale, ClashConstants.viewCardScaleSize, ClashConstants.viewCardAnimationTime, activeCard2);

                    animationHelper.MoveTowardsPoint(ClashConstants.player2Deck, ClashConstants.player2ViewCard, ClashConstants.viewCardAnimationTime, activeCard2);
                }
                else
                {
                    player2.putCardOnBottomOfDeck();
                    activeCard2.GetComponent<SortingGroup>().sortingLayerName = "Cards";
                    NextAnimation();
                }
                break;
            default:
                NextAnimation();
                break;
        }
    }

    void resetPower(GameObject card)
    {
        GameObject powerCountersObject = card.transform.Find("Power Counters").gameObject;
        powerCountersObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
        GameObject numberCountersObject = powerCountersObject.transform.Find("Number Counters").gameObject;
        numberCountersObject.GetComponent<SortingGroup>().sortingOrder = 0;
        int numberPowerCounters = Int32.Parse(numberCountersObject.GetComponent<TextMeshPro>().text);
        numberCountersObject.GetComponent<TextMeshPro>().text = "0";
        GameObject powerObject = card.transform.Find("Power").gameObject;
        int currentPower = Int32.Parse(powerObject.GetComponent<TextMeshPro>().text);
        int newPower = currentPower - numberPowerCounters;
        powerObject.GetComponent<TextMeshPro>().text = newPower.ToString();
        powerObject.GetComponent<TextMeshPro>().fontStyle = FontStyles.Normal;
        powerObject.GetComponent<TextMeshPro>().color = Color.black;
    }

    void updateDeckNumbers()
    {
        //make deck numbers correct
        GameObject.Find("Deck1/NumberCards").GetComponent<TextMeshPro>().text = player1.numberOfCardsInDeck().ToString();
        GameObject.Find("Deck2/NumberCards").GetComponent<TextMeshPro>().text = player2.numberOfCardsInDeck().ToString();
    }

    void NextAnimation()
    {
        updateDeckNumbers();
        clashAnimationQueue.nextAnimation();
    }
}
