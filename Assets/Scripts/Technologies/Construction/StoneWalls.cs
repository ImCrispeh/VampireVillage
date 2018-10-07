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
        technologyDescription = "Defense + 2" + "\n" + "A stone wall is constructed adding to your defenses";
        researchRequirement = "Palisades";
        woodCost = 20;
        stoneCost = 50;
        goldCost = 0;
        researchTime = 15f; 
        researchTimer = researchTime;
        researched = false;
        researching = false;
        applyTechnology = false;
        technologyImage = unresearchedImage;
        proceedingTechnologyBar.Add(connectingBar);
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
        mainBase.defense += 2;
        tech.transform.SetParent(technologyPosition);
    }

    public override void StartResearch() {
        if (!researched && !researching && requiredTechnology.researched) {
            if (resources.wood >= woodCost && resources.stone >= stoneCost) {
                researchTimer = 0;
                researching = true;
                resources.SubtractWood(woodCost);
                resources.SubtractStone(stoneCost);
                resources.UpdateResourceText();
                Destroy(technologyPosition.Find(requiredTechnology.technologyName).gameObject);
                tech = Instantiate(technologyObject);
                finalBuiltPosition = tech.transform.position;
                tech.transform.position = new Vector3(tech.transform.position.x, tech.transform.position.y - tech.GetComponent<Collider>().bounds.size.y, tech.transform.position.z);
                startBuiltPosition = tech.transform.position;
                tech.name = technologyName;
            }
            else {
                NotEnoughResources();
            }
        }
        else {
            MissingPrerequisite();
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
