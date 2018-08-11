using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour {
    public NavMeshAgent agent;

    public bool isCollecting;
    public GameObject resourceToCollect;

    public bool isReturning;
    public GameObject unitBase;

    public int woodCollected;
    public int hungerCollected;

	void Awake () {
        agent = GetComponent<NavMeshAgent>();
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
                        SelectionController._instance.ReturnUnit(this);
                    }
                }
            }
        }
	}

    // Move unit to whatever was selected
    public void Move(GameObject dest) {
        agent.destination = Vector3.Lerp(dest.transform.position, transform.position, 0.05f);
    }

    public void MoveToCollect(GameObject dest) {
        isCollecting = true;
        resourceToCollect = dest;
        Debug.Log("moving to collect, " + dest);
        Move(dest);
    }

    public IEnumerator CollectResource() {
        yield return new WaitForSeconds(2f);
        if (resourceToCollect != null) {
            resourceToCollect.GetComponent<ResourceController>().AddResource(this);
        }
        isCollecting = false;
        resourceToCollect = null;
        ReturnFromCollection();
    }

    public void ReturnFromCollection() {
        isReturning = true;
        Debug.Log("returning to base");
        Move(unitBase);
    }
}
