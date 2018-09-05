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
    public float subjugationBaseSpeed;
    public float subjugationCalculatedSpeed;
    public float subjugationLevel;
    public float subjugationLimit;
    public List<UnitController> units;
    public ThreatController threatCont;
    public bool beingSubjugated;
    public bool subjugationFinished;
    public EnemySpawner enemySpawner;

    public Canvas subjugationCanvas;
    public Image subjugationBar;
    public Camera mainCamera;

    // Use this for initialization
    void Start () {
        subjugationFinished = false;
        beingSubjugated = false;
        subjugationBaseSpeed = 1f;
        units = new List<UnitController>();
        enemySpawner = gameObject.GetComponent<EnemySpawner>();

        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        subjugationCanvas = GameObject.Find("World/Human Town/SubjugationCanvas").GetComponent<Canvas>();
        subjugationBar = GameObject.Find("World/Human Town/SubjugationCanvas/SubjugationBar").GetComponent<Image>();
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
        threatCont.AddThreat(partialFeedThreat);
    }

    public void FullFeedEffect(UnitController unit) {
        unit.hungerCollected += fullFeedAmt;
        threatCont.AddThreat(fullFeedThreat);
    }

    public void ConvertEffect(UnitController unit) {
        unit.humanConvertCollected++;
        threatCont.AddThreat(convertThreat);
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
        ResourceStorage._instance.hungerDepletionRateModifier -= 0.1f;
        enemySpawner.canSpawn = false;
    }
}
