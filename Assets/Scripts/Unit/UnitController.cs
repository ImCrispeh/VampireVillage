using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour {
    public NavMeshAgent agent;
    public GameObject selectionIndicator;
    public bool isCollecting;
    public GameObject resourceToCollect;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        if (isCollecting) {
            if (!agent.pathPending) {
                if ((agent.destination - transform.position).sqrMagnitude <= agent.stoppingDistance) {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
                        resourceToCollect.GetComponent<ResourceController>().AddResource();
                        isCollecting = false;
                        resourceToCollect = null;
                    }
                }
            }
        }
	}

    // move unit to whatever was selected
    public void Move(GameObject dest) {
        //if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground")) {
        //    agent.destination = new Vector3(hit.point.x, transform.position.y, hit.point.z);
        //} else {
            agent.destination = Vector3.Lerp(dest.transform.position, transform.position, 0.05f);
        //}
    }

    public void MoveToCollect(GameObject dest) {
        isCollecting = true;
        resourceToCollect = dest;
        Debug.Log("moving to collect");
        Move(dest);
    }

    //public void Select() {
    //    selectionIndicator.SetActive(true);
    //}

    //public void Deselect() {
    //    selectionIndicator.SetActive(false);
    //}

}
