using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour {
    public int resourceAmt;

	void Start () {
		
	}
	
	void Update () {
		
	}

    public void AddResource(UnitController unit) {
        switch(tag) {
            case "Wood":
                unit.woodCollected += resourceAmt;
                break;
            default:
                break;
        }
    }
}
