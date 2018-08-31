using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public abstract class Technology : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public virtual string technologyName { get; set; }
    public virtual string technologyDescription { get; set; }
    public virtual string researchRequirement { get; set; }
    public virtual int woodCost { get; set; }
    public virtual int stoneCost { get; set; }
    public virtual int goldCost { get; set; }
    public virtual float researchTimer { get; set; }
    public virtual float researchTime { get; set; }
    public virtual bool researched { get; set; }
    public virtual bool researching { get; set; }
    public virtual bool applyTechnology { get; set; }
    public virtual Vector3 hiddenScale { get; set; }
    public virtual Vector3 shownScale { get; set; }

    public virtual Image technologyImage { get; set; }
    public virtual List<Image> proceedingTechnologyBar { get; set; }
    //ttb is tool tip box
    public virtual Text ttbName { get; set; }
    public virtual Text ttbResearchRequirement { get; set; }
    public virtual Text ttbDescription { get; set; }
    public virtual Text ttbWoodCost { get; set; }
    public virtual Text ttbStoneCost { get; set; }
    public virtual Text ttbGoldCost { get; set; }
    public virtual Text ttbResearchTime { get; set; }
    public virtual RectTransform ttbWoodIcon { get; set; }
    public virtual RectTransform ttbStoneIcon { get; set; }
    public virtual RectTransform ttbGoldIcon { get; set; }
    public virtual RectTransform ttbResearchTimeIcon { get; set; }
    public virtual BaseController mainBase { get; set; }
    public virtual ResourceStorage resources { get; set; }

    protected virtual void Start() {
        technologyName = "Placeholder name";
        technologyDescription = "Placeholder description";
        researchRequirement = "Placeholder research requirement";
        researchTimer = 0;
        researchTime = 0;
        woodCost = 0;
        stoneCost = 0;
        goldCost = 0;
        researched = false;
        researching = false;
        applyTechnology = false;
        proceedingTechnologyBar = new List<Image>();
        mainBase = BaseController._instance;
        resources = ResourceStorage._instance;
        hiddenScale = new Vector3(0, 0, 0);
        shownScale = new Vector3(1, 1, 1);

        ttbName = GameObject.Find("TechnologyWindow/TooltipBox/TechnologyName").GetComponent<Text>();
        ttbResearchRequirement = GameObject.Find("TechnologyWindow/TooltipBox/ResearchRequirement").GetComponent<Text>();
        ttbDescription = GameObject.Find("TechnologyWindow/TooltipBox/TechnologyDescription").GetComponent<Text>();
        ttbWoodCost = GameObject.Find("TechnologyWindow/TooltipBox/TechnologyWoodCost").GetComponent<Text>();
        ttbStoneCost = GameObject.Find("TechnologyWindow/TooltipBox/TechnologyStoneCost").GetComponent<Text>();
        ttbGoldCost = GameObject.Find("TechnologyWindow/TooltipBox/TechnologyGoldCost").GetComponent<Text>();
        ttbResearchTime = GameObject.Find("TechnologyWindow/TooltipBox/ResearchTime").GetComponent<Text>();
        ttbWoodIcon = GameObject.Find("TechnologyWindow/TooltipBox/TechnologyWoodCost/Icon").GetComponent<RectTransform>();
        ttbStoneIcon = GameObject.Find("TechnologyWindow/TooltipBox/TechnologyStoneCost/Icon").GetComponent<RectTransform>();
        ttbGoldIcon = GameObject.Find("TechnologyWindow/TooltipBox/TechnologyGoldCost/Icon").GetComponent<RectTransform>();
        ttbResearchTimeIcon = GameObject.Find("TechnologyWindow/TooltipBox/ResearchTime/Icon").GetComponent<RectTransform>();
        ttbWoodIcon.localScale = hiddenScale;
        ttbStoneIcon.localScale = hiddenScale;
        ttbGoldIcon.localScale = hiddenScale;
        ttbResearchTimeIcon.localScale = hiddenScale;
    }
    protected virtual void Update() {
        researchTimer += Time.deltaTime;
        if (researchTimer > researchTime && !researching && !researched) {
            technologyImage.fillAmount = 1;
            //proceedingTechnologyBar.fillAmount = 1;
            foreach (Image unfilled in proceedingTechnologyBar) {
                if (unfilled != null) {
                    unfilled.fillAmount = 1;
                }
            }
        }
        else {
            technologyImage.fillAmount = ((researchTime - researchTimer) / researchTime);
            if (researchTimer < researchTime) { //stops you from clicking again and resetting the research
                researching = true;
                
            }
            else {
                researching = false;
                researched = true;
                //proceedingTechnologyBar.fillAmount = 0;
                foreach (Image unfilled in proceedingTechnologyBar) {
                    if (unfilled != null) {
                        unfilled.fillAmount = 0;
                    }
                }
            }
        }
    }
    public abstract void TechnologyEffect();
    public abstract void StartResearch();
    public abstract void EndResearch();
    public abstract void OnPointerEnter(PointerEventData pointer);
    public abstract void OnPointerExit(PointerEventData pointer);
}
