using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour {
    public int resourceAmt;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Using a switch statement so it is easier to extend when adding more resources instead of multiple else if
    public void AddResource(UnitController unit) {
        switch(tag) {
            case "Wood":
                unit.woodCollected += resourceAmt;
                break;
            case "HumanTown":
                unit.hungerCollected += resourceAmt;
                break;
            default:
                break;
        }

        if (gameObject.tag != "HumanTown") {
            Destroy(this.gameObject);
        }
    }
}
