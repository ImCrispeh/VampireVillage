using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


// Duplicating the SelectionController script otherwise there would probably have to be a check if it's the tutorial in the Update of SelectionController
// which would only be relevant for the start of the game. Doing it this way, we can delete all tutorial-related objects after it is done and not
// have to check
public class TutorialController : SelectionController {
    public static TutorialController _tutInstance;
    public int currText;
    public GameObject[] tutorialTexts;

    public GameObject selectionCont;

    private void Awake() {
        if (_tutInstance != null && _tutInstance != this) {
            Destroy(gameObject);
        } else {
            _tutInstance = this;
        }
    }

    // Use this for initialization
    void Start () {
        Timer._instance.PauseTimer();
    }
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();

        if (Input.GetKeyDown(KeyCode.A)) {
            ChangeText();
        }

        if (Input.GetMouseButtonDown(0) && EventSystem.current.currentSelectedGameObject == actionBtn.gameObject) {
            if (availableUnits > 0) {
                Timer._instance.UnpauseTimer();
            }
        }
    }

    public override void ReturnUnit(UnitController unit) {
        ChangeText();
        base.ReturnUnit(unit);
    }

    public void ChangeText() {
        if (selectedObj != null) {
            DeselectObj();
        }

        tutorialTexts[currText].SetActive(false);

        if (currText < tutorialTexts.Length - 1) {
            currText++;
            tutorialTexts[currText].SetActive(true);
            Timer._instance.PauseTimer();
        } else {
            Timer._instance.UnpauseTimer();
            selectionCont.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
