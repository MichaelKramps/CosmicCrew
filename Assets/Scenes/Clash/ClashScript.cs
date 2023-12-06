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
        player1 = new ClashPlayer("21,23,22,23,22,23,24,18,21");
        player2 = new ClashPlayer("15,21,2,1,20,23,16,22,18,0,19,24,17,20");
        // set animationQueue
        // clashAnimationQueue = new ClashAnimationQueue("");
        clashAnimationQueue = new ClashAnimationQueue("p,dws,1,0*p,p,1,0*s,dws,1,0*s,p,1,0*s,dws,1,0*s,pow,3,1*s,uns,1,0*p,dws,1,0*p,p,2,0*s,dws,1,0*s,p,2,0*p,dws,1,0*p,p,3,0*p,pow,2,2*s,dws,1,0*s,p,3,0*p,dws,1,0*p,p,4,0*s,dws,1,0*s,p,4,0*p,dws,1,0*s,pow,1,1*s,pow,1,4*p,p,5,0*p,pow,2,1*s,dws,1,0*s,p,5,0*p,dws,1,0*s,pow,1,1*s,pow,1,5*p,p,6,0*s,dws,1,0*s,p,6,0*b,r,1,6*p,dws,1,0*s,pow,1,1*s,pow,1,5*p,p,1,0*b,gd,0,1*b,r,1,1*p,dws,1,0*s,pow,1,2*s,pow,1,5*p,p,1,0*b,gd,0,1*b,r,1,2*s,dws,1,0*s,p,2,0*s,pow,2,5*b,gd,1,0*b,r,5,4*s,dws,1,0*s,p,4,0*b,gd,1,0*b,r,1,2*p,dws,1,0*p,p,2,0*p,pow,4,2*s,dws,1,0*s,uns,1,0*b,gd,0,1*b,r,4,4*b,gd,1,1*b,r,2,1*p,dws,1,0*p,p,2,0*b,gd,0,1*b,r,3,4*s,dws,1,0*s,p,5,0*s,dws,1,0*s,uns,1,0*p,dws,1,0*p,uns,1,0*b,gd,1,0*b,r,4,4*s,dws,1,0*s,p,5,0*b,gd,1,0*b,r,4,6*p,dws,1,0*p,p,2,0*p,pow,2,2*b,gd,0,1*p,1w,0,0");
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
            case ClashAnimationType.ResetActiveToFighters:
                ResetActiveToFightersAnimation();
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
            case ClashAnimationType.GunnerDestinations:
                GunnerDestinationsAnimation();
                break;
            case ClashAnimationType.PreparePowerCounter:
                PreparePowerCounterAnimation();
                break;
            case ClashAnimationType.PowerCounter:
                PowerCounterAnimation();
                break;
            case ClashAnimationType.DestroyCard:
                DestroyCardAnimation();
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

    void ResetActiveToFightersAnimation()
    {
        activeCard1 = player1.getCurrentFighter();
        activeCard2 = player2.getCurrentFighter();
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

    void GunnerDestinationsAnimation()
    {
        Vector3 fighter1Destination = clashAnimationQueue.getCurrentAnimation().getPrimaryInteger() == 0 ? ClashConstants.player1Deck : ClashConstants.player1Discard;
        Vector3 fighter2Destination = clashAnimationQueue.getCurrentAnimation().getSecondaryInteger() == 0 ? ClashConstants.player2Deck : ClashConstants.player2Discard;

        if (animationHelper.NotYetReachedDestination(fighter1Destination, activeCard1) ||
            animationHelper.NotYetReachedDestination(fighter2Destination, activeCard2) ||
                    animationHelper.NotYetReachedScale(ClashConstants.defaultCardScale, activeCard1))
        {
            animationHelper.ScaleTowardsSize(ClashConstants.defaultCardScale, ClashConstants.fightCardScale, ClashConstants.viewCardAnimationTime, activeCard1);
            animationHelper.ScaleTowardsSize(ClashConstants.defaultCardScale, ClashConstants.fightCardScale, ClashConstants.viewCardAnimationTime, activeCard2);

            animationHelper.MoveTowardsPoint(fighter1Destination, ClashConstants.player1FightDestination, ClashConstants.viewCardAnimationTime, activeCard1);
            animationHelper.MoveTowardsPoint(fighter2Destination, ClashConstants.player2FightDestination, ClashConstants.viewCardAnimationTime, activeCard2);
        }
        else
        {
            resetPower(activeCard1);
            resetPower(activeCard2);
            activeCard1.GetComponent<SortingGroup>().sortingLayerName = "Cards";
            activeCard2.GetComponent<SortingGroup>().sortingLayerName = "Cards";
            if (clashAnimationQueue.getCurrentAnimation().getPrimaryInteger() == 0)
            {
                player1.winsFight();
            } else
            {
                player1.losesFight();
            }
            if (clashAnimationQueue.getCurrentAnimation().getSecondaryInteger() == 0)
            {
                player2.winsFight();
            }
            else
            {
                player2.losesFight();
            }
            NextAnimation();
        }
    }

    //void FighterTwoWinsAnimation()
    //{
    //    if (animationHelper.NotYetReachedDestination(ClashConstants.player1Discard, activeCard1) ||
    //        animationHelper.NotYetReachedDestination(ClashConstants.player2Deck, activeCard2) ||
    //                animationHelper.NotYetReachedScale(ClashConstants.defaultCardScale, activeCard1))
    //    {
    //        animationHelper.ScaleTowardsSize(ClashConstants.defaultCardScale, ClashConstants.fightCardScale, ClashConstants.viewCardAnimationTime, activeCard1);
    //        animationHelper.ScaleTowardsSize(ClashConstants.defaultCardScale, ClashConstants.fightCardScale, ClashConstants.viewCardAnimationTime, activeCard2);

    //        animationHelper.MoveTowardsPoint(ClashConstants.player1Discard, ClashConstants.player1FightDestination, ClashConstants.viewCardAnimationTime, activeCard1);
    //        animationHelper.MoveTowardsPoint(ClashConstants.player2Deck, ClashConstants.player2FightDestination, ClashConstants.viewCardAnimationTime, activeCard2);
    //    }
    //    else
    //    {
    //        resetPower(activeCard1);
    //        resetPower(activeCard2);
    //        activeCard1.GetComponent<SortingGroup>().sortingLayerName = "Cards";
    //        activeCard2.GetComponent<SortingGroup>().sortingLayerName = "Cards";
    //        player1.losesFight();
    //        player2.winsFight();
    //        NextAnimation();
    //    }
    //}

    //void FightersTieAnimation()
    //{
    //    if (animationHelper.NotYetReachedDestination(ClashConstants.player1Discard, activeCard1) ||
    //        animationHelper.NotYetReachedDestination(ClashConstants.player2Discard, activeCard2) ||
    //                animationHelper.NotYetReachedScale(ClashConstants.defaultCardScale, activeCard1))
    //    {
    //        animationHelper.ScaleTowardsSize(ClashConstants.defaultCardScale, ClashConstants.fightCardScale, ClashConstants.viewCardAnimationTime, activeCard1);
    //        animationHelper.ScaleTowardsSize(ClashConstants.defaultCardScale, ClashConstants.fightCardScale, ClashConstants.viewCardAnimationTime, activeCard2);

    //        animationHelper.MoveTowardsPoint(ClashConstants.player1Discard, ClashConstants.player1FightDestination, ClashConstants.viewCardAnimationTime, activeCard1);
    //        animationHelper.MoveTowardsPoint(ClashConstants.player2Discard, ClashConstants.player2FightDestination, ClashConstants.viewCardAnimationTime, activeCard2);
    //    }
    //    else
    //    {
    //        resetPower(activeCard1);
    //        resetPower(activeCard2);
    //        activeCard1.GetComponent<SortingGroup>().sortingLayerName = "Cards";
    //        activeCard2.GetComponent<SortingGroup>().sortingLayerName = "Cards";
    //        player1.losesFight();
    //        player2.losesFight();
    //        NextAnimation();
    //    }
    //}

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
                    int newPowerCounters1 = currentPowerCounters1 + numberPowerCounters >= 0 ? currentPowerCounters1 + numberPowerCounters : 0;
                    numberCountersObject1.GetComponent<TextMeshPro>().text = newPowerCounters1.ToString();
                    GameObject powerObject1 = activeCard1.transform.Find("Power").gameObject;
                    int currentPower1 = Int32.Parse(powerObject1.GetComponent<TextMeshPro>().text);
                    int newPower1 = currentPower1 + numberPowerCounters;
                    powerObject1.GetComponent<TextMeshPro>().text = newPower1.ToString();
                    if (newPowerCounters1 > 0)
                    {
                        powerObject1.GetComponent<TextMeshPro>().fontStyle = FontStyles.Bold;
                        powerObject1.GetComponent<TextMeshPro>().color = new Color32(0, 90, 0, 255);
                    } else
                    {
                        GameObject powerCountersObject1 = activeCard1.transform.Find("Power Counters").gameObject;
                        SpriteRenderer powerCountersSprite1 = powerCountersObject1.GetComponent<SpriteRenderer>();
                        powerCountersSprite1.sortingOrder = 0;
                        powerCountersObject1.transform.Find("Number Counters").gameObject.GetComponent<SortingGroup>().sortingOrder = 0;
                        powerObject1.GetComponent<TextMeshPro>().fontStyle = FontStyles.Normal;
                        powerObject1.GetComponent<TextMeshPro>().color = new Color32(0, 0, 0, 255);
                    }
                    
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
                    int newPowerCounters2 = currentPowerCounters2 + numberPowerCounters >= 0 ? currentPowerCounters2 + numberPowerCounters : 0;
                    numberCountersObject2.GetComponent<TextMeshPro>().text = newPowerCounters2.ToString();
                    GameObject powerObject2 = activeCard2.transform.Find("Power").gameObject;
                    int currentPower2 = Int32.Parse(powerObject2.GetComponent<TextMeshPro>().text);
                    int newPower2 = currentPower2 + numberPowerCounters;
                    powerObject2.GetComponent<TextMeshPro>().text = newPower2.ToString();
                    if (newPowerCounters2 > 0)
                    {
                        powerObject2.GetComponent<TextMeshPro>().fontStyle = FontStyles.Bold;
                        powerObject2.GetComponent<TextMeshPro>().color = new Color32(0, 90, 0, 255);
                    }
                    else
                    {
                        GameObject powerCountersObject2 = activeCard2.transform.Find("Power Counters").gameObject;
                        SpriteRenderer powerCountersSprite2 = powerCountersObject2.GetComponent<SpriteRenderer>();
                        powerCountersSprite2.sortingOrder = 0;
                        powerCountersObject2.transform.Find("Number Counters").gameObject.GetComponent<SortingGroup>().sortingOrder = 0;
                        powerObject2.GetComponent<TextMeshPro>().fontStyle = FontStyles.Normal;
                        powerObject2.GetComponent<TextMeshPro>().color = new Color32(0, 0, 0, 255);
                    }
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

    void DestroyCardAnimation()
    {
        int fighterNumber = clashAnimationQueue.getCurrentAnimation().getSecondaryInteger();
        switch (clashAnimationQueue.getCurrentActingPlayer())
        {
            case ActingPlayer.Primary:
                if (animationHelper.NotYetReachedDestination(ClashConstants.player1Discard, activeCard1))
                {
                    int fighterNumber1 = clashAnimationQueue.getCurrentAnimation().getSecondaryInteger();
                    animationHelper.MoveTowardsPoint(ClashConstants.player1Discard, ClashConstants.getPlayer1CardLocation(fighterNumber), ClashConstants.viewCardAnimationTime, activeCard1);
                }
                else
                {
                    resetPower(activeCard1);
                    activeCard1.GetComponent<SortingGroup>().sortingLayerName = "Cards";
                    player1.destroyFighter(clashAnimationQueue.getCurrentAnimation().getSecondaryInteger());
                    NextAnimation();
                }
                break;
            case ActingPlayer.Secondary:
                if (animationHelper.NotYetReachedDestination(ClashConstants.player2Discard, activeCard2))
                {
                    animationHelper.MoveTowardsPoint(ClashConstants.player2Discard, ClashConstants.getPlayer2CardLocation(fighterNumber), ClashConstants.viewCardAnimationTime, activeCard2);
                }
                else
                {
                    resetPower(activeCard2);
                    activeCard2.GetComponent<SortingGroup>().sortingLayerName = "Cards";
                    player2.destroyFighter(clashAnimationQueue.getCurrentAnimation().getSecondaryInteger());
                    NextAnimation();
                }
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
