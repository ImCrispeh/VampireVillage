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
    public Image textBackground;
    public GameObject[] tutorialTexts;

    public GameObject selectionCont;
    public GameObject techBtn;

    private void Awake() {
        if (_tutInstance != null && _tutInstance != this) {
            Destroy(gameObject);
        } else {
            _tutInstance = this;
        }
    }

    void Start() {
        Timer._instance.PauseTimer();
    }

    protected override void Update() {
        base.Update();

        if (Input.GetMouseButtonDown(0) && EventSystem.current.currentSelectedGameObject == actionBtn.gameObject) {
            if (availableUnits > 0) {
                Timer._instance.UnpauseTimer();
                HideText();
            }
        }

        if (Input.GetMouseButtonDown(0) && EventSystem.current.currentSelectedGameObject == techBtn && currText == tutorialTexts.Length - 2) {
            HideText();
        }
    }

    public override void ReturnUnit(UnitController unit) {
        ChangeText();
        base.ReturnUnit(unit);
    }

    public void ChangeText() {
        Debug.Log("change");
        if (selectedObj != null) {
            DeselectObj();
        }

        HideText();

        if (currText < tutorialTexts.Length - 1) {
            currText++;
            textBackground.enabled = true;
            tutorialTexts[currText].SetActive(true);

            // Keep tech button hidden until it gets to the tutorial step about it
            if (currText == tutorialTexts.Length - 2) {
                techBtn.SetActive(true);
            }

            // Make it so the final message does not pause the timer (since there is no action to take)
            if (currText < tutorialTexts.Length - 1) {
                Timer._instance.PauseTimer();
            }

        } else {
            Timer._instance.UnpauseTimer();
            selectionCont.SetActive(true);
            actionBtn.onClick.RemoveAllListeners();
            actionBtn.onClick.AddListener(selectionCont.GetComponent<SelectionController>().SendUnit);
            Destroy(this.gameObject);
        }
    }

    public void HideText() {
        textBackground.enabled = false;
        tutorialTexts[currText].SetActive(false);
    }
}
