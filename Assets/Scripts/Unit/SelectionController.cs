using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour {
    public List<GameObject> selectedUnits;

	// Use this for initialization
	void Start () {
        selectedUnits = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // only 1 unit can be selected at a time at this current point
            if (Physics.Raycast(ray, out hit)) {
                if (hit.transform.tag == "Unit") {
                    selectedUnits.Clear();
                    selectedUnits.Add(hit.transform.gameObject);
                } else {
                    selectedUnits.Clear();
                }
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            foreach (GameObject unit in selectedUnits) {
                unit.GetComponent<UnitController>().Move();
            }
        }
    }
}
