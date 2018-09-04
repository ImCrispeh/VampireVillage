using System.Collections;
using System.Collections.Generic;
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
    public bool cancelled;

	// Use this for initialization
	void Start () {
        cancelled = false;
        subjugationBaseSpeed = 1f;
        units = new List<UnitController>();        
        InvokeRepeating("CalculateSubjugationSpeed", 2, 1);
    }
	
	// Update is called once per frame
	void Update () {        
        if (subjugationLevel >= subjugationLimit && !cancelled) {
            Invoke("EndSubjugation", 1);
            CancelInvoke("CalculateSubjugationSpeed");
            cancelled = true;
        }
        //Debug.Log("Subjugation level: " + subjugationLevel);
        //Debug.Log("Units: " + units.Count);
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

    public void Subjugate(UnitController unit) {
        Debug.Log("disabling unit");
        units.Add(unit);
        unit.gameObject.SetActive(false);        
    }

    //the speed the subjugation takes place depends on the number of units at the town
    public void CalculateSubjugationSpeed() {
        subjugationCalculatedSpeed = units.Count * subjugationBaseSpeed;
        subjugationLevel += subjugationCalculatedSpeed;
    }

    //once the subjugation limit has been reached, subjugation ends and the units are sent back home
    public void EndSubjugation() {
        foreach (UnitController unit in units) {
            Debug.Log("reactivating unit");
            unit.gameObject.SetActive(true);
            unit.ReturnFromAction();            
        }
    }
}
