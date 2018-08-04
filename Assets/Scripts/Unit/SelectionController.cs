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
            SelectUnits();
        }

        // perform action based on thing that was clicked on
        if (Input.GetMouseButtonDown(1)) {
            CommandUnits();
        }
    }

    public void SelectUnits() {
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

    public void CommandUnits() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit)) {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Resource")) {
                foreach (UnitController unit in selectedUnits) {
                    unit.MoveToCollect(hit);
                    Debug.Log("collecting");
                }
            } else {
                foreach (UnitController unit in selectedUnits) {
                    unit.Move(hit);
                    Debug.Log("moving");
                }
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
