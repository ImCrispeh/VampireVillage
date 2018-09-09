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
    public GameObject[] feedTutIndicators;
    public GameObject[] woodTutIndicators;

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
        techBtn.SetActive(false);
        SetActionButtonsOnClick(true);
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Select();
        }

        if (currText == 0 && availableUnits > 0) {
            townActionBtns[0].interactable = false;
            townActionBtns[1].interactable = true;
            townActionBtns[2].interactable = false;
            townActionBtns[3].interactable = false;
        } else {
            foreach (Button btn in townActionBtns) {
                btn.interactable = false;
            }
        }

        if (currText == 2 && availableUnits > 0) {
            if (selectedObj != null) {
                if (selectedObj.tag == "Wood") {
                    resourceActionBtn.interactable = true;
                }
            }
        } else {
            resourceActionBtn.interactable = false;
        }

        repairActionBtn.interactable = false;

        // Deselect object when it is destroyed (e.g. resource)
        if (selectedObj == null && resourceActionBtn.gameObject.activeInHierarchy) {
            resourceActionBtn.gameObject.SetActive(false);
            selectedObjText.text = "";
        }

        if (Input.GetMouseButtonDown(0)) {
            Select();
        }

        // Deselect object when it is destroyed (e.g. resource)
        if (selectedObj == null && resourceActionBtn.gameObject.activeInHierarchy) {
            resourceActionBtn.gameObject.SetActive(false);
            selectedObjText.text = "";
        }

        if (Input.GetMouseButtonDown(0) && (EventSystem.current.currentSelectedGameObject == resourceActionBtn.gameObject || EventSystem.current.currentSelectedGameObject == townActionBtns[1].gameObject)) {
            if (availableUnits > 0) {
                Timer._instance.UnpauseTimer();
                HideText();
            }
        }

        if (Input.GetMouseButtonDown(0) && EventSystem.current.currentSelectedGameObject == techBtn && currText == tutorialTexts.Length - 2) {
            HideText();
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            ChangeText();
        }
    }

    public void SetVariables() {
        SelectionController cont = selectionCont.GetComponent<SelectionController>();

        selectedObjectPanel = cont.selectedObjectPanel;
        selectedObjText = cont.selectedObjText;
        unitBase = cont.unitBase;
        spawnPoint = cont.spawnPoint;
        resourceActionBtn = cont.resourceActionBtn;
        repairActionBtn = cont.repairActionBtn;
        townActionsContainer = cont.townActionsContainer;
        townActionBtns = cont.townActionBtns;
        actionIconsList = cont.actionIconsList;

        actionIcons = new Dictionary<ActionIconNames, GameObject>();

        foreach (ActionIcon icon in actionIconsList) {
            actionIcons.Add(icon.iconName, icon.icon);
        }

        plannedActions = cont.plannedActions;
        plannedActionRemovalIcons = cont.plannedActionRemovalIcons;
        plannedActionsPanel = cont.plannedActionsPanel;
        planningIndicatorPanel = cont.planningIndicatorPanel;
        portraitPlaceholder = cont.portraitPlaceholder;

        selectionCont.SetActive(false);

        resourceActionBtn.onClick.AddListener(() => SendUnit(Actions.collect));
        townActionBtns[1].onClick.AddListener(() => SendUnit(Actions.fullFeed));
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

            if (currText == 2) {
                foreach (GameObject obj in woodTutIndicators) {
                    obj.SetActive(true);
                }
            }

            // Keep tech button hidden until it gets to the tutorial step about it
            if (currText == 3) {
                techBtn.SetActive(true);
            }

            // Make it so the final message does not pause the timer (since there is no action to take)
            Timer._instance.PauseTimer();

        } else {
            Timer._instance.UnpauseTimer();
            selectionCont.SetActive(true);
            resourceActionBtn.interactable = true;
            repairActionBtn.interactable = true;
            Destroy(this.gameObject);
        }
    }

    public void HideText() {
        if (currText == 0) {
            foreach (GameObject obj in feedTutIndicators) {
                obj.SetActive(false);
            }
        }

        if (currText == 2) {
            foreach (GameObject obj in woodTutIndicators) {
                obj.SetActive(false);
            }
        }
        textBackground.enabled = false;
        tutorialTexts[currText].SetActive(false);
    }
}
