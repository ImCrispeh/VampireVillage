using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanTownController : MonoBehaviour {
    public float partialFeedAmt;
    public float fullFeedAmt;

    public float partialFeedThreat;
    public float fullFeedThreat;
    public float convertThreat;

    public int population;

	// Use this for initialization
	void Start () {
        partialFeedAmt = 25f;
        fullFeedAmt = 100f;

        partialFeedThreat = 7.5f;
        fullFeedThreat = 25f;
        convertThreat = 35f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PartialFeedEffect(UnitController unit) {
        unit.hungerCollected += partialFeedAmt;
        ThreatController._instance.AddThreat(partialFeedThreat);
    }

    public void FullFeedEffect(UnitController unit) {
        unit.hungerCollected += fullFeedAmt;
        ThreatController._instance.AddThreat(fullFeedThreat);
    }

    public void ConvertEffect(UnitController unit) {
        unit.humanConvertCollected++;
        ThreatController._instance.AddThreat(convertThreat);
    }
}
