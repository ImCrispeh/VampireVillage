using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectionController : MonoBehaviour {
    public static SelectionController _instance;

    public Material selectedObjMat;
    public Color32 selectedMatColor;
    public GameObject selectedObj;
    public Material matToUse;
    public GameObject selectedObjectPanel;
    public Text selectedObjText;

    public GameObject unit;
    public int maxUnits;
    public int availableUnits;
    public GameObject unitBase;
    public Transform spawnPoint;

    public Button resourceActionBtn;
    public GameObject townActionsContainer;
    public List<Button> townActionBtns;
    public enum Actions { partialFeed, fullFeed, convert, collect };
    public List<PlannedAction> plannedActions;
    public bool isNightActions;
    public bool hasExecutedPlanned;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    // Subtracts child count so it gets the correct amount of available units when it becomes active (in place of the TutorialController)
    void Start () {

        foreach (Button btn in townActionBtns) {
            btn.interactable = true;
        }

        plannedActions = new List<PlannedAction>();

        availableUnits = 1 - spawnPoint.childCount;
        ResourceStorage._instance.UpdateResourceText();
	}
	
	protected virtual void Update () {
		if (Input.GetMouseButtonDown(0)) {
            Select();
        }

        // Deselect object when it is destroyed (e.g. resource)
        if (selectedObj == null && resourceActionBtn.gameObject.activeInHierarchy) {
            resourceActionBtn.gameObject.SetActive(false);
            selectedObjText.text = "";
        }

        if ((Timer._instance.currentTime >= 0.75f || Timer._instance.currentTime <= 0.25f) && !isNightActions) {
            isNightActions = true;
            SetActionButtonsOnClick(true);
            
            if (!hasExecutedPlanned) {
                hasExecutedPlanned = true;
                StartCoroutine("ExecutePlannedActions");
            }

        } else if ((Timer._instance.currentTime <= 0.75f && Timer._instance.currentTime >= 0.25f) && isNightActions) {
            isNightActions = false;
            hasExecutedPlanned = false;
            SetActionButtonsOnClick(false);
        }
	}
    
    // Gets object that was clicked on and makes it selected
    public void Select() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit)) {

            // Remove highlight from previously selected object and deselect object (only if not cliking on the UI)
            if (EventSystem.current.IsPointerOverGameObject()) {
                return;
            } else if (selectedObj != null) {
                DeselectObj();
            }

            if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Ground")) {
                HighlightSelected(hit);

                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Resource") || hit.transform.gameObject.tag == "HumanTown") {
                    SetActionButton();
                }

                SetObjText();
            }
        }
    }

    // Revert material of previously selected object
    public void DeselectObj() {
        selectedObj.GetComponent<Renderer>().material = selectedObjMat;
        selectedObj = null;
        selectedObjMat = null;
        resourceActionBtn.gameObject.SetActive(false);
        townActionsContainer.SetActive(false);
        SetObjText();
    }

    // Change material of object to indicate that it is selected
    public void HighlightSelected(RaycastHit hit) {
        selectedObj = hit.transform.gameObject;
        selectedObjMat = hit.transform.gameObject.GetComponent<Renderer>().material;
        selectedMatColor = selectedObjMat.color;

        matToUse.color = new Color32(selectedMatColor.r, selectedMatColor.g, selectedMatColor.b, 190);
        selectedObj.GetComponent<Renderer>().material = matToUse;
    }

    // Brings up action buttons based on what object is selected
    public void SetActionButton() {
        if (selectedObj.tag == "HumanTown") {
            resourceActionBtn.gameObject.SetActive(false);
            townActionsContainer.SetActive(true);
        } else {
            townActionsContainer.SetActive(false);
            resourceActionBtn.gameObject.SetActive(true);
        }
    }

    // Sends unit out to execute action
    public void SendUnit(Actions action) {
        if (availableUnits > 0) {
                availableUnits--;
                GameObject newUnit = Instantiate(unit, spawnPoint);
                UnitController newUnitCont = newUnit.GetComponent<UnitController>();
                newUnitCont.unitBase = unitBase;
                newUnitCont.MoveToAction(selectedObj);
                newUnitCont.action = action;
                ResourceStorage._instance.UpdateResourceText();
        } else {
            ErrorController._instance.SetErrorText("No units available");
        }
    }

    // Add action + specified object to the plannedActions list for execution once the night cycle comes
    public void PlanAction(Actions action) {
        PlannedAction planned = new PlannedAction();
        planned.action = action;
        planned.objectForAction = selectedObj;
        plannedActions.Add(planned);
        Debug.Log(planned.action + " on " + planned.objectForAction);
    }

    // Sends units to complete actions in plannedActions list with a small delay between each unit being sent out (just so they don't all spawn at the same time)
    IEnumerator ExecutePlannedActions() {
        while (plannedActions.Count > 0) {
            if (availableUnits > 0) {
                PlannedAction planned = plannedActions[0];
                availableUnits--;
                GameObject newUnit = Instantiate(unit, spawnPoint);
                UnitController newUnitCont = newUnit.GetComponent<UnitController>();
                newUnitCont.unitBase = unitBase;
                newUnitCont.MoveToAction(planned.objectForAction);
                newUnitCont.action = planned.action;
                ResourceStorage._instance.UpdateResourceText();
                plannedActions.RemoveAt(0);
                yield return new WaitForSeconds(0.5f);
            } else {
                yield return null;
            }
        }
        yield return null;
    }

    // Have unit come back into the base by adding all resources they collected to the resource storage and destroying
    public virtual void ReturnUnit(UnitController unit) {
        availableUnits++;
        maxUnits += unit.humanConvertCollected;
        availableUnits += unit.humanConvertCollected;
        ResourceStorage._instance.AddWood(unit.woodCollected);
        ResourceStorage._instance.AddHunger(unit.hungerCollected);
        ResourceStorage._instance.UpdateResourceText();
        Destroy(unit.gameObject);
    }

    // Show description of selected object, remove description box if no item is selected
    public void SetObjText() {
        if (selectedObj != null) {
            selectedObjectPanel.SetActive(true);
            if (selectedObj.layer == LayerMask.NameToLayer("Resource")) {
                selectedObjText.text =
                    selectedObj.tag + "\n"
                    + selectedObj.GetComponent<ResourceController>().resourceAmt + " " + selectedObj.tag + " available";
            }

            if (selectedObj.tag == "HumanTown") {
                selectedObjText.text =
                    "Human Town" + "\n"
                    + "Can feed to restore hunger or kidnap and convert a human. All actions increase threat level";
            }

            if (selectedObj.tag == "Base") {
                selectedObjText.text =
                    "Main Base" + "\n"
                    + "Health: " + BaseController._instance.health + "\n"
                    + "Attack level: " + BaseController._instance.attack + "\n"
                    + "Defense level: " + BaseController._instance.defense;
            }

            if (selectedObj.tag == "Enemy") {
                selectedObjText.text =
                    "Enemy" + "\n"
                    + "Health: " + selectedObj.GetComponent<EnemyController>().health + "\n"
                    + "Attack level: " + selectedObj.GetComponent<EnemyController>().attack;
            }
        } else {
            selectedObjText.text = "";
            selectedObjectPanel.SetActive(false);
        }
    }

    // Change the onClicks of the buttons to either sending a unit out or planning the action
    public void SetActionButtonsOnClick(bool isNight) {
        resourceActionBtn.onClick.RemoveAllListeners();

        foreach (Button btn in townActionBtns) {
            btn.onClick.RemoveAllListeners();
        }

        if (isNight) {
            resourceActionBtn.onClick.AddListener(() => SendUnit(Actions.collect));
            townActionBtns[0].onClick.AddListener(() => SendUnit(Actions.partialFeed));
            townActionBtns[1].onClick.AddListener(() => SendUnit(Actions.fullFeed));
            townActionBtns[2].onClick.AddListener(() => SendUnit(Actions.convert));
        } else {
            resourceActionBtn.onClick.AddListener(() => PlanAction(Actions.collect));
            townActionBtns[0].onClick.AddListener(() => PlanAction(Actions.partialFeed));
            townActionBtns[1].onClick.AddListener(() => PlanAction(Actions.fullFeed));
            townActionBtns[2].onClick.AddListener(() => PlanAction(Actions.convert));
        }
    }

    public struct PlannedAction {
        public Actions action;
        public GameObject objectForAction;
    }
}
