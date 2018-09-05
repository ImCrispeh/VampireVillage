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
    public GameObject town;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start () {
        
    }

	void Update () {
        if (isMovingToAttack) {
            this.transform.LookAt(town.transform);
            if (!agent.pathPending) {
                if (new Vector3(agent.destination.x - transform.position.x, 0f, agent.destination.z - transform.position.z).sqrMagnitude <= agent.stoppingDistance) {
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

    public void MoveToAttack(int destId) {
        float rad;
        if (destId % 2 == 0) {
            rad = (450f - (360f / 10f * destId/2)) % 360 * Mathf.Deg2Rad;
        } else {
            rad = (90f + (360f / 10f * Mathf.Ceil((float)destId / 2))) % 360 * Mathf.Deg2Rad;
        }
        Vector3 basePos = BaseController._instance.transform.position;
        Vector3 dest = new Vector3(basePos.x + 5f * Mathf.Sin(rad), transform.position.y, basePos.z + 5f * Mathf.Cos(rad));
        agent.destination = dest;
        isMovingToAttack = true;
    }

    public bool IsDeadAfterDamage(int amt) {
        health -= amt;
        health = Mathf.Clamp(health, 0, int.MaxValue);
        return health == 0;
    }
}
