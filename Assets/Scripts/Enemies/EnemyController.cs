using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {
    public NavMeshAgent agent;

    public bool isMovingToAttack;
    public bool isAttacking;

    public int health;
    public int attack;
    public float attackTimer;
    public float timeBetweenAttacks;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        if (isMovingToAttack) {
            if (!agent.pathPending) {
                if ((agent.destination - transform.position).sqrMagnitude <= agent.stoppingDistance) {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
                        isMovingToAttack = false;
                        isAttacking = true;
                    }
                }
            }
        }

        if (isAttacking) {
            attackTimer += Time.deltaTime;

            if (attackTimer >= timeBetweenAttacks) {
                BaseController._instance.TakeDamage(attack);
                attackTimer -= timeBetweenAttacks;
            }
        }
    }

    public void MoveToAttack() {
        agent.destination = Vector3.Lerp(BaseController._instance.transform.position, transform.position, 0.05f);
    }

    public bool IsDeadAfterDamage(int amt) {
        health -= amt;
        return health <= 0;
    }
}
