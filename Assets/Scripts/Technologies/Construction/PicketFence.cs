using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.AI;

public class PicketFence : Technology, IPointerEnterHandler, IPointerExitHandler {

    public Image unresearchedImage;
    public Image connectingBar;
    public GameObject technologyObject;
    public Transform technologyPosition;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        technologyName = "Picket Fence";
        technologyDescription = "Defense + 1" + "\n" + "A wooden fence is constructed adding to your defenses";
        researchRequirement = "";
        woodCost = 10;
        stoneCost = 0;
        goldCost = 0;
        researchTime = 5f; 
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
        mainBase.defense += 1;
        mainBase.gameObject.GetComponent<NavMeshObstacle>().size = new Vector3(6.5f, 1f, 6.5f);
        SelectionController._instance.MoveSpawnPoint();
        foreach (UnitController unit in FindObjectsOfType<UnitController>()) {
            if (unit.isReturning) {
                unit.agent.destination = SelectionController._instance.spawnPoint.position;
            }
        }

        foreach (EnemyController enemy in FindObjectsOfType<EnemyController>()) {
            if (enemy.isMovingToAttack) {
                enemy.agent.destination = SelectionController._instance.spawnPoint.position;
                if (enemy.isAttacking) {
                    enemy.isAttacking = false;
                }

                if (BaseController._instance.enemiesInRange.Contains(this.gameObject)) {
                    BaseController._instance.enemiesInRange.Remove(this.gameObject);
                }
            }
        }
        tech.transform.SetParent(technologyPosition);
    }

    public override void StartResearch() {
        if (!researched && !researching) {
            if (resources.wood >= woodCost) {
                if (TutorialController._tutInstance != null) {
                    Timer._instance.UnpauseTimer();
                }
                researchTimer = 0;
                researching = true;
                resources.SubtractWood(woodCost);
                resources.UpdateResourceText();
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
    }

    public override void EndResearch() {
        if (TutorialController._tutInstance != null) {
            TutorialController._tutInstance.ChangeText();
        }
        TechnologyEffect(); 
    }

    public override void OnPointerEnter(PointerEventData pointer) {
        ttbName.text = technologyName;
        ttbResearchRequirement.text = researchRequirement;
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
