using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanTownController : MonoBehaviour {
    public float partialFeedAmt;
    public float fullFeedAmt;
    public float convertAmt;

    public ThreatController threatCont;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PartialFeedEffect(UnitController unit) {
        unit.hungerCollected += partialFeedAmt;
        threatCont.AddThreat(partialFeedAmt / 2);
    }

    public void FullFeedEffect(UnitController unit) {
        unit.hungerCollected += fullFeedAmt;
        threatCont.AddThreat(fullFeedAmt / 2);
    }

    public void ConvertEffect(UnitController unit) {
        unit.humanConvertCollected++;
        threatCont.AddThreat(convertAmt);
    }
}
