using System;
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

    public AudioClip battle1;
    public AudioClip battle2;
    public AudioClip battle3;
    public AudioClip battle4;
    public AudioClip battle5;
    public AudioClip battle6;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start () {
        
    }

	void Update () {
        if (isMovingToAttack) {
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
                SoundManager.instance.RandomizeSfx(battle1, battle2, battle3, battle4, battle5, battle6);
            }
        }

        this.transform.LookAt(new Vector3(BaseController._instance.transform.position.x, transform.position.y, BaseController._instance.transform.position.z));
    }

    public void SetStats(float threatLevel, float difficulty) {
        attack = (int)Math.Round((attack * threatLevel * difficulty), MidpointRounding.AwayFromZero);
        health = (int)Math.Round((health * threatLevel * difficulty), MidpointRounding.AwayFromZero);
    }

    public void MoveToAttack() {
        Vector3 basePos = BaseController._instance.transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(Vector3.Lerp(basePos, transform.position, 0.05f), out hit, 10f, NavMesh.AllAreas)) {
            agent.destination = hit.position;
        }
        isMovingToAttack = true;
    }

    public bool IsDeadAfterDamage(int amt) {
        health -= amt;
        health = Mathf.Clamp(health, 0, int.MaxValue);
        return health == 0;
    }
}
