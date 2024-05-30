using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RecruitingScript : MonoBehaviour
{
    public GameObject crewCardPrefab;
    public GameObject minimizedCardPrefab;

    public List<GameObject> faceOffDeckSummaries;

    private GameObject card1;
    private GameObject card2;
    private GameObject card3;
    private GameObject shownCardFromSummary;

    // Start is called before the first frame update
    void Start()
    {
        if (FandomForge.currentRecruitingAction != CurrentRecruitingAction.Recruiting)
        {
            if (FandomForge.currentRecruitingAction == CurrentRecruitingAction.Dismiss && FandomForge.selectedCrewCard != null)
            {
                completeCardDismiss();
            }
            FandomForge.selectedCrewCard = null;
            FandomForge.currentRecruitingAction = CurrentRecruitingAction.Recruiting;
        } else
        {
            FandomForge.newRecruitingHand();
        }
        showRecruitingHand();
        updatePlayerInformation();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.transform.gameObject.tag == "CardSummaryBox")
            {
                showCardFromSummary(hit.transform.gameObject);
            }
            //Check for mouse click 
            if (Input.GetMouseButtonDown(0))
            {
                handleClick(hit.transform.gameObject);
                updatePlayerInformation();
            }
        } else
        {
            if (shownCardFromSummary != null)
            {
                Destroy(shownCardFromSummary);
            }
        }

        
    }

    private void handleClick(GameObject gameObject)
    {
        switch (gameObject.tag)
        {
            case "RecruitCardOne":
                recruitCard(1);
                break;
            case "RecruitCardTwo":
                recruitCard(2);
                break;
            case "RecruitCardThree":
                recruitCard(3);
                break;
            case "Refresh":
                refresh();
                break;
            case "Place":
                place();
                break;
            case "Dismiss":
                dismiss();
                break;
            case "Invest":
                invest();
                break;
            case "FaceOff":
                SceneManager.LoadScene("FaceOff");
                break;
            default:
                break;
        }
        //updatePlayerInformation();
    }

    private void recruitCard(int whichCard)
    {
        bool succeeded = FandomForge.recruitCard(whichCard);
        if (succeeded)
        {
            removeCard(whichCard);
        }
    }

    private void refresh()
    {
        bool succeeded = FandomForge.refresh();
        if (succeeded)
        {
            showRecruitingHand();
        }
    }

    private void place()
    {
        
    }

    private void dismiss()
    {
        bool succeeded = FandomForge.dismissScreen();
        if (succeeded)
        {
            SceneManager.LoadScene("Cards In Deck");
        }
    }

    private void invest()
    {
        bool succeeded = FandomForge.invest();
    }

    private void removeCard(int whichCard)
    {
        switch (whichCard)
        {
            case 1:
                Destroy(card1);
                break;
            case 2:
                Destroy(card2);
                break;
            case 3:
                Destroy(card3);
                break;
            default:
                break;
        }
    }

    private void destroyRecruitingHand()
    {
        Destroy(card1);
        Destroy(card2);
        Destroy(card3);
    }

    private void showRecruitingHand()
    {
        destroyRecruitingHand();

        crewCardPrefab.GetComponent<CrewCardScript>().crewCard = FandomForge.currentRecruitingHand()[0];
        card1 = Instantiate(
                    crewCardPrefab,
                    new Vector3(-5.2f, 2.2f, transform.position.z),
                    transform.rotation);
        GameObject.Find("recruit-button-1/cost-text").GetComponent<TextMeshPro>().text = FandomForge.determineCostOfCard(FandomForge.currentRecruitingHand()[0]).ToString();

        crewCardPrefab.GetComponent<CrewCardScript>().crewCard = FandomForge.currentRecruitingHand()[1];
        card2 = Instantiate(
                    crewCardPrefab,
                    new Vector3(-2f, 2.2f, transform.position.z),
                    transform.rotation);
        GameObject.Find("recruit-button-2/cost-text").GetComponent<TextMeshPro>().text = FandomForge.determineCostOfCard(FandomForge.currentRecruitingHand()[1]).ToString();

        crewCardPrefab.GetComponent<CrewCardScript>().crewCard = FandomForge.currentRecruitingHand()[2];
        card3 = Instantiate(
                    crewCardPrefab,
                    new Vector3(1.2f, 2.2f, transform.position.z),
                    transform.rotation);
        GameObject.Find("recruit-button-3/cost-text").GetComponent<TextMeshPro>().text = FandomForge.determineCostOfCard(FandomForge.currentRecruitingHand()[2]).ToString();
    }

    private void updatePlayerInformation()
    {
        //update recruiting coins
        GameObject.Find("token-counter/remaining-tokens").GetComponent<TextMeshPro>().text = FandomForge.currentRecruitingTokens().ToString();
        GameObject.Find("token-counter/tokens-generated").GetComponent<TextMeshPro>().text = "$" + FandomForge.currentTokensGenerated().ToString() + " dividend";

        //delete old deck
        foreach (GameObject summaryGameObject in faceOffDeckSummaries)
        {
            Destroy(summaryGameObject);
        }
        faceOffDeckSummaries = new List<GameObject>();
        //make new deck
        int summaryItemIndex = 0;
        foreach (KeyValuePair<CrewCard, int> summaryItem in FandomForge.getFaceOffDeckSummary())
        {
            minimizedCardPrefab.GetComponent<CardSummaryScript>().crewCard = summaryItem.Key;
            GameObject thisSummaryGameObject = Instantiate(
                minimizedCardPrefab,
                new Vector3(5.05f, 3.7f - 0.55f * summaryItemIndex, transform.position.z),
                transform.rotation);
            thisSummaryGameObject.transform.Find("card-name").gameObject.GetComponent<TextMeshPro>().text = summaryItem.Key.cardName;
            thisSummaryGameObject.transform.Find("number-in-deck").gameObject.GetComponent<TextMeshPro>().text = summaryItem.Value.ToString();

            faceOffDeckSummaries.Add(thisSummaryGameObject);

            summaryItemIndex++;
        }
    }

    private void showCardFromSummary(GameObject cardSummaryObject)
    {
        CrewCard crewCardFromSummary = cardSummaryObject.GetComponent<CardSummaryScript>().crewCard;
        if (shownCardFromSummary != null && crewCardFromSummary == shownCardFromSummary.GetComponent<CrewCardScript>().crewCard)
        {
            return;
        }

        Destroy(shownCardFromSummary);
        crewCardPrefab.GetComponent<CrewCardScript>().crewCard = crewCardFromSummary;
        shownCardFromSummary = Instantiate(
                    crewCardPrefab,
                    new Vector3(6.5f, 0f, transform.position.z),
                    transform.rotation);
        shownCardFromSummary.GetComponent<Transform>().localScale = new Vector3(0.75f, 0.75f);
    }

    private void completeCardDismiss()
    {
        FandomForge.dismissCard(FandomForge.selectedCrewCard);
    }
}