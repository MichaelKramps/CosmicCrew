using System;
using System.Collections.Generic;

public class ClashAnimationQueue
{
    private Queue<ClashAnimation> animationQueue;

    public ClashAnimationQueue(string animationString)
    {
        animationQueue = createAnimationQueue(animationString);
    }

    private Queue<ClashAnimation> createAnimationQueue(string animationString)
    {
        Queue<ClashAnimation> newAnimationQueue = new Queue<ClashAnimation>();

        string[] animationStrings = animationString.Split("*");
        foreach (string thisAnimationString in animationStrings)
        {
            newAnimationQueue.Enqueue(new ClashAnimation(thisAnimationString));
        }
        return newAnimationQueue;
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
