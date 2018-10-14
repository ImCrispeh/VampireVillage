using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class ResourceStorage : MonoBehaviour {
    public static ResourceStorage _instance;

    public float maxHunger;
    public float hunger;
    public int wood;
    public int stone;
    public int gold;
    public int collectionModifier;

    public float hungerDepletionRate;
	public float hungerDepletionRateModifier;
	public float numberOfDays;
	public float dayMultiplier;
	public float unitMultiplier;

    public AudioClip hungerLowAudio;

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
		unitMultiplier = 1;
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
            SoundManager.instance.StopHunger();
            hasHungerWarningShown = false;
        }
        hungerBar.value = HungerPercentage();
    }

    public void SubtractHunger() {
        if (SelectionController._instance != null) {
			CalculateUnitModifier();
			CalculateDayModifier();
            if (SoftMantle._instance.applyTechnology && !CloakOfDarkness._instance.applyTechnology) {
                if ((Timer._instance.currentTime <= 0.75f && Timer._instance.currentTime >= 0.25f) && SelectionController._instance.availableUnits < SelectionController._instance.maxUnits) {
					hunger -= hungerDepletionRate * hungerDepletionRateModifier * unitMultiplier* dayMultiplier * 2;
                } else {
					hunger -= hungerDepletionRate * hungerDepletionRateModifier * unitMultiplier * dayMultiplier;
                }
            } else {
				hunger -= hungerDepletionRate * hungerDepletionRateModifier * unitMultiplier * dayMultiplier;
            }
        } else {
			hunger -= hungerDepletionRate * hungerDepletionRateModifier * unitMultiplier;
        }

        hunger = Mathf.Clamp(hunger, 0f, maxHunger);
        hungerBar.value = HungerPercentage();

        if (hunger <= 25f) {
            if (!hasHungerWarningShown) {
                hasHungerWarningShown = true;
                SoundManager.instance.HungerBeat(hungerLowAudio);
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

	private void CalculateUnitModifier() {
		if (SelectionController._instance.maxUnits <= 7) {
			unitMultiplier = 1 + (SelectionController._instance.maxUnits - 2) / 10;
			//Debug.Log (SelectionController._instance.maxUnits + " Unit:" + unitMultiplier);
		} else {
			unitMultiplier = 1.5f;
		}
	}

	private void CalculateDayModifier() {
		if (Timer._instance.currentDay <= 6) {
			dayMultiplier = 3;
		} else if (Timer._instance.currentDay <= 20) {
			dayMultiplier = 1 + Timer._instance.currentDay / 2;
		} else {
			dayMultiplier = 11f;
		}
		//Debug.Log (Timer._instance.currentDay + " Day:" + dayMultiplier);
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
