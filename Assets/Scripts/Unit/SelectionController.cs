using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour {
    public List<UnitController> selectedUnits;

	// Use this for initialization
	void Start () {
        selectedUnits = new List<UnitController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // only 1 unit can be selected at a time at this current point
            if (Physics.Raycast(ray, out hit)) {
                if (hit.transform.tag == "Unit") {
                    DeselectUnits();
                    UnitController unit = hit.transform.gameObject.GetComponent<UnitController>();
                    unit.Select();
                    selectedUnits.Add(unit);
                } else {
                    DeselectUnits();
                }
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            foreach (UnitController unit in selectedUnits) {
                unit.GetComponent<UnitController>().Move();
            }
        }
    }

    public void DeselectUnits() {
        foreach (UnitController unit in selectedUnits) {
            unit.Deselect();
        }

        selectedUnits.Clear();
    }
}
