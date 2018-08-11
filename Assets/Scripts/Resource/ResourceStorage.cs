﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceStorage : MonoBehaviour {
    public static ResourceStorage _instance;

    public float maxHunger;
    public float hunger;
    public int wood;

    public Text resourceText;
    public GameObject test;
    public Slider hungerBar;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    void Start () {
        maxHunger = 100f;
        UpdateResourceText();
        hungerBar.value = HungerPercentage();
    }
	
	void Update () {
        if (!Timer._instance.isPaused) {
            SubtractHunger(0.005f);
        }
	}

    public void AddHunger(float amt) {
        BaseController._instance.isHungerEmpty = false;
        hunger += amt;
        hunger = Mathf.Clamp(hunger, 0f, maxHunger);
        hungerBar.value = HungerPercentage();
    }

    public void SubtractHunger(float amt) {
        if (hunger <= 0f) {
            BaseController._instance.isHungerEmpty = true;
        }

        hunger -= amt;
        hunger = Mathf.Clamp(hunger, 0f, maxHunger);
        hungerBar.value = HungerPercentage();
    }

    public float HungerPercentage() {
        return hunger / maxHunger;
    }

    public void AddWood(int amt) {
        wood += amt;
    }

    public void SubtractWood(int amt) {
        wood -= amt;
    }

    public void UpdateResourceText() {
        int units;

        if (SelectionController._instance != null) {
            units = SelectionController._instance.availableUnits;
        } else {
            units = TutorialController._tutInstance.availableUnits;
        }

        resourceText.text =
        "Hunger: " + hunger + "\n"
        + "Wood: " + wood + "\n"
        + "Available Units: " + units;
    }
}
