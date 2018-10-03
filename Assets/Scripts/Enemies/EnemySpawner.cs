using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour {
    public static EnemySpawner _instance;

    public int enemiesToSpawn;
    public int heavyEnemiesToSpawn;
    public int catapultsToSpawn;
    public float spawnTimer;
    public float timeBetweenSpawns;
    public bool isSpawning;
    public bool hasSetSpawn;
    public float difficultyMultiplier;
    public AudioClip callToArms;

    private bool bellSounded = true;

    public float subjugationBaseSpeed;
    public float subjugationCalculatedSpeed;
    public float subjugationLevel;
    public float subjugationLimit;

    public List<UnitController> units;
    public bool beingSubjugated;
    public bool subjugationFinished;

    public Canvas subjugationCanvas;
    public Image subjugationBar;
    public Camera mainCamera;

    [SerializeField]
    private Transform[] spawnPositions;
    [SerializeField]
    private GameObject[] lightEnemies;
    [SerializeField]
    private GameObject[] heavyEnemies;
    public GameObject catapult;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    void Start() {
        subjugationFinished = false;
        beingSubjugated = false;
        subjugationBaseSpeed = 0.5f;
        subjugationLimit = 100f;
        units = new List<UnitController>();

        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        subjugationCanvas = transform.Find("SubjugationCanvas").gameObject.GetComponent<Canvas>();
        subjugationBar = transform.Find("SubjugationCanvas/SubjugationBar").GetComponent<Image>();
        subjugationCanvas.gameObject.SetActive(false);

        InvokeRepeating("CalculateSubjugationSpeed", 2, 1);

        difficultyMultiplier = 0.75f;
    }

    void Update() {
        if (Timer._instance.currentTime >= 0.325f && Timer._instance.currentTime <= 0.6f && !isSpawning && enemiesToSpawn > 0) {
            isSpawning = true;
            hasSetSpawn = false;
        }
        if (isSpawning) {
            SpawnEnemies();
        } else {
            if (!hasSetSpawn && ThreatController._instance.threatLevel > 0) {
                if (Timer._instance.currentTime >= 0.3f && Timer._instance.currentTime <= 0.375f) {
                    SetEnemiesToSpawn();
                }
            }
        }

        if (subjugationLevel >= subjugationLimit && !subjugationFinished) {
            subjugationLevel = subjugationLimit;
            Invoke("EndSubjugation", 1);
            CancelInvoke("CalculateSubjugationSpeed");
            subjugationFinished = true;
        }
    }

    void LateUpdate() {
        subjugationCanvas.transform.forward = mainCamera.transform.forward;
    }

    public void SpawnEnemies() {

        if (heavyEnemiesToSpawn > 0) {
            if(!bellSounded){
                //SoundManager.instance.RandomizeSfx(callToArms);
                //bellSounded = true;
            }
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
            if (!bellSounded) {
                SoundManager.instance.RandomizeSfx(callToArms);
                bellSounded = true;
            }
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
            bellSounded = false;
            ThreatController._instance.SubtractThreat();
        }

        if (catapultsToSpawn > 0) {
            SpawnEnemy(catapult, spawnPositions[0]);
            catapultsToSpawn--;
        }
    }

    public void SpawnEnemy(GameObject enemyType, Transform spawnPos) {
        GameObject newEnemy = Instantiate(enemyType, spawnPos);
        newEnemy.transform.localScale = new Vector3(0.1f, 0.125f, 0.1f);
        //newEnemy.transform.SetParent(spawnPos);
        //newEnemy.transform.position = spawnPos.position;
        EnemyController enemy = newEnemy.GetComponent<EnemyController>();
        enemy.SetStats(ThreatController._instance.threatLevel, difficultyMultiplier);
        newEnemy.GetComponent<EnemyController>().MoveToAttack();
    }

    public void SetEnemiesToSpawn() {
        enemiesToSpawn = (int)(1.5 * ThreatController._instance.threatLevel);
        heavyEnemiesToSpawn = ThreatController._instance.threatLevel / 2;
        if (ThreatController._instance.threatLevel == 5) {
            catapultsToSpawn = 1;
        }
        hasSetSpawn = true;
    }

    public void IncreaseDifficulty() {
        if (difficultyMultiplier < 1.5f) {
            difficultyMultiplier = Mathf.Clamp(difficultyMultiplier + 0.15f, 0f, 1.5f);
        }
    }

    //disables the unit and they're added to a list, the canvas for the subjugation level is then activated
    public void Subjugate(UnitController unit) {
        if (!subjugationFinished) {
            beingSubjugated = true;
            units.Add(unit);
            unit.gameObject.SetActive(false);
            if (!subjugationCanvas.gameObject.activeSelf) {
                subjugationCanvas.gameObject.SetActive(true);
            }
        }
    }

    //the speed the subjugation takes place depends on the number of units at the town, also updates the subjugation bar
    public void CalculateSubjugationSpeed() {
        subjugationCalculatedSpeed = units.Count * subjugationBaseSpeed;
        subjugationLevel += subjugationCalculatedSpeed;
        float currentSubjugation = subjugationLevel;
        currentSubjugation = currentSubjugation / subjugationLimit;
        subjugationBar.fillAmount = currentSubjugation;
    }

    //once the subjugation limit has been reached, subjugation ends and the units are sent back home
    public void EndSubjugation() {
        foreach (UnitController unit in units) {
            Debug.Log("reactivating unit");
            unit.gameObject.SetActive(true);
            unit.ReturnFromAction();
        }
        beingSubjugated = false;
        SubjugatedBonuses();
        subjugationCanvas.gameObject.SetActive(false);
    }

    //get reduced hunger depletion and the town stops spawning enemies for subjugation
    public void SubjugatedBonuses() {
        SceneController.Instance.EndGame(true, "You successfully subjugated all the humans");
    }
}
