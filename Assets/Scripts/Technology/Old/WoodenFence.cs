using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class WoodenFence : Technology, IPointerEnterHandler, IPointerExitHandler {

    public Image unresearchedImage;
    public Image connectingBar;
    public GameObject technologyObject;
    public Transform technologyPosition;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        technologyName = "Wooden Fence";
        technologyDescription = "A wooden fence is constructed adding to your defenses";
        researchCost = 50; //This will need to be changed once we discuss resources
        researchTime = 10f; //10 secs - currently not linked to the timer
        researchTimer = researchTime;
        researched = false;
        researching = false;
        applyTechnology = false;
        technologyImage = unresearchedImage;
        proceedingTechnologyBar = connectingBar;
        mainBase = BaseController._instance;
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
        if (researched && !researching && !applyTechnology) {
            EndResearch();
            applyTechnology = true;
        }
	}

    public override void TechnologyEffect() {
        //The effects of the technology which are active once research ends
        //mainBase.defense += 3;
        Debug.Log("Added " + technologyName + " to the town");
        //Instantiate(technologyObject, technologyPosition);
    }

    public override void StartResearch() {

        // Since it's the first technology, check if tutorial is running or
        // if it has been skipped (TO IMPLEMENT LATER)
        if (!researched && !researching) {
            researchTimer = 0;
            researching = true;
            /*if (ResourceStorage._instance.wood >= researchCost) {
                if (TutorialController._tutInstance != null) {
                    Timer._instance.UnpauseTimer();
                }

                researchTimer = 0;
                researching = true;
                ResourceStorage._instance.SubtractWood(researchCost);
                ResourceStorage._instance.UpdateResourceText();
                Debug.Log("Researching: " + technologyName);
            } else {
                ErrorController._instance.SetErrorText("Not enough resources available");
            }*/
        }
    }

    public override void EndResearch() {
        if (TutorialController._tutInstance != null) {
            TutorialController._tutInstance.ChangeText();
        }

        TechnologyEffect();
        Debug.Log("Researching: " + technologyName);
    }

    public override void OnPointerEnter(PointerEventData pointer) {
        Debug.Log("Mouse is over: " + technologyName);
    }

    public override void OnPointerExit(PointerEventData pointer) {
        Debug.Log("Mouse is not over: " + technologyName + " anymore");
    }
}
