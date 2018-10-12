using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThreatController : MonoBehaviour {
    public static ThreatController _instance;

    public Slider threatBar;
    public float maxThreat;
    public float minThreat;
    public float threat;
    public int threatLevel;
    public float threatToDeplete;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    void Start () {
        maxThreat = 100f;
        threat = 0f;
        minThreat = 0f;
        threatBar = GetComponent<Slider>();
	}
	
	void Update () {

	}

    public void AddThreat(float amt) {
        threat += amt;
        threat = Mathf.Clamp(threat, minThreat, maxThreat);
        SetThreatLevel();
        threatBar.value = HungerPercentage();
    }

    public void SubtractThreat() {
        if (threatLevel != 0) {
            threat -= threatToDeplete / threatLevel;
        }

        threat = Mathf.Clamp(threat, minThreat, maxThreat);
        SetThreatLevel();
        threatBar.value = HungerPercentage();
    }

    public float HungerPercentage() {
        return threat / maxThreat;
    }

    public void SetThreatLevel() {
        if (threat == 0) {
            threatLevel = 0;
        } else if (threat < 25f) {
            threatLevel = 1;
        } else if (threat < 50f) {
            threatLevel = 2;
        } else if (threat < 75f) {
            threatLevel = 3;
        } else if (threat < 100f) {
            threatLevel = 4;
        } else if (threat == 100f) {
            threatLevel = 5;
        }
    }
}
