using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HumanTownController : MonoBehaviour {
    public float partialFeedAmt;
    public float fullFeedAmt;
    public float partialFeedThreat;
    public float fullFeedThreat;
    public float convertThreat;

    public float population;
    public float populationBaseRegenRate;

    public float subjugationBaseSpeed;
    public float subjugationCalculatedSpeed;
    public float subjugationLevel;
    public float subjugationLimit;

    public List<UnitController> units;
    public bool beingSubjugated;
    public bool subjugationFinished;

    public Canvas subjugationCanvas;
    public Image subjugationBar;
    public Camera mainCamera;

    // Use this for initialization
    void Start () {
        partialFeedAmt = 25f;
        fullFeedAmt = 100f;

        partialFeedThreat = 7.5f;
        fullFeedThreat = 25f;
        convertThreat = 35f;

        populationBaseRegenRate = 0.5f;

        subjugationFinished = false;
        beingSubjugated = false;
        subjugationBaseSpeed = 1f;
        subjugationLimit = 100f;
        units = new List<UnitController>();

        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        subjugationCanvas = transform.Find("SubjugationCanvas").gameObject.GetComponent<Canvas>();
        subjugationBar = transform.Find("SubjugationCanvas/SubjugationBar").GetComponent<Image>();
        subjugationCanvas.gameObject.SetActive(false);            
        
        InvokeRepeating("CalculateSubjugationSpeed", 2, 1);
    }
	
	// Update is called once per frame
	void Update () {        
        if (subjugationLevel >= subjugationLimit && !subjugationFinished) {
            subjugationLevel = subjugationLimit;
            Invoke("EndSubjugation", 1);
            CancelInvoke("CalculateSubjugationSpeed");
            subjugationFinished = true;
        }
	}

    void LateUpdate() {
        subjugationCanvas.transform.forward = mainCamera.transform.forward;
    }

    public void PartialFeedEffect(UnitController unit) {
        unit.hungerCollected += partialFeedAmt;
        ThreatController._instance.AddThreat(partialFeedThreat);
        population -= 0.25f;
        population = Mathf.Clamp(population, 0, float.MaxValue);
    }

    public void FullFeedEffect(UnitController unit) {
        unit.hungerCollected += fullFeedAmt;
        ThreatController._instance.AddThreat(fullFeedThreat);
        population--;
        population = Mathf.Clamp(population, 0, float.MaxValue);
    }

    public void ConvertEffect(UnitController unit) {
        unit.humanConvertCollected++;
        ThreatController._instance.AddThreat(convertThreat);
        population--;
        population = Mathf.Clamp(population, 0, float.MaxValue);
    }

    //disables the unit and they're added to a list, the canvas for the subjugation level is then activated
    public void Subjugate(UnitController unit) {
        if (!subjugationFinished) {
            beingSubjugated = true;
            units.Add(unit);
            unit.gameObject.SetActive(false);
            if (!subjugationCanvas.gameObject.activeSelf) {
                subjugationCanvas.gameObject.SetActive(true);
            }
        }
    }

    //the speed the subjugation takes place depends on the number of units at the town, also updates the subjugation bar
    public void CalculateSubjugationSpeed() {
        subjugationCalculatedSpeed = units.Count * subjugationBaseSpeed;
        subjugationLevel += subjugationCalculatedSpeed;
        float currentSubjugation = subjugationLevel;
        currentSubjugation = currentSubjugation / subjugationLimit;
        subjugationBar.fillAmount = currentSubjugation;
    }

    //once the subjugation limit has been reached, subjugation ends and the units are sent back home
    public void EndSubjugation() {        
        foreach (UnitController unit in units) {
            Debug.Log("reactivating unit");
            unit.gameObject.SetActive(true);
            unit.ReturnFromAction();            
        }
        beingSubjugated = false;
        SubjugatedBonuses();
        subjugationCanvas.gameObject.SetActive(false);
    }

    //get reduced hunger depletion and the town stops spawning enemies for subjugation
    public void SubjugatedBonuses() {
        ResourceStorage._instance.hungerDepletionRateModifier *= 0.9f;
        SelectionController._instance.subjugatedHumanTowns++;

        // Set minimum threat
        ThreatController._instance.minThreat += 20f;
        ThreatController._instance.AddThreat(0);
    }

    public void RegenPopulation() {
        population += populationBaseRegenRate * (population / 20f);
    }
}
