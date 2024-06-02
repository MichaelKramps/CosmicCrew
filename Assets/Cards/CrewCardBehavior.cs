using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;

public class CrewCardBehavior : MonoBehaviour
{
    static float defaultCardScale = 0.3f;

    private List<CardAnimation> animationQueue = new List<CardAnimation>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (animationQueue.Count > 0)
        {
            this.animate();
        }
    }

    private void animate()
    {
        switch (this.currentAnimation().getAnimationType())
        {
            case CardAnimationType.MOVE_CARD:
                animateMove();
                break;
            case CardAnimationType.SWAY_COUNTERS:
                animateSwayCounters();
                break;
            case CardAnimationType.DRAW_CARD:
                animateDrawCard();
                break;
            case CardAnimationType.CYCLE_CARD:
                animateCycleCard();
                break;
        }
    }

    private CardAnimation currentAnimation()
    {
        return animationQueue[0];
    }

    public void animateMove()
    {
        if(!Animate.moveTowardsPoint(this.currentAnimation().getEndingPoint(), this.currentAnimation().getStartingPoint(), this.currentAnimation().getDuration(), gameObject))
        {
            nextAnimation();
        }
    }

    public void moveTo(Vector3 whereToMove, int timeInMilliseconds)
    {
        Vector3 currentCardPosition = gameObject.GetComponent<Transform>().position;
        CardAnimation moveAnimation = new CardAnimation(CardAnimationType.MOVE_CARD)
            .withDuration(timeInMilliseconds)
            .withStartingPoint(currentCardPosition)
            .withEndingPoint(whereToMove);

        //moveTo always gets added to the front of the animation queue
        //because it takes precedence over any other animation
        //It must also overwrite any other movement animations in the queue
        this.removeMoveCardAnimationsFromQueue();
        this.animationQueue.Insert(0, moveAnimation);
        FaceOffGlobalInformation.objectAnimating(gameObject.GetInstanceID());
        printAnimationQueue();
    }

    private void animateSwayCounters()
    {
        if (!Animate.moveTowardsPointAlongY(this.currentAnimation().getEndingPoint(), this.currentAnimation().getStartingPoint(), 750f, gameObject.transform.Find("Power Counters").gameObject))
        {
            //handle ending of sway counter animation
            GameObject numberCountersObject = gameObject.transform.Find("Power Counters").Find("Number Counters").gameObject;
            numberCountersObject.GetComponent<TextMeshPro>().text = this.currentAnimation().getAnimationString();
            GameObject powerObject = gameObject.transform.Find("Power").gameObject;
            int newPowerCounters = gameObject.GetComponent<CrewCardScript>().crewCard.power + this.currentAnimation().getAnimationInteger();
            powerObject.GetComponent<TextMeshPro>().text = newPowerCounters.ToString();
            if (newPowerCounters > 0)
            {
                powerObject.GetComponent<TextMeshPro>().fontStyle = FontStyles.Bold;
                powerObject.GetComponent<TextMeshPro>().color = new Color32(0, 45, 0, 255);
            }
            else
            {
                GameObject powerCountersObject = gameObject.transform.Find("Power Counters").gameObject;
                SpriteRenderer powerCountersSprite = powerCountersObject.GetComponent<SpriteRenderer>();
                powerCountersSprite.sortingOrder = 0;
                powerCountersObject.transform.Find("Number Counters").gameObject.GetComponent<SortingGroup>().sortingOrder = 0;
                powerObject.GetComponent<TextMeshPro>().fontStyle = FontStyles.Normal;
                powerObject.GetComponent<TextMeshPro>().color = new Color32(0, 0, 0, 255);
            }
            nextAnimation();
        }
    }

    public void addSwayCounters(int newTotalSwayCounters)
    {
        Vector3 currentCardPosition = gameObject.GetComponent<Transform>().position;
        CardAnimation swayCounterAnimation = new CardAnimation(CardAnimationType.SWAY_COUNTERS)
            .withAnimationString(newTotalSwayCounters.ToString())
            .withAnimationInteger(newTotalSwayCounters)
            .withStartingPoint(new Vector3(currentCardPosition.x, currentCardPosition.y))
            .withEndingPoint(new Vector3(currentCardPosition.x, currentCardPosition.y + 0.55f));

        addAnimation(swayCounterAnimation);

        //set up start of sway counter animation
        GameObject powerCountersObject = gameObject.transform.Find("Power Counters").gameObject;
        SpriteRenderer powerCountersSprite = powerCountersObject.GetComponent<SpriteRenderer>();
        powerCountersSprite.sortingOrder = 1;
        powerCountersSprite.transform.position = swayCounterAnimation.getStartingPoint();
        powerCountersObject.transform.Find("Number Counters").gameObject.GetComponent<SortingGroup>().sortingOrder = 2;
    }

    public void animateDrawCard()
    {
        if (!Animate.moveTowardsPointAlongX(this.currentAnimation().getEndingPoint(), this.currentAnimation().getStartingPoint(), 750f, gameObject) ||
            !Animate.scaleTowardsSize(defaultCardScale * 1.5f, defaultCardScale, 750f, gameObject))
        {
            nextAnimation();
        }
    }

    public void animateCycleCard()
    {
        if (!Animate.moveTowardsPointAlongX(this.currentAnimation().getEndingPoint(), this.currentAnimation().getStartingPoint(), 750f, gameObject) ||
            !Animate.scaleTowardsSize(defaultCardScale, defaultCardScale * 1.5f, 750f, gameObject))
        {
            gameObject.GetComponent<SortingGroup>().sortingLayerName = "Cards";
            nextAnimation();
        }
    }

    public void cycleCard()
    {
        Vector3 currentCardPosition = gameObject.GetComponent<Transform>().position;
        Vector3 showcasePosition = new Vector3(currentCardPosition.x - 1.5f, currentCardPosition.y);
        CardAnimation drawAnimation = new CardAnimation(CardAnimationType.DRAW_CARD)
            .withStartingPoint(currentCardPosition)
            .withEndingPoint(showcasePosition);
        CardAnimation cycleAnimation = new CardAnimation(CardAnimationType.CYCLE_CARD)
            .withStartingPoint(showcasePosition)
            .withEndingPoint(currentCardPosition);

        addAnimation(drawAnimation);
        addAnimation(cycleAnimation);

        //set up draw animation
        gameObject.GetComponent<SortingGroup>().sortingLayerName = "Featured";
    }

    private void removeMoveCardAnimationsFromQueue()
    {
        List<int> indexesToRemove = new List<int>();
        for(int index = 0; index < this.animationQueue.Count; index++)
        {
            CardAnimation animation = this.animationQueue[index];
            if (animation.getAnimationType() == CardAnimationType.MOVE_CARD)
            {
                indexesToRemove.Add(index);
            }
        }

        foreach(int index in indexesToRemove)
        {
            this.animationQueue.RemoveAt(index);
        }
    }

    public void addAnimation(CardAnimation animation)
    {
        this.animationQueue.Add(animation);
        FaceOffGlobalInformation.objectAnimating(gameObject.GetInstanceID());
    }

    public void nextAnimation()
    {
        this.animationQueue.RemoveAt(0);
        this.recalibrateCurrentAnimation();
        if (this.animationQueue.Count == 0)
        {
            FaceOffGlobalInformation.objectDoneAnimating(gameObject.GetInstanceID());
        }
    }

    public void recalibrateCurrentAnimation()
    {
        if (this.animationQueue.Count > 0)
        {
            switch (this.currentAnimation().getAnimationType())
            {
                case CardAnimationType.SWAY_COUNTERS:
                    //need to reset starting/ending points to current card position
                    Vector3 currentCardPosition = gameObject.GetComponent<Transform>().position;
                    this.currentAnimation()
                        .withStartingPoint(new Vector3(currentCardPosition.x, currentCardPosition.y))
                        .withEndingPoint(new Vector3(currentCardPosition.x, currentCardPosition.y + 0.55f)); ;
                    break;
                case CardAnimationType.DRAW_CARD:
                    //put card back on top of deck
                    gameObject.GetComponent<SortingGroup>().sortingLayerName = "Featured";
                    break;
            }
        }
    }

    private void printAnimationQueue()
    {
        Debug.Log(gameObject.GetComponent<CrewCardScript>().crewCard.cardName);
        string queueOutput = "";
        foreach (CardAnimation animation in this.animationQueue)
        {
            queueOutput += animation.getAnimationType();
            queueOutput += " ";
        }
        Debug.Log(queueOutput);
    }
}
