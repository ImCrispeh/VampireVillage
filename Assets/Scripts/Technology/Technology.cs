using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public abstract class Technology : MonoBehaviour {
    public virtual string technologyName { get; set; }
    public virtual string technologyDescription { get; set; }
    public virtual int researchCost { get; set; } //this will need to be changed once we discuss resources
    public virtual float researchTimer { get; set; }
    public virtual float researchTime { get; set; }
    public virtual bool researched { get; set; }
    public virtual bool researching { get; set; }
    public virtual bool applyTechnology { get; set; }

    public virtual Image technologyImage { get; set; }
    public virtual Image proceedingTechnologyBar { get; set; }
    public virtual BaseController mainBase { get; set; }

    protected virtual void Start() {
        technologyName = "Placeholder name";
        technologyDescription = "Placeholder description";
        researchTimer = 0;
        researchTime = 0;
        researchCost = 0;
        researched = false;
        researching = false;
        applyTechnology = false;
    }
    protected virtual void Update() {
        researchTimer += Time.deltaTime;
        if (researchTimer > researchTime && !researching && !researched) {
            technologyImage.fillAmount = 1;
            proceedingTechnologyBar.fillAmount = 1;
        }
        else {
            technologyImage.fillAmount = ((researchTime - researchTimer) / researchTime);

            if (researchTimer < researchTime) { //stops you from clicking again and resetting the research
                researching = true;
                
            }
            else {
                researching = false;
                researched = true;
                proceedingTechnologyBar.fillAmount = 0;
            }
        }
    }
    public abstract void TechnologyEffect();
    public abstract void StartResearch();
    public abstract void EndResearch();
}
