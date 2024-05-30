using System;
using UnityEngine;

public class CardAnimation
{
    private CardAnimationType animationType;
    private int animationInteger = 0;
    private String animationString = "";
    private Vector3 startingPoint;
    private Vector3 endingPoint;


    public CardAnimation(CardAnimationType animationType)
    {
        this.animationType = animationType;
    }

    public CardAnimationType getAnimationType()
    {
        return this.animationType;
    }

    public CardAnimation withAnimationInteger(int animationInteger)
    {
        this.animationInteger = animationInteger;
        return this;
    }

    public int getAnimationInteger()
    {
        return this.animationInteger;
    }

    public CardAnimation withAnimationString(String animationString)
    {
        this.animationString = animationString;
        return this;
    }

    public String getAnimationString()
    {
        return this.animationString;
    }

    public CardAnimation withStartingPoint(Vector3 startingPoint)
    {
        this.startingPoint = startingPoint;
        return this;
    }

    public Vector3 getStartingPoint()
    {
        return this.startingPoint;
    }

    public CardAnimation withEndingPoint(Vector3 endingPoint)
    {
        this.endingPoint = endingPoint;
        return this;
    }

    public Vector3 getEndingPoint()
    {
        return this.endingPoint;
    }
}
