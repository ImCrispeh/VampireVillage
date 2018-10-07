using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class CloakOfDarkness : Technology, IPointerEnterHandler, IPointerExitHandler
{
    public static CloakOfDarkness _instance;

    public Image unresearchedImage;
    public Image connectingBar;
    public GameObject technologyObject;
    public Transform technologyPosition;
    public Technology requiredTechnology;   //add more if you need more than one pre-requiste

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        }
        else {
            _instance = this;
        }
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        technologyName = "Cloak of Darkness";
        technologyDescription = "A sturdy cloak able to shield you from most of the sunlight";
        researchRequirement = "Soft Mantle";
        woodCost = 80;
        stoneCost = 110;
        goldCost = 90;
        researchTime = 120f;
        researchTimer = researchTime;
        researched = false;
        researching = false;
        applyTechnology = false;
        technologyImage = unresearchedImage;
        proceedingTechnologyBar.Add(connectingBar);
        mainBase = BaseController._instance;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (researched && !researching && !applyTechnology)
        {
            EndResearch();
            applyTechnology = true;
        }
    }

    public override void TechnologyEffect()
    {
        //The effects of the technology which are active once research ends
        //Allows player to send units out during the day but the thirst rate stay as the default rate (hungerDepletionRate = 0.0025f;)
        //resources.hungerDepletionRate = 0.0025f;
        //Instantiate(technologyObject, technologyPosition);
    }

    public override void StartResearch()
    {
        if (!researched && !researching && requiredTechnology.researched)
        {
            if (resources.wood >= woodCost && resources.stone >= stoneCost && resources.gold >= goldCost)
            {
                researchTimer = 0;
                researching = true;
                resources.SubtractWood(woodCost);
                resources.SubtractStone(stoneCost);
                resources.SubtractGold(goldCost);
                resources.UpdateResourceText();
            }
            else {
                NotEnoughResources();
            }
        }
        else {
            MissingPrerequisite();
        }
    }

    public override void EndResearch()
    {
        TechnologyEffect();
    }

    public override void OnPointerEnter(PointerEventData pointer)
    {
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

    public override void OnPointerExit(PointerEventData pointer)
    {
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

