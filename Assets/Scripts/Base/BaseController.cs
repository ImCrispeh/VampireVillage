using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour {
    public static BaseController _instance;

    public int health;
    public int attack;
    public int defense;
    public float attackTimer;
    public float timeBetweenAttacks;

    public List<GameObject> enemiesInRange;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    // Use this for initialization
    void Start () {
        enemiesInRange = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update () {
		if (enemiesInRange.Count > 0) {
            attackTimer += Time.deltaTime;

            if (attackTimer >= timeBetweenAttacks) {
                DealDamage();
                attackTimer -= timeBetweenAttacks;
            }
        }
	}

    public void TakeDamage(int amt) {
        int damage = Mathf.RoundToInt(amt / defense);
        health -= damage;
    }

    // Iteration through list done in reverse to safely remove any dead enemies
    public void DealDamage() {
        for (int i = enemiesInRange.Count - 1; i >= 0; i--) {
            if (enemiesInRange[i].GetComponent<EnemyController>().IsDeadAfterDamage(attack)) {
                GameObject toDestroy = enemiesInRange[i];
                enemiesInRange.RemoveAt(i);
                Destroy(toDestroy);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Enemy") {
            enemiesInRange.Add(other.gameObject);
        }
    }
}
