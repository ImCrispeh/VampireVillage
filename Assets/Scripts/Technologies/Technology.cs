using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public abstract class Technology : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public virtual string technologyName { get; set; }
    public virtual string technologyDescription { get; set; }
    public virtual string researchRequirement { get; set; }
    public virtual int researchCost { get; set; } //this will need to be changed once we discuss resources
    public virtual float researchTimer { get; set; }
    public virtual float researchTime { get; set; }
    public virtual bool researched { get; set; }
    public virtual bool researching { get; set; }
    public virtual bool applyTechnology { get; set; }

    public virtual Image technologyImage { get; set; }
    public virtual List<Image> proceedingTechnologyBar { get; set; }
    //ttb is tool tip box
    public virtual Text ttbName { get; set; }
    public virtual Text ttbResearchRequirement { get; set; }
    public virtual Text ttbDescription { get; set; }
    public virtual Text ttbCost { get; set; }
    public virtual Text ttbResearchTime { get; set; }
    public virtual BaseController mainBase { get; set; }

    protected virtual void Start() {
        technologyName = "Placeholder name";
        technologyDescription = "Placeholder description";
        researchRequirement = "Placeholder research requirement";
        researchTimer = 0;
        researchTime = 0;
        researchCost = 0;
        researched = false;
        researching = false;
        applyTechnology = false;
        proceedingTechnologyBar = new List<Image>();
        mainBase = BaseController._instance;

        ttbName = GameObject.Find("TechnologyWindow/TooltipBox/TechnologyName").GetComponent<Text>();
        ttbResearchRequirement = GameObject.Find("TechnologyWindow/TooltipBox/ResearchRequirement").GetComponent<Text>();
        ttbDescription = GameObject.Find("TechnologyWindow/TooltipBox/TechnologyDescription").GetComponent<Text>();
        ttbCost = GameObject.Find("TechnologyWindow/TooltipBox/TechnologyCost").GetComponent<Text>();
        ttbResearchTime = GameObject.Find("TechnologyWindow/TooltipBox/ResearchTime").GetComponent<Text>();
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
