using System;
using UnityEngine;

public class WaitTracker
{
    float currentWaitTime = 0;
    float waitUntil = 0;
    bool waiting = false;

    public WaitTracker()
    {
    }

    public void updateTime(float deltaTime)
    {
        if (this.currentWaitTime < waitUntil)
        {
            this.currentWaitTime += deltaTime * 1000f;
        }
        else
        {
            this.currentWaitTime = 0f;
            this.waiting = false;
        }
    }

    public void wait(int millisecondsToWait)
    {
        this.waiting = true;
        this.waitUntil = millisecondsToWait;
    }

    public bool isWaiting()
    {
        return waiting;
    }
}
