using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour {
    public static TutorialController _instance;
    public int currText;
    public bool isPaused;
    public GameObject[] tutorialTexts;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    // Use this for initialization
    void Start () {
        isPaused = true;
        Timer._instance.PauseTimer();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeText() {
        tutorialTexts[currText].SetActive(false);

        if (currText < tutorialTexts.Length - 1) {
            currText++;
            tutorialTexts[currText].SetActive(true);
        }
    }
}
