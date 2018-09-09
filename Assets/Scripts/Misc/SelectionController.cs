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
    public Button mainHumanBaseSubjugateBtn;
    public GameObject townActionsContainer;
    public List<Button> townActionBtns;
    public enum Actions { partialFeed, fullFeed, convert, collect, repair, subjugate };
    public enum ActionIconNames { collectWood, collectStone, collectGold, partialFeed, fullFeed, convert, repair, subjugate };
    public List<ActionIcon> actionIconsList;
    public Dictionary<ActionIconNames, GameObject> actionIcons;
    public List<PlannedAction> plannedActions;
    public List<GameObject> plannedActionRemovalIcons;
    public Transform plannedActionsPanel;
    private bool isNightActions;
    private bool hasExecutedPlanned;

    public GameObject planningIndicatorPanel;
    public RawImage portraitPlaceholder;

    public int totalHumanTowns;
    public int subjugatedHumanTowns;

    public bool isTechnologyOpen;

    public GameObject unitCommons;
    public bool isUnitCommonsBuilt;

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

        totalHumanTowns = FindObjectsOfType<HumanTownController>().Length;

        if (TutorialController._tutInstance != null) {
            TutorialController._tutInstance.SetVariables();
        }
	}

    void Update() {
        //calling this to continuously update the subjugation level and button for a town
        SetObjText();
        SetActionButton();
        if (Input.GetMouseButtonDown(0)) {
            Select();
        }

        // Deselect object when it is destroyed (e.g. resource)
        if (selectedObj == null && resourceActionBtn.gameObject.activeInHierarchy) {
            resourceActionBtn.gameObject.SetActive(false);
            selectedObjText.text = "";
        }

        // Allows units to be sent out during the day if technology is researched, else restrict actions to night cycles
        if (SoftMantle._instance.applyTechnology || CloakOfDarkness._instance.applyTechnology) {
            if (!isNightActions) {
                isNightActions = true;
                planningIndicatorPanel.SetActive(false);
                SetActionButtonsOnClick(true);

                if (!hasExecutedPlanned) {
                    hasExecutedPlanned = true;
                    StartCoroutine("ExecutePlannedActions");
                }
            }
        } else {
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

        if (Input.GetKeyDown(KeyCode.O)) {
            BaseController._instance.TakeDamage(100);
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            Subjugation._instance.researched = true;
        }
    }
    
    // Gets object that was clicked on and makes it selected
    public void Select() {
        if (!isTechnologyOpen) {
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

                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Resource") || hit.transform.gameObject.tag == "HumanTown" || hit.transform.gameObject.tag == "Base" || hit.transform.gameObject.tag == "HumanBase") {
                        SetActionButton();
                    }

                    SetObjText();
                    SetObjPortrait();
                }
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
        SetObjPortrait();
        //BaseController._instance.HideCanvas();
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
        if (selectedObj != null) {
            if (selectedObj.tag == "HumanTown") {
                HumanTownController town = selectedObj.GetComponent<HumanTownController>();
                foreach (Button btn in townActionBtns) {
                    btn.interactable = (town.population > 0);
                }

                if (town.population <= 0) {
                    PopupController._instance.SetPopupText("No population to perform actions on");
                }

                resourceActionBtn.gameObject.SetActive(false);
                repairActionBtn.gameObject.SetActive(false);
                mainHumanBaseSubjugateBtn.gameObject.SetActive(false);
                townActionsContainer.SetActive(true);
                if (!Subjugation._instance.researched) {
                    townActionBtns[3].interactable = false;
                } else if (Subjugation._instance.researched && !town.subjugationFinished) {
                    townActionBtns[3].interactable = true;
                } else if (Subjugation._instance.researched && town.subjugationFinished) {
                    townActionBtns[3].interactable = false;
                }
                //BaseController._instance.HideCanvas();
            } else if (selectedObj.tag == "HumanBase") {
                Debug.Log("blah");
                townActionsContainer.SetActive(false);
                resourceActionBtn.gameObject.SetActive(false);
                repairActionBtn.gameObject.SetActive(false);
                mainHumanBaseSubjugateBtn.gameObject.SetActive(true);
                mainHumanBaseSubjugateBtn.interactable = (subjugatedHumanTowns == totalHumanTowns);

            } else if (selectedObj.tag == "Base") {
                townActionsContainer.SetActive(false);
                resourceActionBtn.gameObject.SetActive(false);
                repairActionBtn.gameObject.SetActive(true);
                mainHumanBaseSubjugateBtn.gameObject.SetActive(false);
                //BaseController._instance.ShowCanvas();
            } else if (selectedObj.layer == LayerMask.NameToLayer("Resource")) {
                townActionsContainer.SetActive(false);
                repairActionBtn.gameObject.SetActive(false);
                resourceActionBtn.gameObject.SetActive(true);
                mainHumanBaseSubjugateBtn.gameObject.SetActive(false);
                //BaseController._instance.HideCanvas();
            }
        } else {
            townActionsContainer.SetActive(false);
            repairActionBtn.gameObject.SetActive(false);
            resourceActionBtn.gameObject.SetActive(false);
            mainHumanBaseSubjugateBtn.gameObject.SetActive(false);
            //BaseController._instance.HideCanvas();
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
                NavMeshHit hit;
                if (NavMesh.SamplePosition(spawnPoint.position, out hit, 10f, NavMesh.AllAreas)) {
                    SpawnUnit(hit.position, selectedObj, action);
                }
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
                case Actions.subjugate:
                    AddPlannedAction(ActionIconNames.subjugate);
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
                        NavMeshHit hit;
                        if (NavMesh.SamplePosition(spawnPoint.position, out hit, 10f, NavMesh.AllAreas)) {
                            SpawnUnit(hit.position, planned.objectForAction, planned.action);
                            plannedActions.RemoveAt(0);
                            Destroy(plannedActionRemovalIcons[0]);
                            plannedActionRemovalIcons.RemoveAt(0);
                            yield return new WaitForSeconds(0.5f);
                        }
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

    public void SpawnUnit(Vector3 spawnPos, GameObject objectForAction, Actions action) {
        spawnPoint.position = spawnPos;
        GameObject newUnit = Instantiate(unit, spawnPoint);
        UnitController newUnitCont = newUnit.GetComponent<UnitController>();
        newUnitCont.spawnPoint = spawnPoint.gameObject;
        newUnitCont.MoveToAction(objectForAction);
        newUnitCont.action = action;
        ResourceStorage._instance.UpdateResourceText();
    }

    public void ShowAndHidePlannedActions() {
        plannedActionsPanel.GetChild(1).gameObject.SetActive(!plannedActionsPanel.GetChild(1).gameObject.activeInHierarchy);
    }

    // Have unit come back into the base by adding all resources they collected to the resource storage and destroying
    public virtual void ReturnUnit(UnitController unit) {
        availableUnits++;
        maxUnits += unit.humanConvertCollected;
        availableUnits += unit.humanConvertCollected;

        if (maxUnits >= 5 && !isUnitCommonsBuilt) {
            isUnitCommonsBuilt = true;
            GameObject commons = Instantiate(unitCommons);
            commons.transform.SetParent(BaseController._instance.transform);
        }

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
                    + selectedObj.GetComponent<ResourceController>().currentResourceAmt + " " + selectedObj.tag + " available";
            }

            if (selectedObj.tag == "HumanTown" && selectedObj.GetComponent<HumanTownController>().beingSubjugated) {
                Debug.Log("Town selected, yes subjugation");
                selectedObjText.text =
                    "Human Town" + "\n"
                    + "Population: " + (int)selectedObj.GetComponent<HumanTownController>().population + "\n"
                    + "Can feed to restore hunger or kidnap and convert a human. All actions increase threat level" + "\n"
                    + "Subjugation level: " + selectedObj.GetComponent<HumanTownController>().subjugationLevel + "/" + selectedObj.GetComponent<HumanTownController>().subjugationLimit;
            }
            else if (selectedObj.tag == "HumanTown" && selectedObj.GetComponent<HumanTownController>().subjugationFinished) {
                Debug.Log("Town selected, yes subjugation");
                selectedObjText.text =
                    "Human Town" + "\n"
                    + "Population: " + (int)selectedObj.GetComponent<HumanTownController>().population + "\n"
                    + "Can feed to restore hunger or kidnap and convert a human. All actions increase threat level" + "\n"
                    + "Subjugated: provides small regeneration to your hunger";
            }
            else if (selectedObj.tag == "HumanTown" && !selectedObj.GetComponent<HumanTownController>().subjugationFinished) {
                Debug.Log("Town selected, no subjugation");
                selectedObjText.text =
                    "Human Town" + "\n"
                    + "Population: " + (int)selectedObj.GetComponent<HumanTownController>().population + "\n"
                    + "Can feed to restore hunger or kidnap and convert a human. All actions increase threat level";
            }

            if (selectedObj.tag == "HumanBase") {
                selectedObjText.text =
                    "Main Human Base" + "\n"
                    + "Will send out attacks against you based on your level of threat" + "\n"
                    + "Can be subjugated after all towns are subjugated";
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
            if (selectedObj.tag == "HumanBase") {
                portraitPlaceholder.enabled = true;
                PortraitCameraController._instance.following = false;
                PortraitCameraController._instance.SetPosition(selectedObj.transform);
            }
            if (selectedObj.tag == "Base") {//is this supposed to be HumanBase? if so, replace it with the if-rule above
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
            portraitPlaceholder.enabled = false;
            PortraitCameraController._instance.following = false;
            PortraitCameraController._instance.target = null;
        }
    }

    // Change the onClicks of the buttons to either sending a unit out or planning the action
    public void SetActionButtonsOnClick(bool isNight) {
        resourceActionBtn.onClick.RemoveAllListeners();
        repairActionBtn.onClick.RemoveAllListeners();
        mainHumanBaseSubjugateBtn.onClick.RemoveAllListeners();


        foreach (Button btn in townActionBtns) {
            btn.onClick.RemoveAllListeners();
        }

        if (isNight) {
            resourceActionBtn.onClick.AddListener(() => SendUnit(Actions.collect));
            repairActionBtn.onClick.AddListener(() => SendUnit(Actions.repair));
            mainHumanBaseSubjugateBtn.onClick.AddListener(() => SendUnit(Actions.subjugate));
            townActionBtns[0].onClick.AddListener(() => SendUnit(Actions.partialFeed));
            townActionBtns[1].onClick.AddListener(() => SendUnit(Actions.fullFeed));
            townActionBtns[2].onClick.AddListener(() => SendUnit(Actions.convert));
            townActionBtns[3].onClick.AddListener(() => SendUnit(Actions.subjugate));
        } else {
            resourceActionBtn.onClick.AddListener(() => PlanAction(Actions.collect));
            repairActionBtn.onClick.AddListener(() => PlanAction(Actions.repair));
            mainHumanBaseSubjugateBtn.onClick.AddListener(() => PlanAction(Actions.subjugate));
            townActionBtns[0].onClick.AddListener(() => PlanAction(Actions.partialFeed));
            townActionBtns[1].onClick.AddListener(() => PlanAction(Actions.fullFeed));
            townActionBtns[2].onClick.AddListener(() => PlanAction(Actions.convert));
            townActionBtns[3].onClick.AddListener(() => PlanAction(Actions.subjugate));
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
