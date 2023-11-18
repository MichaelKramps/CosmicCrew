using System;
using System.Collections.Generic;

public class ClashAnimationQueue
{
    private Queue<ClashAnimation> animationQueue;

    public ClashAnimationQueue(string animationString)
    {
        animationQueue = new Queue<ClashAnimation>();
        addAnimationsToQueue(animationString);
    }

    private void addAnimationsToQueue(string animationString)
    {
        string[] animationStrings = animationString.Split("*");
        foreach (string thisAnimationString in animationStrings)
        {
            addAnimationsFromCode(thisAnimationString);
        }
    }

    private void addAnimationsFromCode(string animationCodeString)
    {
        string[] animationCodes = animationCodeString.Split(",");
        ClashAnimation thisAnimation = new ClashAnimation(animationCodeString);

        switch (thisAnimation.getAnimationType())
        {
            case ClashAnimationType.DrawCard:
                animationQueue.Enqueue(new ClashAnimation(animationCodes[0] + ",gtc,0,0"));
                animationQueue.Enqueue(thisAnimation);
                break;
            case ClashAnimationType.RollDice:
                animationQueue.Enqueue(new ClashAnimation("b,sd,0,0"));
                animationQueue.Enqueue(new ClashAnimation("b,w,500,0"));
                animationQueue.Enqueue(thisAnimation);
                animationQueue.Enqueue(new ClashAnimation("b,w,750,0"));
                animationQueue.Enqueue(new ClashAnimation("b,hd,0,0"));
                animationQueue.Enqueue(new ClashAnimation("b,sf," + animationCodes[2] + "," + animationCodes[3]));
                animationQueue.Enqueue(new ClashAnimation("b,w,750,0"));
                break;
            case ClashAnimationType.FighterOneWins:
            case ClashAnimationType.FighterTwoWins:
            case ClashAnimationType.FightersTie:
                animationQueue.Enqueue(thisAnimation);
                animationQueue.Enqueue(new ClashAnimation("b,w,500,0"));
                break;
            case ClashAnimationType.PowerCounter:
                animationQueue.Enqueue(new ClashAnimation(animationCodes[0] + ",ppo," + animationCodes[2] + "," + animationCodes[3]));
                animationQueue.Enqueue(thisAnimation);
                break;
            default:
                animationQueue.Enqueue(thisAnimation);
                break;
        }
    }

    public ClashAnimation getCurrentAnimation()
    {
        return animationQueue.Peek();
    }

    public ClashAnimationType getCurrentAnimationType()
    {
        if (animationQueue.Count > 0)
        {
            return animationQueue.Peek().getAnimationType();
        } else
        {
            return ClashAnimationType.EndFight;
        }
        
    }

    public ActingPlayer getCurrentActingPlayer()
    {
        return animationQueue.Peek().getActingPlayer();
    }

    public void nextAnimation()
    {
        animationQueue.Dequeue();
        if (animationQueue.Count == 0)
        {
            animationQueue.Enqueue(new ClashAnimation("x,end,0,0"));
        }
        
    }
}
