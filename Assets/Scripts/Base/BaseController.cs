using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseController : MonoBehaviour {
    public static BaseController _instance;

    public int maxHealth;
    public int health;
    public int attack;
    public int defense;
    public float attackTimer;
    public float timeBetweenAttacks;

    public bool isHungerEmpty;
    public bool hasHungerMessagePopped;
    public float noHungerTimer;
    public float timeBetweenNoHungerDmg;

    public List<GameObject> enemiesInRange;
    public GameObject miniCanvas;
    public Image healthBar;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    void Start () {
        maxHealth = 100;
        health = maxHealth;
        enemiesInRange = new List<GameObject>();
        miniCanvas = GameObject.Find("MiniCanvas");
        healthBar = miniCanvas.transform.Find("HealthBar").GetComponent<Image>();
        //miniCanvas.SetActive(false);
    }
	
	void Update () {
		if (enemiesInRange.Count > 0) {
            attackTimer += Time.deltaTime;

            if (attackTimer >= timeBetweenAttacks) {
                DealDamage();
                attackTimer -= timeBetweenAttacks;
            }
        }

        if (isHungerEmpty) {
            if (!hasHungerMessagePopped) {
                hasHungerMessagePopped = true;
                PopupController._instance.SetPopupText("Your empty hunger bar causes your colony to take damage");
            }

            noHungerTimer += Time.deltaTime;
            
            if (noHungerTimer >= timeBetweenNoHungerDmg) {
                TakeHungerDamage(5);
                noHungerTimer -= timeBetweenNoHungerDmg;
            }
        } else {
            hasHungerMessagePopped = false;
        }
	}

    public void Repair(int amountToRepair) {
        
        if (maxHealth - health < amountToRepair) {
            amountToRepair = maxHealth - health;
        }

        if (ResourceStorage._instance.wood > amountToRepair * 3 && ResourceStorage._instance.stone > amountToRepair * 3) {
            health += amountToRepair;
            ResourceStorage._instance.SubtractWood(amountToRepair * 3);
            ResourceStorage._instance.SubtractStone(amountToRepair * 3);
        } else {
            amountToRepair = (int)Mathf.Floor((Mathf.Min(ResourceStorage._instance.wood, ResourceStorage._instance.stone) / 3));
            health += amountToRepair;
            ResourceStorage._instance.SubtractWood(amountToRepair * 3);
            ResourceStorage._instance.SubtractStone(amountToRepair * 3);
        }

        SelectionController._instance.SetObjText();

        healthBar.fillAmount = (float)health / (float)maxHealth;
    }

    public void TakeDamage(int amt) {
        int damage = Mathf.Clamp(Mathf.RoundToInt(amt - (defense/2)), 1, int.MaxValue);
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        if (SelectionController._instance != null) {
            SelectionController._instance.SetObjText();
        } else {
            TutorialController._instance.SetObjText();
        }

        if (health == 0) {
            SceneController.Instance.EndGame(false, "Your colony lost all of its health");
        }

        healthBar.fillAmount = (float)health / (float)maxHealth;
    }

    public void TakeHungerDamage(int amt) {
        health -= amt;
        health = Mathf.Clamp(health, 0, maxHealth);

        if (SelectionController._instance != null) {
            SelectionController._instance.SetObjText();
        } else {
            TutorialController._tutInstance.SetObjText();
        }

        if (health == 0) {
            SceneController.Instance.EndGame(false, "Your colony lost all of its health");
        }

        healthBar.fillAmount = (float)health / (float)maxHealth;
    }

    public bool IsFullHealth() {
        return health == maxHealth;
    }

    // Iteration through list done in reverse to safely remove any dead enemies
    public void DealDamage() {
        for (int i = enemiesInRange.Count - 1; i >= 0; i--) {
            if (enemiesInRange[i] != null) {
                if (enemiesInRange[i].GetComponent<EnemyController>().IsDeadAfterDamage(attack)) {
                    GameObject toDestroy = enemiesInRange[i];
                    enemiesInRange.RemoveAt(i);
                    Destroy(toDestroy);
                    SelectionController._instance.SetObjText();
                }
            }
        }
        SelectionController._instance.SetObjText();
    }

    public void HideCanvas() {
        miniCanvas.SetActive(false);
    }

    public void ShowCanvas() {
        miniCanvas.SetActive(true);
    }
}
