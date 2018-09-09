using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class Palisades : Technology, IPointerEnterHandler, IPointerExitHandler {

    public Image unresearchedImage;
    public Image connectingBar;
    public Image connectingBar2;
    public GameObject technologyObject;
    public Transform technologyPosition;
    public Technology requiredTechnology;   //add more if you need more than one pre-requiste

    // Use this for initialization
    protected override void Start () {
        base.Start();
        technologyName = "Palisades";
        technologyDescription = "Defense +1" + "\n" + "A palisade is constructed adding to your defenses";
        researchRequirement = "Picket Fence";
        woodCost = 20;
        stoneCost = 0;
        goldCost = 0;
        researchTime = 10f; 
        researchTimer = researchTime;
        researched = false;
        researching = false;
        applyTechnology = false;
        technologyImage = unresearchedImage;
        proceedingTechnologyBar.Add(connectingBar);
        proceedingTechnologyBar.Add(connectingBar2);
        technologyPosition = GameObject.Find(BaseController._instance.gameObject.name + "/Walls").transform;
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
        mainBase.defense += 1;
        Destroy(technologyPosition.Find(requiredTechnology.technologyName).gameObject);
        GameObject tech = Instantiate(technologyObject);
        tech.name = technologyName;
        tech.transform.SetParent(technologyPosition);
    }

    public override void StartResearch() {
        if (!researched && !researching && requiredTechnology.researched) {
            if (resources.wood >= woodCost) {
                researchTimer = 0;
                researching = true;
                resources.SubtractWood(woodCost);
                resources.UpdateResourceText();
            }            
        }
    }

    public override void EndResearch() {
        TechnologyEffect();
    }

    public override void OnPointerEnter(PointerEventData pointer) {
        ttbName.text = technologyName;
        ttbResearchRequirement.text = "Requirement: " + researchRequirement;
        ttbDescription.text = technologyDescription;
        ttbWoodCost.text = woodCost.ToString();
        ttbStoneCost.text = stoneCost.ToString();
        ttbGoldCost.text = goldCost.ToString();
        ttbResearchTime.text = researchTime.ToString() + " s";
        ttbWoodIcon.localScale = shownScale;
        ttbStoneIcon.localScale = shownScale;
        ttbGoldIcon.localScale = shownScale;
        ttbResearchTimeIcon.localScale = shownScale;
    }

    public override void OnPointerExit(PointerEventData pointer) {
        ttbName.text = "";
        ttbResearchRequirement.text = "";
        ttbDescription.text = "";
        ttbWoodCost.text = "";
        ttbStoneCost.text = "";
        ttbGoldCost.text = "";
        ttbResearchTime.text = "";
        ttbWoodIcon.localScale = hiddenScale;
        ttbStoneIcon.localScale = hiddenScale;
        ttbGoldIcon.localScale = hiddenScale;
        ttbResearchTimeIcon.localScale = hiddenScale;
    }
}
