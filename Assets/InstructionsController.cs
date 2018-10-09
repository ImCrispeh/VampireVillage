using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsController : MonoBehaviour {
    public GameObject instructions;
	
    public void ToggleInstructions() {
        if (SelectionController._instance.selectedObj != null) {
            SelectionController._instance.DeselectObj();
            SelectionController._instance.selectedObj = null;
            SelectionController._instance.repairActionsContainer.SetActive(false);
            SelectionController._instance.resourceActionBtn.gameObject.SetActive(false);
            SelectionController._instance.townActionsContainer.SetActive(false);
            SelectionController._instance.SetObjText();
            SelectionController._instance.SetObjPortrait();
        }

        instructions.SetActive(!instructions.activeInHierarchy);
        if (instructions.activeInHierarchy) {
            SceneController.Instance.togglePseudoPause = true;
            Timer._instance.PauseTimer();
        }
        else {
            SceneController.Instance.togglePseudoPause = false;
            Timer._instance.UnpauseTimer();
        }
    }
}
