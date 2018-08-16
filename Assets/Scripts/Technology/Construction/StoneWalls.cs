using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class StoneWalls : Technology, IPointerEnterHandler, IPointerExitHandler {

    public Image unresearchedImage;
    public Image connectingBar;
    public GameObject technologyObject;
    public Transform technologyPosition;
    public Technology requiredTechnology;   //add more if you need more than one pre-requiste

    // Use this for initialization
    protected override void Start () {
        base.Start();
        technologyName = "Stone Walls";
        technologyDescription = "A stone wall is constructed adding to your defenses";
        researchRequirement = "Palisades";
        researchCost = 80; 
        researchTime = 5f; 
        researchTimer = researchTime;
        researched = false;
        researching = false;
        applyTechnology = false;
        technologyImage = unresearchedImage;
        proceedingTechnologyBar.Add(connectingBar);
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
        if (!researched && !researching && requiredTechnology.researched) {
            researchTimer = 0;
            researching = true;
            Debug.Log("Researching: " + technologyName);
        }
    }

    public override void EndResearch() {
        TechnologyEffect();
        Debug.Log("Researched: " + technologyName);
    }

    public override void OnPointerEnter(PointerEventData pointer) {
        //Debug.Log("Mouse has entered " + technologyName);
        ttbName.text = technologyName;
        ttbResearchRequirement.text = "Requirement: " + researchRequirement;
        ttbDescription.text = technologyDescription;
        ttbCost.text = researchCost.ToString() + " wood";
        ttbResearchTime.text = researchTime.ToString() + " seconds (need to edit)";
    }

    public override void OnPointerExit(PointerEventData pointer) {
        //Debug.Log("Mouse has exited " + technologyName);
        ttbName.text = "";
        ttbResearchRequirement.text = "";
        ttbDescription.text = "";
        ttbCost.text = "";
        ttbResearchTime.text = "";
    }
}
