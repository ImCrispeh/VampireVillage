using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsController : MonoBehaviour {
    public GameObject instructions;
	
    public void ToggleInstructions() {
        instructions.SetActive(!instructions.activeInHierarchy);
    }
}
