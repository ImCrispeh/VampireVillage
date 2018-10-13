using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceStorage : MonoBehaviour {
    public static ResourceStorage _instance;

    public float maxHunger;
    public float hunger;
    public int wood;
    public int stone;
    public int gold;
    public int collectionModifier;

	public Timer timerScript;
    public float hungerDepletionRate;
    public float hungerDepletionRateModifier;
	public float numberOfDays;
	public float dayMultiplier;

    public Text resourceText;
    public Slider hungerBar;

    public bool hasHungerWarningShown;
    public Animation hungerBarOutlineAnimation;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    void Start () {
        maxHunger = 100f;
        hungerDepletionRate = 0.0075f;
        hungerDepletionRateModifier = 1f;
		dayMultiplier = 2;
        hunger = 20f;
        UpdateResourceText();
        hungerBar.value = HungerPercentage();
        collectionModifier = 1;
    }
	
	void Update () {
        if (!Timer._instance.isPaused) {
            SubtractHunger();
        }
	}

    public void AddHunger(float amt) {
        BaseController._instance.isHungerEmpty = false;
        hunger += amt;
        hunger = Mathf.Clamp(hunger, 0f, maxHunger);
        if (hunger > 25f) {
            hasHungerWarningShown = false;
        }
        hungerBar.value = HungerPercentage();
    }

    public void SubtractHunger() {
        if (SelectionController._instance != null) {
			CalculateHungerModifier();
            if (SoftMantle._instance.applyTechnology && !CloakOfDarkness._instance.applyTechnology) {
                if ((Timer._instance.currentTime <= 0.75f && Timer._instance.currentTime >= 0.25f) && SelectionController._instance.availableUnits < SelectionController._instance.maxUnits) {
					hunger -= hungerDepletionRate * hungerDepletionRateModifier * SelectionController._instance.maxUnits * 2;
                } else {
                    hunger -= hungerDepletionRate * hungerDepletionRateModifier * SelectionController._instance.maxUnits;
                }
            } else {
                hunger -= hungerDepletionRate * hungerDepletionRateModifier * SelectionController._instance.maxUnits;
            }
        } else {
            hunger -= hungerDepletionRate * hungerDepletionRateModifier;
        }

        hunger = Mathf.Clamp(hunger, 0f, maxHunger);
        hungerBar.value = HungerPercentage();

        if (hunger <= 25f) {
            if (!hasHungerWarningShown) {
                hasHungerWarningShown = true;
                PopupController._instance.SetPopupText("Your hunger is getting low. You will start taking damage if it reaches 0.");
            }
            if (!hungerBarOutlineAnimation.isPlaying) {
                hungerBarOutlineAnimation.Play("HungerBarWarning");
            }
        }

        if (hunger == 0f) {
            BaseController._instance.isHungerEmpty = true;
        }
    }

	private void CalculateHungerModifier() {
		numberOfDays = timerScript.currentDay;
		if (numberOfDays >= 20) {
			hungerDepletionRateModifier = 1.5f;
		} else {
			hungerDepletionRateModifier = 1 + numberOfDays / 40;
		}
	}

    public float HungerPercentage() {
        return hunger / maxHunger;
    }

    public void AddWood(int amt) {
        wood += amt * collectionModifier;
    }

    public void SubtractWood(int amt) {
        wood -= amt;
    }

    public void AddStone(int amt) {
        stone += amt * collectionModifier;
    }

    public void SubtractStone(int amt) {
        stone -= amt;
    }

    public void AddGold(int amt) {
        gold += amt * collectionModifier;
    }

    public void SubtractGold(int amt) {
        gold -= amt;
    }

    public void UpdateResourceText() {
        int units;

        if (SelectionController._instance != null) {
            units = SelectionController._instance.availableUnits;
            Debug.Log(units);
        } else {
            units = TutorialController._tutInstance.availableUnits;
            Debug.Log(units);
        }

        resourceText.text =
        //"Hunger: " + hunger + "\n"
        //+ "Wood: " + wood + "\n"
        //+ "Stone: " + stone + "\n"
        //+ "Gold: " + gold + "\n"
        //+ "Available Units: " + units;
        "Available Units: " + units;
    }
}
