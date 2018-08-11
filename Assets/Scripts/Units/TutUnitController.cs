using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Same idea as TutorialController and SelectionController relationship
public class TutUnitController : UnitController {

	// Use this for initialization
	void Start () {
		
	}
	
	void Update () {
        if (resourceToCollect == null && !isReturning) {
            isCollecting = false;
            ReturnFromCollection();
        }

        if (isCollecting) {
            if (!agent.pathPending) {
                if ((agent.destination - transform.position).sqrMagnitude <= agent.stoppingDistance) {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
                        StartCoroutine("CollectResource");
                    }
                }
            }
        }

        if (isReturning) {
            if (!agent.pathPending) {
                if ((agent.destination - transform.position).sqrMagnitude <= agent.stoppingDistance) {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
                        isReturning = false;
                        TutorialController._tutInstance.ReturnUnit(this);
                    }
                }
            }
        }
    }
}
