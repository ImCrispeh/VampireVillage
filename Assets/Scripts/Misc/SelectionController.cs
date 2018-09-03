using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using System;

public class SelectionController : MonoBehaviour {
    public static SelectionController _instance;

    public GameObject selectedObj;
    public GameObject selectedObjectPanel;
    public Text selectedObjText;

    public GameObject unit;
    public int maxUnits;
    public int availableUnits;
    public GameObject unitBase;
    public Transform spawnPoint;

    public Button resourceActionBtn;
    public Button repairActionBtn;
    public GameObject townActionsContainer;
    public List<Button> townActionBtns;
    public enum Actions { partialFeed, fullFeed, convert, collect, repair };
    public enum ActionIconNames { collectWood, collectStone, collectGold, partialFeed, fullFeed, convert, repair };
    public List<ActionIcon> actionIconsList;
    private Dictionary<ActionIconNames, GameObject> actionIcons;
    public List<PlannedAction> plannedActions;
    public List<GameObject> plannedActionRemovalIcons;
    public Transform plannedActionsPanel;
    private bool isNightActions;
    private bool hasExecutedPlanned;

    public GameObject planningIndicatorPanel;
    public RawImage portraitPlaceholder;

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
        actionIcons = new Dictionary<ActionIconNames, GameObject>();

        foreach (ActionIcon icon in actionIconsList) {
            actionIcons.Add(icon.iconName, icon.icon);
        }

        availableUnits = 1 - spawnPoint.childCount;
        ResourceStorage._instance.UpdateResourceText();
        portraitPlaceholder = GameObject.Find("Canvas/BottomBar/InformationWindow/PortraitPlaceholder").GetComponent<RawImage>();
        portraitPlaceholder.enabled = false;
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
            planningIndicatorPanel.SetActive(false);
            SetActionButtonsOnClick(true);
            
            if (!hasExecutedPlanned) {
                hasExecutedPlanned = true;
                StartCoroutine("ExecutePlannedActions");
            }

        } else if ((Timer._instance.currentTime <= 0.75f && Timer._instance.currentTime >= 0.25f) && isNightActions) {
            isNightActions = false;
            planningIndicatorPanel.SetActive(true);
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

                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Resource") || hit.transform.gameObject.tag == "HumanTown" || hit.transform.gameObject.tag == "Base") {
                    SetActionButton();
                }

                SetObjText();
                SetObjPortrait();
            }
        }
    }

    // Revert material of previously selected objectoon
    public void DeselectObj() {
        foreach (Renderer rend in selectedObj.transform.GetComponentsInChildren<Renderer>()) {
            foreach (Material mat in rend.materials) {
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 1f);
            }
        }
        selectedObj = null;
        repairActionBtn.gameObject.SetActive(false);
        resourceActionBtn.gameObject.SetActive(false);
        townActionsContainer.SetActive(false);
        SetObjText();
    }

    // Change material of object to indicate that it is selected
    public void HighlightSelected(RaycastHit hit) {
        selectedObj = hit.transform.gameObject;
        foreach (Renderer rend in selectedObj.transform.GetComponentsInChildren<Renderer>()) {
            if (rend.gameObject.GetComponent<NavMeshAgent>() == null) {
                foreach (Material mat in rend.materials) {
                    mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.3f);
                }
            }
        }
    }

    // Brings up action buttons based on what object is selected
    public void SetActionButton() {
        if (selectedObj.tag == "HumanTown") {
            resourceActionBtn.gameObject.SetActive(false);
            repairActionBtn.gameObject.SetActive(false);
            townActionsContainer.SetActive(true);
        } else if (selectedObj.tag == "Base") {
            townActionsContainer.SetActive(false);
            resourceActionBtn.gameObject.SetActive(false);
            repairActionBtn.gameObject.SetActive(true);
        } else {
            townActionsContainer.SetActive(false);
            repairActionBtn.gameObject.SetActive(false);
            resourceActionBtn.gameObject.SetActive(true);
        }
    }

    // Sends unit out to execute action
    public void SendUnit(Actions action) {
        if (availableUnits > 0) {
            if (action == Actions.repair && selectedObj.GetComponent<BaseController>().IsFullHealth()) {
                PopupController._instance.SetPopupText("Structure already at full health");
            } else if (action == Actions.repair && (ResourceStorage._instance.wood < 3 || ResourceStorage._instance.stone < 3)) {
                PopupController._instance.SetPopupText("Not enough resources to repair");
            } else {
                availableUnits--;
                GameObject newUnit = Instantiate(unit, spawnPoint);
                UnitController newUnitCont = newUnit.GetComponent<UnitController>();
                newUnitCont.spawnPoint = spawnPoint.gameObject;
                newUnitCont.MoveToAction(selectedObj);
                newUnitCont.action = action;
                ResourceStorage._instance.UpdateResourceText();
            }
        } else {
            PlanAction(action);
            StopAllCoroutines();
            StartCoroutine("ExecutePlannedActions");
        }
    }

    // Add action + specified object to the plannedActions list for execution once the night cycle comes
    public void PlanAction(Actions action) {
        if (plannedActions.Count < 30) {
            PlannedAction planned = new PlannedAction();
            planned.action = action;
            planned.objectForAction = selectedObj;
            plannedActions.Add(planned);

            switch (action) {
                case Actions.collect:
                    switch (selectedObj.tag) {
                        case "Wood":
                            AddPlannedAction(ActionIconNames.collectWood);
                            break;
                        case "Stone":
                            AddPlannedAction(ActionIconNames.collectStone);
                            break;
                        case "Gold":
                            AddPlannedAction(ActionIconNames.collectGold);
                            break;
                        default:
                            break;
                    }
                    break;
                case Actions.partialFeed:
                    AddPlannedAction(ActionIconNames.partialFeed);
                    break;
                case Actions.fullFeed:
                    AddPlannedAction(ActionIconNames.fullFeed);
                    break;
                case Actions.convert:
                    AddPlannedAction(ActionIconNames.convert);
                    break;
                case Actions.repair:
                    AddPlannedAction(ActionIconNames.repair);
                    break;
                default:
                    break;
            }
        } else {
            PopupController._instance.SetPopupText("Max 30 planned actions");
        }
    }

    public void AddPlannedAction(ActionIconNames icon) {
        GameObject newAction;
        newAction = Instantiate(actionIcons[icon], plannedActionsPanel.GetChild(1));
        newAction.GetComponent<Button>().onClick.AddListener(RemovePlannedAction);
        plannedActionRemovalIcons.Add(newAction);
    }

    public void RemovePlannedAction() {
        GameObject toRemove = EventSystem.current.currentSelectedGameObject;
        int pos = plannedActionRemovalIcons.IndexOf(toRemove);
        plannedActions.RemoveAt(pos);
        plannedActionRemovalIcons.RemoveAt(pos);
        Destroy(toRemove);
    }

    // Sends units to complete actions in plannedActions list with a small delay between each unit being sent out (just so they don't all spawn at the same time)
    IEnumerator ExecutePlannedActions() {
        while (plannedActions.Count > 0) {
            if (isNightActions) {
                if (availableUnits > 0) {
                    PlannedAction planned = plannedActions[0];
                    if (planned.action == Actions.repair && planned.objectForAction.GetComponent<BaseController>().IsFullHealth()) {
                        PopupController._instance.SetPopupText("Structure already at full health. Skipping...");
                        plannedActions.RemoveAt(0);
                        Destroy(plannedActionRemovalIcons[0]);
                        plannedActionRemovalIcons.RemoveAt(0);
                    } else if (planned.action == Actions.repair && (ResourceStorage._instance.wood < 3 || ResourceStorage._instance.stone < 3)) {
                        PopupController._instance.SetPopupText("Not enough resources to repair. Skipping...");
                        plannedActions.RemoveAt(0);
                        Destroy(plannedActionRemovalIcons[0]);
                        plannedActionRemovalIcons.RemoveAt(0);
                    } else {
                        availableUnits--;
                        GameObject newUnit = Instantiate(unit, spawnPoint);
                        UnitController newUnitCont = newUnit.GetComponent<UnitController>();
                        newUnitCont.spawnPoint = spawnPoint.gameObject;
                        newUnitCont.MoveToAction(planned.objectForAction);
                        newUnitCont.action = planned.action;
                        ResourceStorage._instance.UpdateResourceText();
                        plannedActions.RemoveAt(0);
                        Destroy(plannedActionRemovalIcons[0]);
                        plannedActionRemovalIcons.RemoveAt(0);
                        yield return new WaitForSeconds(0.5f);
                    }
                } else {
                    yield return null;
                }
            } else {
                PopupController._instance.SetPopupText("Cannot sent units out during the day. Pausing remaining actions...");
                yield break;
            }
        }
        yield return null;
    }

    public void ShowAndHidePlannedActions() {
        plannedActionsPanel.GetChild(1).gameObject.SetActive(!plannedActionsPanel.GetChild(1).gameObject.activeInHierarchy);
    }

    // Have unit come back into the base by adding all resources they collected to the resource storage and destroying
    public virtual void ReturnUnit(UnitController unit) {
        availableUnits++;
        maxUnits += unit.humanConvertCollected;
        availableUnits += unit.humanConvertCollected;
        ResourceStorage._instance.AddWood(unit.woodCollected);
        ResourceStorage._instance.AddHunger(unit.hungerCollected);
        ResourceStorage._instance.AddStone(unit.stoneCollected);
        ResourceStorage._instance.AddGold(unit.goldCollected);
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
            portraitPlaceholder.enabled = false;
            PortraitCameraController._instance.following = false;
            PortraitCameraController._instance.target = null;
            Debug.Log("remove text");
        }
    }

    //sets the portrait in the bottombar depending on what is clicked, currently only uses a placeholder portrait
    public void SetObjPortrait() {
        portraitPlaceholder.enabled = !portraitPlaceholder.enabled;
        if (selectedObj != null) {
            if (selectedObj.layer == LayerMask.NameToLayer("Resource")) {
                if (selectedObj.tag == "Wood") {
                    portraitPlaceholder.enabled = true;
                    PortraitCameraController._instance.following = false;
                    PortraitCameraController._instance.SetPosition(selectedObj.transform);
                }
                if (selectedObj.tag == "Stone") {
                    portraitPlaceholder.enabled = true;
                    PortraitCameraController._instance.following = false;
                    PortraitCameraController._instance.SetPosition(selectedObj.transform);
                }
                if (selectedObj.tag == "Gold") {
                    portraitPlaceholder.enabled = true;
                    PortraitCameraController._instance.following = false;
                    PortraitCameraController._instance.SetPosition(selectedObj.transform);
                }
            }
            if (selectedObj.tag == "HumanTown") {
                portraitPlaceholder.enabled = true;
                PortraitCameraController._instance.following = false;
                PortraitCameraController._instance.SetPosition(selectedObj.transform);
            }
            if (selectedObj.tag == "Base") {
                portraitPlaceholder.enabled = true;
                PortraitCameraController._instance.following = false;
                PortraitCameraController._instance.SetPosition(selectedObj.transform);
            }
            if (selectedObj.tag == "Enemy") {
                portraitPlaceholder.enabled = true;
                PortraitCameraController._instance.following = true;
                PortraitCameraController._instance.target = selectedObj.transform;
                PortraitCameraController._instance.Update();
            }
        }
        else {
            Debug.Log("fucking work please");
        }
    }

    // Change the onClicks of the buttons to either sending a unit out or planning the action
    public void SetActionButtonsOnClick(bool isNight) {
        resourceActionBtn.onClick.RemoveAllListeners();
        repairActionBtn.onClick.RemoveAllListeners();

        foreach (Button btn in townActionBtns) {
            btn.onClick.RemoveAllListeners();
        }

        if (isNight) {
            resourceActionBtn.onClick.AddListener(() => SendUnit(Actions.collect));
            repairActionBtn.onClick.AddListener(() => SendUnit(Actions.repair));
            townActionBtns[0].onClick.AddListener(() => SendUnit(Actions.partialFeed));
            townActionBtns[1].onClick.AddListener(() => SendUnit(Actions.fullFeed));
            townActionBtns[2].onClick.AddListener(() => SendUnit(Actions.convert));
        } else {
            resourceActionBtn.onClick.AddListener(() => PlanAction(Actions.collect));
            repairActionBtn.onClick.AddListener(() => PlanAction(Actions.repair));
            townActionBtns[0].onClick.AddListener(() => PlanAction(Actions.partialFeed));
            townActionBtns[1].onClick.AddListener(() => PlanAction(Actions.fullFeed));
            townActionBtns[2].onClick.AddListener(() => PlanAction(Actions.convert));
        }
    }

    public struct PlannedAction {
        public Actions action;
        public GameObject objectForAction;
    }

    [Serializable]
    public struct ActionIcon {
        public ActionIconNames iconName;
        public GameObject icon;
    }
}
