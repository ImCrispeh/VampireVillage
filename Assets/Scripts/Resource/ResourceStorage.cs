using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceStorage : MonoBehaviour {
    public static ResourceStorage _instance;

    public int hunger;
    public int wood;

    public Text resourceText;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    // Use this for initialization
    void Start () {
        UpdateResourceText();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddHunger(int amt) {
        hunger += amt;
        hunger = Mathf.Clamp(hunger, 0, 100);
    }

    public void SubtractHunger(int amt) {
        hunger -= amt;
        hunger = Mathf.Clamp(hunger, 0, 100);
    }

    public void AddWood(int amt) {
        wood += amt;
    }

    public void SubtractWood(int amt) {
        wood -= amt;
    }

    public void UpdateResourceText() {
        resourceText.text =
            "Hunger: " + hunger + "\n"
            + "Wood: " + wood;
    }
}
