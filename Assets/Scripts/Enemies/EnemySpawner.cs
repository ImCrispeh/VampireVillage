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

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Timer._instance.currentTime >= 0.4 && !isSpawning && enemiesToSpawn > 0) {
            isSpawning = true;
        }

        if (isSpawning) {
            SpawnEnemies();
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
