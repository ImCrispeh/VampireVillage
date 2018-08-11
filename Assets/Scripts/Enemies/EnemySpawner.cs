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

	void Start () {
		
	}
	
	void Update () {
        if (Timer._instance.currentTime >= 0.375f && Timer._instance.currentTime <= 0.7f && !isSpawning && enemiesToSpawn > 0) {
            isSpawning = true;
            hasSetSpawn = false;
        }

        if (isSpawning) {
            SpawnEnemies();
        } else {
            if (!hasSetSpawn) {
                if (Timer._instance.currentTime >= 0.75f) {
                    enemiesToSpawn = 2;
                    hasSetSpawn = true;
                }
            }
        }
    }

    public void SpawnEnemies() {
        if (enemiesToSpawn > 0) {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= timeBetweenSpawns) {
                Instantiate(enemy, spawnPoint);
                enemiesToSpawn--;
                spawnTimer -= timeBetweenSpawns;
            }
        } else {
            isSpawning = false;
        }
    }
}
