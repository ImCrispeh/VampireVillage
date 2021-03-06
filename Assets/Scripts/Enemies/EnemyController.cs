﻿using System;
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
    public float movementResetTimer;
    public float attackTimer;
    public float timeBetweenAttacks;
    public bool doesIgnoreDefense;
    public GameObject town;

    public AudioClip battle1;
    public AudioClip battle2;
    public AudioClip battle3;
    public AudioClip battle4;
    public AudioClip battle5;
    public AudioClip battle6;

    private Animator anim;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start () {
        anim = GetComponent<Animator>();
        anim.SetBool("isDead", false);
    }

	void Update () {
        if (isMovingToAttack) {
            movementResetTimer += Time.deltaTime;

            //testing if resetting movement destination helps in removing enemies from their "stuck" state
            if (movementResetTimer >= 5f) {
                agent.ResetPath();
                MoveToAttack();
                movementResetTimer = 0;
            }
            anim.SetBool("isAttacking", false);
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
            if (!anim.GetBool("isDead")) {
                transform.position = Vector3.MoveTowards(transform.position, SelectionController._instance.spawnPoint.position, 15f * Time.deltaTime);
            }
            if (!BaseController._instance.enemiesInRange.Contains(this.gameObject)) {
                BaseController._instance.enemiesInRange.Add(this.gameObject);
            }

            attackTimer += Time.deltaTime;
            anim.SetBool("isAttacking", true);

            if (attackTimer >= timeBetweenAttacks) {
                BaseController._instance.TakeDamage(attack, doesIgnoreDefense);
                attackTimer -= timeBetweenAttacks;
                SoundManager.instance.RandomizeSfx(battle1, battle2, battle3, battle4, battle5, battle6);
            }
        }

        if (health <= 0) {
            attack = 0;
            isAttacking = false;
            if (BaseController._instance.enemiesInRange.Contains(this.gameObject)) {
                BaseController._instance.enemiesInRange.Remove(this.gameObject);
            }
        }

        this.transform.LookAt(new Vector3(BaseController._instance.transform.position.x, transform.position.y, BaseController._instance.transform.position.z));
    }

    public void SetStats(float threatLevel, float difficulty) {
        attack = (int)Math.Round((attack * (Mathf.Clamp(threatLevel / 1.5f, 1, 3)) * difficulty), MidpointRounding.AwayFromZero);
        health = (int)Math.Round((health * (Mathf.Clamp(threatLevel / 1.5f, 1, 3)) * difficulty), MidpointRounding.AwayFromZero);
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
        if (health <= 0) {
            isAttacking = false;
            anim.SetBool("isAttacking", false);
            anim.SetBool("isDead", true);
        }

        return health == 0;
    }
}
