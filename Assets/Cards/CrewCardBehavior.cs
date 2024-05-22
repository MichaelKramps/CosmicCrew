using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewCardBehavior : MonoBehaviour
{
    private bool moving;
    private Vector3 startingPoint;
    private Vector3 endingPoint;
    private int timeInMilliseconds;

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
    }

    public void moveTo(Vector3 whereToMove, int timeInMilliseconds)
    {
        this.startingPoint = gameObject.GetComponent<Transform>().position;
        this.endingPoint = whereToMove;
        this.timeInMilliseconds = timeInMilliseconds;
        this.moving = true;
    }
}
