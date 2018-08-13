using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanTownController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PartialFeedEffect(UnitController unit) {
        unit.hungerCollected += 25f;
    }

    public void FullFeedEffect(UnitController unit) {
        unit.hungerCollected += 100f;
    }

    public void ConvertEffect(UnitController unit) {
        unit.humanConvertCollected++;
    }
}
