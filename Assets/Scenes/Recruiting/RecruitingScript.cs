using System;
using System.Collections.Generic;
using UnityEngine;

public class RecruitingScript : MonoBehaviour
{
    public GameObject crewCardPrefab;
    private DraftMachine draftMachine;
    // Start is called before the first frame update
    void Start()
    {
        draftMachine = new DraftMachine();
        List<CrewCard> firstRecruitingHand = draftMachine.newRecruitingHand();

        crewCardPrefab.GetComponent<CrewCardScript>().crewCard = firstRecruitingHand[0];
        GameObject card1 = Instantiate(
                    crewCardPrefab,
                    new Vector3(-5.2f, 2.2f, transform.position.z),
                    transform.rotation);

        crewCardPrefab.GetComponent<CrewCardScript>().crewCard = firstRecruitingHand[1];
        GameObject card2 = Instantiate(
                    crewCardPrefab,
                    new Vector3(-2f, 2.2f, transform.position.z),
                    transform.rotation);

        crewCardPrefab.GetComponent<CrewCardScript>().crewCard = firstRecruitingHand[2];
        GameObject card3 = Instantiate(
                    crewCardPrefab,
                    new Vector3(1.2f, 2.2f, transform.position.z),
                    transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
