using System;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class ChooseCardScript : MonoBehaviour
{
    public GameObject crewCardPrefab;
    // Use this for initialization
    void Start()
    {
        switch (FandomForge.currentRecruitingAction)
        {
            case CurrentRecruitingAction.Dismiss:
                GameObject.Find("SceneHelperText").GetComponent<TextMeshPro>().text = "Choose a card to Dismiss...";
                break;
            default:
                break;
        }

        //show cards you may select from
        int summaryItemIndex = 0;
        foreach (KeyValuePair<CrewCard, int> summaryItem in FandomForge.getFaceOffDeckSummary())
        {
            int row = (int) Math.Floor((double) summaryItemIndex / 6);
            int column = summaryItemIndex % 6;
            crewCardPrefab.GetComponent<CrewCardScript>().crewCard = summaryItem.Key;
            GameObject crewCardForSelection = Instantiate(
                crewCardPrefab,
                new Vector3(-7f + (column * 2.8f), 1.5f - (row * 4.3f), transform.position.z),
                transform.rotation);

            //faceOffDeckSummaries.Add(thisSummaryGameObject);

            summaryItemIndex++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.down);
            if (hit.collider != null && hit.transform.gameObject.tag == "Card")
            {
                FandomForge.selectedCrewCard = hit.transform.gameObject.GetComponent<CrewCardScript>().crewCard;
                SceneManager.LoadScene("Recruiting Phase");
            } else if (hit.collider != null && hit.transform.gameObject.tag == "Back")
            {
                SceneManager.LoadScene("Recruiting Phase");
            }
        }
    }
}
