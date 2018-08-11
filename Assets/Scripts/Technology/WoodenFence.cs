using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WoodenFence : Technology {

    public Image unresearchedImage;
    public GameObject technologyObject;
    public Transform technologyPosition;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        technologyName = "Wooden Fence";
        technologyDescription = "A wooden fence is constructed adding to your defenses";
        researchCost = 50; //This will need to be changed once we discuss resources
        researchTime = 5f; //55 secs - currently not linked to the timer
        researchTimer = researchTime;
        researched = false;
        researching = false;
        applyTechnology = false;
        technologyImage = unresearchedImage;
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
        //something like Town.Defense += 5;
        mainBase.defense += 3;
        Debug.Log("Added " + technologyName + " to the town");
        Instantiate(technologyObject, technologyPosition);
    }

    public override void StartResearch() {

        // Since it's the first technology, check if tutorial is running or
        // if it has been skipped (TO IMPLEMENT LATER)
        if (!researched && !researching) {
            if (ResourceStorage._instance.wood > researchCost) {
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
            }
        }
    }

    public override void EndResearch() {
        if (TutorialController._tutInstance != null) {
            TutorialController._tutInstance.ChangeText();
        }

        TechnologyEffect();
        Debug.Log("Researching: " + technologyName);
    }
}
