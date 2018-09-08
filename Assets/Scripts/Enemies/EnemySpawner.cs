using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public static EnemySpawner _instance;

    public int enemiesToSpawn;
    public int heavyEnemiesToSpawn;
    public float spawnTimer;
    public float timeBetweenSpawns;
    public bool isSpawning;
    public bool hasSetSpawn;
    public bool canSpawn;
    public float difficultyMultiplier;

    [SerializeField]
    private Transform[] spawnPositions;
    [SerializeField]
    private GameObject[] lightEnemies;
    [SerializeField]
    private GameObject[] heavyEnemies;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    void Start() {
        difficultyMultiplier = 1;
        canSpawn = true;
    }

    void Update() {
        if (Timer._instance.currentTime >= 0.325f && Timer._instance.currentTime <= 0.6f && !isSpawning && enemiesToSpawn > 0) {
            isSpawning = true;
            hasSetSpawn = false;
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            heavyEnemiesToSpawn = 2;
            enemiesToSpawn = 6;
            isSpawning = true;
            hasSetSpawn = false;
        }

        if (canSpawn) {
            if (isSpawning) {
                SpawnEnemies();
            } else {
                if (!hasSetSpawn && ThreatController._instance.threatLevel > 0) {
                    if (Timer._instance.currentTime >= 0.3f && Timer._instance.currentTime <= 0.375f) {
                        SetEnemiesToSpawn();
                    }
                }
            }
        }
    }

    public void SpawnEnemies() {

        if (heavyEnemiesToSpawn > 0) {
            Debug.Log("heavy: " + heavyEnemiesToSpawn);
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= timeBetweenSpawns) {
                for (int i = 0; i < spawnPositions.Length; i++) {
                    SpawnEnemy(heavyEnemies[Random.Range(0, heavyEnemies.Length)], spawnPositions[i]);
                    heavyEnemiesToSpawn--;
                    if (heavyEnemiesToSpawn == 0) {
                        break;
                    }
                }
                spawnTimer -= timeBetweenSpawns;
            }

        }

        if (enemiesToSpawn > 0) {
            Debug.Log("Enemies: " + enemiesToSpawn);
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= timeBetweenSpawns) {
                for (int i = 0; i < spawnPositions.Length; i++) {
                    SpawnEnemy(lightEnemies[Random.Range(0, lightEnemies.Length)], spawnPositions[i]);
                    enemiesToSpawn--;
                    if (enemiesToSpawn == 0) {
                        break;
                    }
                }
                spawnTimer -= timeBetweenSpawns;
            }
        } else {
            isSpawning = false;
            ThreatController._instance.SubtractThreat();
        }
    }

    public void SpawnEnemy(GameObject enemyType, Transform spawnPos) {
        GameObject newEnemy = Instantiate(enemyType);
        newEnemy.transform.position = spawnPos.position;
        newEnemy.transform.SetParent(spawnPos);
        EnemyController enemy = newEnemy.GetComponent<EnemyController>();
        enemy.SetStats(ThreatController._instance.threatLevel, difficultyMultiplier);
        newEnemy.GetComponent<EnemyController>().MoveToAttack();
    }

    public void SetEnemiesToSpawn() {
        enemiesToSpawn = 2 * ThreatController._instance.threatLevel;
        heavyEnemiesToSpawn = ThreatController._instance.threatLevel / 2;
        hasSetSpawn = true;
    }

    public void IncreaseDifficulty() {
        difficultyMultiplier += 0.25f;
    }
}
