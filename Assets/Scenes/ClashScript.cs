using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClashScript : MonoBehaviour
{
    public GameObject crewCardPrefab;
    public List<CrewCard> crewCardList;

    private List<CrewCard> deck1;
    private List<CrewCard> deck2;

    private Queue<ClashAnimation> animationQueue;

    // Start is called before the first frame update
    void Start()
    {
        crewCardPrefab.GetComponent<CrewCardScript>().crewCard = crewCardList[0];
        Instantiate(
            crewCardPrefab,
            new Vector3(-7f, 2.25f, transform.position.z),
            transform.rotation);

        crewCardPrefab.GetComponent<CrewCardScript>().crewCard = crewCardList[1];
        Instantiate(
            crewCardPrefab,
            new Vector3(4.2f, -2.25f, transform.position.z),
            transform.rotation);

        crewCardPrefab.GetComponent<CrewCardScript>().crewCard = crewCardList[2];
        Instantiate(
            crewCardPrefab,
            new Vector3(1.4f, 2.25f, transform.position.z),
            transform.rotation);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
