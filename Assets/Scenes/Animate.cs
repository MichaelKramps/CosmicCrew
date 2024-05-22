using System;
using UnityEngine;

public static class Animate
{
    public static int cardMovementTime = 350;

    public static bool moveTowardsPoint(Vector3 destination, Vector3 startingPoint, float timeInMilliseconds, GameObject objectToMove)
    {
        if (Animate.notYetReachedDestination(destination, objectToMove))
        {
            float currentX = objectToMove.transform.position.x;
            float currentY = objectToMove.transform.position.y;

            float amountToChangeX = ((destination.x - startingPoint.x) / (timeInMilliseconds / 1000f)) * Time.deltaTime;
            float amountToChangeY = ((destination.y - startingPoint.y) / (timeInMilliseconds / 1000f)) * Time.deltaTime;

            float newX = currentX + amountToChangeX;
            float newY = currentY + amountToChangeY;


            if ((destination.x > startingPoint.x && newX > destination.x) || (destination.x < startingPoint.x && newX < destination.x))
            {
                newX = destination.x;
            }

            if ((destination.y > startingPoint.y && newY > destination.y) || (destination.y < startingPoint.y && newY < destination.y))
            {
                newY = destination.y;
            }

            objectToMove.transform.position = new Vector3(newX, newY, objectToMove.transform.position.z);
            //still moving towards point
            return true;
        }
        else
        {
            objectToMove.transform.position = destination;
            //no longer moving towards point
            return false;
        }

    }

    public static bool notYetReachedDestination(Vector3 destination, GameObject objectToMove)
    {
        return objectToMove.transform.position.x != destination.x || objectToMove.transform.position.y != destination.y;
    }

    public static bool scaleTowardsSize(float scaleDestination, float scaleStartingPoint, float timeInMilliseconds, GameObject objectToScale)
    {
        if (Animate.notYetReachedScale(scaleDestination, objectToScale))
        {
            float difference = scaleDestination - scaleStartingPoint; //negative if shrinking ex. 0.2
            float addToCurrentScale = difference * Time.deltaTime / (timeInMilliseconds / 1000f);

            float newScaleValue = objectToScale.transform.localScale.x + addToCurrentScale;

            if (scaleDestination > scaleStartingPoint && newScaleValue > scaleDestination)
            {
                newScaleValue = scaleDestination;
            }
            else if (scaleDestination < scaleStartingPoint && newScaleValue < scaleDestination)
            {
                newScaleValue = scaleDestination;
            }

            objectToScale.transform.localScale = new Vector3(newScaleValue, newScaleValue);
            //still scaling towards size
            return true;
        }
        //no longer scaling towards size
        return false;

    }

    public static bool notYetReachedScale(float destination, GameObject objectToMove)
    {
        return objectToMove.transform.localScale.x != destination;
    }
}
