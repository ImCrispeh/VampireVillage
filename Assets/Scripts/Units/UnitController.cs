using System.Collections;
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
        // Stop action and return if resource runs out
        if (objectForAction != null) {
            if (objectForAction.GetComponent<ResourceController>() != null) {
                if (objectForAction.GetComponent<ResourceController>().isRespawning && !isReturning) {
                    isPerformingAction = false;
                    ReturnFromAction();
                }
            }
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

        this.transform.LookAt(new Vector3(agent.destination.x, transform.position.y, agent.destination.z));
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
        NavMeshHit hit;
        if (NavMesh.SamplePosition(Vector3.Lerp(dest.transform.position, transform.position, 0.05f), out hit, 10f, NavMesh.AllAreas)) {
            agent.destination = hit.position;
        }
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
        if(objectForAction.ToString().Contains("Wood")){
            Debug.Log("YUREEKA");
        }
        Debug.Log(objectForAction);
        yield return new WaitForSeconds(2f);

        if (objectForAction != null) {
            switch (action) {
                case SelectionController.Actions.collect:
                    if (!objectForAction.GetComponent<ResourceController>().isRespawning) {
                        objectForAction.GetComponent<ResourceController>().AddResource(this);
                    }
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
                case SelectionController.Actions.repair:
                    objectForAction.GetComponent<BaseController>().Repair();
                    break;
                case SelectionController.Actions.subjugate:
                    if (objectForAction.GetComponent<HumanTownController>() != null) {
                        objectForAction.GetComponent<HumanTownController>().Subjugate(this);
                    } else {
                        objectForAction.GetComponent<EnemySpawner>().Subjugate(this);
                    }
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
