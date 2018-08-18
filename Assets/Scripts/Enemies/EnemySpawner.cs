using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public int enemiesToSpawn;
    public GameObject enemy;
    public float spawnTimer;
    public float timeBetweenSpawns;
    public Transform spawnPoint;
    public bool isSpawning;
    public bool hasSetSpawn;

    public ThreatController threatCont;

    void Start() {

    }

    void Update() {
        if (Timer._instance.currentTime >= 0.375f && Timer._instance.currentTime <= 0.7f && !isSpawning && enemiesToSpawn > 0) {
            isSpawning = true;
            hasSetSpawn = false;
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            isSpawning = true;
            hasSetSpawn = false;
            enemiesToSpawn = 10;
        }

        if (isSpawning) {
            SpawnEnemies();
        } else {
            if (!hasSetSpawn) {
                SetSpawn();
            }
        }
    }

    public void SpawnEnemies() {
        if (enemiesToSpawn > 0) {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= timeBetweenSpawns) {
                GameObject newEnemy = Instantiate(enemy, spawnPoint);
                newEnemy.GetComponent<EnemyController>().attack *= threatCont.threatLevel;
                newEnemy.GetComponent<EnemyController>().MoveToAttack(spawnPoint.childCount-1);
                enemiesToSpawn--;
                spawnTimer -= timeBetweenSpawns;
            }
        } else {
            isSpawning = false;
            threatCont.SubtractThreat();
        }
    }

    public void SetSpawn() {
        if (Timer._instance.currentTime >= 0.3f && Timer._instance.currentTime <= 0.375f) {
            enemiesToSpawn = 2 * threatCont.threatLevel;
            hasSetSpawn = true;
        }
    }
}
