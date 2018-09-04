using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanTownController : MonoBehaviour {
    public float partialFeedAmt;
    public float fullFeedAmt;

    public float partialFeedThreat;
    public float fullFeedThreat;
    public float convertThreat;

    public ThreatController threatCont;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
        Debug.Log("Should be subjugating except its not implemented");
    }
}
