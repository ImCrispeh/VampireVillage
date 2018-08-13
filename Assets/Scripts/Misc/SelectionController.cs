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
    public Text selectedObjText;

    public GameObject unit;
    public int availableUnits;
    public GameObject unitBase;
    public Transform spawnPoint;

    public Button resourceActionBtn;
    public GameObject townActionsContainer;
    public List<Button> townActionBtns;
    public enum Actions { partialFeed, fullFeed, convert, collect };

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    // Subtracts child count so it gets the correct amount of available units when it becomes active (in place of the TutorialController)
    void Start () {
        // Set buttons to work from SelectionController rather than TutorialController
        resourceActionBtn.onClick.RemoveAllListeners();
        resourceActionBtn.onClick.AddListener(() => SendUnit(Actions.collect));

        townActionBtns[1].onClick.RemoveAllListeners();
        townActionBtns[0].onClick.AddListener(() => SendUnit(Actions.partialFeed));
        townActionBtns[1].onClick.AddListener(() => SendUnit(Actions.fullFeed));
        townActionBtns[2].onClick.AddListener(() => SendUnit(Actions.convert));
        foreach (Button btn in townActionBtns) {
            btn.interactable = true;
        }

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

    public void DeselectObj() {
        selectedObj.GetComponent<Renderer>().material = selectedObjMat;
        selectedObj = null;
        selectedObjMat = null;
        resourceActionBtn.gameObject.SetActive(false);
        townActionsContainer.SetActive(false);
        selectedObjText.text = "";
    }

    public void HighlightSelected(RaycastHit hit) {
        selectedObj = hit.transform.gameObject;
        selectedObjMat = hit.transform.gameObject.GetComponent<Renderer>().material;
        selectedMatColor = selectedObjMat.color;

        matToUse.color = new Color32(selectedMatColor.r, selectedMatColor.g, selectedMatColor.b, 190);
        selectedObj.GetComponent<Renderer>().material = matToUse;
    }

    public void SetActionButton() {
        if (selectedObj.tag == "HumanTown") {
            resourceActionBtn.gameObject.SetActive(false);
            townActionsContainer.SetActive(true);
        } else {
            townActionsContainer.SetActive(false);
            resourceActionBtn.gameObject.SetActive(true);
        }
    }

    public void SendUnit(Actions action) {
        if (availableUnits > 0) {
            if (Timer._instance.currentTime >= 0.75f || Timer._instance.currentTime <= 0.2f) {
                availableUnits--;
                GameObject newUnit = Instantiate(unit, spawnPoint);
                UnitController newUnitCont = newUnit.GetComponent<UnitController>();
                newUnitCont.unitBase = unitBase;
                newUnitCont.MoveToAction(selectedObj);
                newUnitCont.action = action;
                ResourceStorage._instance.UpdateResourceText();
            } else {
                ErrorController._instance.SetErrorText("Cannot send units out during the day");
            }
        } else {
            ErrorController._instance.SetErrorText("No units available");
        }
    }

    public virtual void ReturnUnit(UnitController unit) {
        availableUnits++;
        availableUnits += unit.humanConvertCollected;
        ResourceStorage._instance.AddWood(unit.woodCollected);
        ResourceStorage._instance.AddHunger(unit.hungerCollected);
        ResourceStorage._instance.UpdateResourceText();
        Destroy(unit.gameObject);
    }

    public void SetObjText() {
        if (selectedObj != null) {
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
        }
    }
}
