using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Same idea as TutorialController and SelectionController relationship
public class TutUnitController : UnitController {

	// Use this for initialization
	void Start () {

    }

    void Update() {
        if (objectForAction == null && !isReturning) {
            isPerformingAction = false;
            ReturnFromAction();
        }

        if (isPerformingAction) {
            if (HasReachedDestination()) {
                StartCoroutine("PerformAction");
            }
        }

        if (isReturning) {
            if (HasReachedDestination()) {
                isReturning = false;
                TutorialController._tutInstance.ReturnUnit(this);
            }
        }

        this.transform.LookAt(new Vector3(agent.destination.x, transform.position.y, agent.destination.z));
    }
}
