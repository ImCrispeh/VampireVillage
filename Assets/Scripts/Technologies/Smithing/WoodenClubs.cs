using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class WoodenClubs : Technology, IPointerEnterHandler, IPointerExitHandler
{

    public Image unresearchedImage;
    public Image connectingBar;
    public GameObject technologyObject;
    public Transform technologyPosition;

    // Use this for initialization
    protected override void Start() {
        base.Start();
        technologyName = "Wooden Clubs";
        technologyDescription = "You're able to pick the best sticks to use as clubs";
        researchRequirement = "";
        researchCost = 10;
        researchTime = 5f;
        researchTimer = researchTime;
        researched = false;
        researching = false;
        applyTechnology = false;
        technologyImage = unresearchedImage;
        proceedingTechnologyBar.Add(connectingBar);
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();
        if (researched && !researching && !applyTechnology)
        {
            EndResearch();
            applyTechnology = true;
        }
    }

    public override void TechnologyEffect() {
        //The effects of the technology which are active once research ends
        mainBase.attack += 1;
        Debug.Log("Added " + technologyName + " to the town");
        //Instantiate(technologyObject, technologyPosition);
    }

    public override void StartResearch() {
        if (!researched && !researching)
        {
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
        ttbName.text = technologyName;
        ttbResearchRequirement.text = researchRequirement;
        ttbDescription.text = technologyDescription;
        ttbCost.text = researchCost.ToString() + " wood";
        ttbResearchTime.text = researchTime.ToString() + " seconds (need to edit)";
    }

    public override void OnPointerExit(PointerEventData pointer) {
        ttbName.text = "";
        ttbResearchRequirement.text = "";
        ttbDescription.text = "";
        ttbCost.text = "";
        ttbResearchTime.text = "";
    }
}
