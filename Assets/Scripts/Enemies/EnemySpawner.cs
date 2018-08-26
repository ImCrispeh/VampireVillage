using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public int enemiesToSpawn;
    public int heavyEnemiesToSpawn;
    public GameObject enemy;
    public float spawnTimer;
    public float timeBetweenSpawns;
    public Transform spawnPoint;
    public bool isSpawning;
    public bool hasSetSpawn;

    [SerializeField]
    private Transform[] spawnPositions;
    [SerializeField]
    private GameObject[] lightEnemies;
    [SerializeField]
    private GameObject[] heavyEnemies;

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

        if(heavyEnemiesToSpawn > 0){
            Debug.Log("heavy: "+heavyEnemiesToSpawn);
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= timeBetweenSpawns) {
                for(int i=0; i < spawnPositions.Length; i++){
                        GameObject newHeavyEnemy = Instantiate(heavyEnemies[Random.Range(0, heavyEnemies.Length)], spawnPositions[i]);
                        newHeavyEnemy.GetComponent<EnemyController>().attack *= threatCont.threatLevel;
                        newHeavyEnemy.GetComponent<EnemyController>().MoveToAttack(spawnPositions[i].childCount-1);
                        heavyEnemiesToSpawn--;
                        if(heavyEnemiesToSpawn == 0){
                            break;
                        }
                    }
                    spawnTimer -= timeBetweenSpawns;
            }

        }

        if (enemiesToSpawn > 0) {
            Debug.Log("Enemies: "+enemiesToSpawn);
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= timeBetweenSpawns) {
                for(int i=0; i < spawnPositions.Length; i++){
                        GameObject newEnemy = Instantiate(lightEnemies[Random.Range(0, lightEnemies.Length)], spawnPositions[i]);
                        newEnemy.GetComponent<EnemyController>().attack *= threatCont.threatLevel;
                        newEnemy.GetComponent<EnemyController>().MoveToAttack(spawnPositions[i].childCount-1);
                        enemiesToSpawn--;
                        if(enemiesToSpawn == 0){
                            break;
                        }
                    }
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
            heavyEnemiesToSpawn = threatCont.threatLevel / 2;
            hasSetSpawn = true;
        }
    }
}
