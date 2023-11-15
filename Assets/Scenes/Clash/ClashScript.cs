using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClashScript : MonoBehaviour
{
    public GameObject crewCardPrefab;
    public List<CrewCard> crewCardList;

    private ClashPlayer player1;
    private ClashPlayer player2;

    private Queue<ClashAnimation> animationQueue;
    private ClashAnimation currentAnimation;

    // Start is called before the first frame update
    void Start()
    {
        // set player1 and player2
        // set animationQueue




        //crewCardPrefab.GetComponent<CrewCardScript>().crewCard = crewCardList[0];
        //Instantiate(
        //    crewCardPrefab,
        //    new Vector3(-7f, 2.25f, transform.position.z),
        //    transform.rotation);

    }

    // Update is called once per frame
    void Update()
    {
        switch (currentAnimation.getAnimationType())
        {
            case ClashAnimationType.DrawCard:
                DrawCardAnimation();
                break;
            default:
                NextAnimation();
                break;
        }
    }

    void DrawCardAnimation()
    {
        switch (currentAnimation.getActingPlayer())
        {
            case ActingPlayer.Primary:
                break;
            case ActingPlayer.Secondary:
                break;
            default:
                NextAnimation();
                break;
        }
    }

    void NextAnimation()
    {
        if (animationQueue.Count > 0)
        {
            currentAnimation = animationQueue.Dequeue();
        } else
        {
            //end fight animation
            currentAnimation = new ClashAnimation("x,end,0,0");
        }
    }
}
