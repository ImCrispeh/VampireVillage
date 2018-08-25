﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour {
    public NavMeshAgent agent;

    public bool isPerformingAction;
    public GameObject objectForAction;
    public SelectionController.Actions action;

    public bool isReturning;
    public GameObject spawnPoint;

    public int woodCollected;
    public int stoneCollected;
    public int goldCollected;
    public float hungerCollected;
    public int humanConvertCollected;

	void Awake () {
        agent = GetComponent<NavMeshAgent>();
	}
	
	void Update () {
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
                SelectionController._instance.ReturnUnit(this);
            }
        }
	}

    public bool HasReachedDestination() {
        if (!agent.pathPending) {
            if (new Vector3(agent.destination.x - transform.position.x, 0f, agent.destination.z - transform.position.z).sqrMagnitude <= agent.stoppingDistance) {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
                    return true;
                }
            }
        }
        return false;
    }

    // Move unit to whatever was selected
    public void Move(GameObject dest) {
        agent.destination = Vector3.Lerp(dest.transform.position, transform.position, 0.1f);
    }

    public void MoveToAction(GameObject dest) {
        isPerformingAction = true;
        objectForAction = dest;
        Debug.Log("moving to perform action on, " + dest);
        Move(dest);
    }

    // Perform action based on string passed in by button click
    public IEnumerator PerformAction() {
        Debug.Log("performing action");
        yield return new WaitForSeconds(2f);

        if (objectForAction != null) {
            switch (action) {
                case SelectionController.Actions.collect:
                    objectForAction.GetComponent<ResourceController>().AddResource(this);
                    break;
                case SelectionController.Actions.partialFeed:
                    objectForAction.GetComponent<HumanTownController>().PartialFeedEffect(this);
                    break;
                case SelectionController.Actions.fullFeed:
                    objectForAction.GetComponent<HumanTownController>().FullFeedEffect(this);
                    break;
                case SelectionController.Actions.convert:
                    objectForAction.GetComponent<HumanTownController>().ConvertEffect(this);
                    break;
                default:
                    break;
            }
        }
        isPerformingAction = false;
        objectForAction = null;
        ReturnFromAction();
    }

    public void ReturnFromAction() {
        isReturning = true;
        Debug.Log("returning to base");
        Move(spawnPoint);
    }
}
