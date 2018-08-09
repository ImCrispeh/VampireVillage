using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Fortifications : Technology {

    public Image unresearchedImage;
    public Technology requiredTechnology;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        technologyName = "Fortifications";
        technologyDescription = "Fortifications are added to your stone walls, adding to your defenses";
        researchCost = 20f; //This will need to be changed once we discuss resources
        researchTime = 45f; //45 secs - currently not linked to the timer
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
        //something like Town.Defense += 25;
        Debug.Log("Added " + technologyName + " to the town");
    }

    public override void StartResearch() {
        if (!researched && !researching && requiredTechnology.researched) {
            researchTimer = 0;
            researching = true;
            Debug.Log("Researching: " + technologyName);
        }
    }

    public override void EndResearch() {
        TechnologyEffect();
        Debug.Log("Finished researching: " + technologyName);
    }
}
