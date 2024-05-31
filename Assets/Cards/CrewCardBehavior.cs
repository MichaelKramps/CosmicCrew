using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;

public class CrewCardBehavior : MonoBehaviour
{
    private bool moving;
    private Vector3 startingPoint;
    private Vector3 endingPoint;
    private int timeInMilliseconds;

    private Queue<CardAnimation> animationQueue = new Queue<CardAnimation>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            this.moving = Animate.moveTowardsPoint(this.endingPoint, this.startingPoint, this.timeInMilliseconds, gameObject);
        }

        if (animationQueue.Count > 0)
        {
            this.animate();
        }
    }

    public void moveTo(Vector3 whereToMove, int timeInMilliseconds)
    {
        this.startingPoint = gameObject.GetComponent<Transform>().position;
        this.endingPoint = whereToMove;
        this.timeInMilliseconds = timeInMilliseconds;
        this.moving = true;
    }

    private void animate()
    {
        switch (this.currentAnimation().getAnimationType())
        {
            case CardAnimationType.SWAY_COUNTERS:
                animateSwayCounters();
                break;
        }
    }

    private CardAnimation currentAnimation()
    {
        return animationQueue.Peek();
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

            animationQueue.Dequeue();
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

        this.animationQueue.Enqueue(swayCounterAnimation);

        //set up start of sway counter animation
        GameObject powerCountersObject = gameObject.transform.Find("Power Counters").gameObject;
        SpriteRenderer powerCountersSprite = powerCountersObject.GetComponent<SpriteRenderer>();
        powerCountersSprite.sortingOrder = 1;
        powerCountersSprite.transform.position = swayCounterAnimation.getStartingPoint();
        powerCountersObject.transform.Find("Number Counters").gameObject.GetComponent<SortingGroup>().sortingOrder = 2;
    }
}
