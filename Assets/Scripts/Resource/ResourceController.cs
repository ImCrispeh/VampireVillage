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
    public void AddResource() {
        switch(tag) {
            case "Wood":
                ResourceStorage._instance.AddWood(resourceAmt);
                break;
            default:
                break;
        }

        Destroy(this.gameObject);
    }
}
