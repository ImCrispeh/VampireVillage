using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WoodenFence : Technology {

    public Image unresearchedImage;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        technologyName = "Wooden Fence";
        technologyDescription = "A wooden fence is constructed adding to your defenses";
        researchCost = 20f; //This will need to be changed once we discuss resources
        researchTime = 15f; //15 secs - currently not linked to the timer
        researchTimer = researchTime;
        researched = false;
        researching = false;
        applyTechnology = false;
        technologyImage = unresearchedImage;
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
        Debug.Log("Added " + technologyName + " to the town");
    }

    public override void StartResearch() {
        if (!researched && !researching) {
            researchTimer = 0;
            researching = true;
            Debug.Log("Researching: " + technologyName);
        }
    }

    public override void EndResearch() {
        TechnologyEffect();
        Debug.Log("Researching: " + technologyName);
    }
}
