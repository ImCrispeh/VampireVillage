using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class FortifiedWalls : Technology, IPointerEnterHandler, IPointerExitHandler {

    public Image unresearchedImage;
    public Image connectingBar;
    public Image connectingBar2;
    public GameObject technologyObject;
    public Transform technologyPosition;
    public Technology requiredTechnology;   //add more if you need more than one pre-requiste

    // Use this for initialization
    protected override void Start () {
        base.Start();
        technologyName = "Fortified Walls";
        technologyDescription = "Defense + 1" + "\n" + "Your stone walls are fortified increasing their effectiveness";
        researchRequirement = "Stone Walls";
        woodCost = 20;
        stoneCost = 80;
        goldCost = 10;
        researchTime = 40f; 
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
        Debug.Log("Added " + technologyName + " to the town");
        technologyPosition.Find(requiredTechnology.technologyName).gameObject.name = technologyName;
    }

    public override void StartResearch() {
        if (!researched && !researching && requiredTechnology.researched) {
            if (resources.wood >= woodCost && resources.stone >= stoneCost && resources.gold >= goldCost) {
                researchTimer = 0;
                researching = true;
                resources.SubtractWood(woodCost);
                resources.SubtractStone(stoneCost);
                resources.SubtractGold(goldCost);
                resources.UpdateResourceText();
                Debug.Log("Researching: " + technologyName);
            }
        }
    }

    public override void EndResearch() {
        TechnologyEffect();
        Debug.Log("Researched: " + technologyName);
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
