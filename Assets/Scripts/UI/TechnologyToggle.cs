﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Audio;

public class TechnologyToggle : MonoBehaviour {

    public bool clicked;
    public RectTransform rect;
    public Vector3 scale;

    public AudioClip openTechTree;
    public AudioClip closeTechTree;

	// Use this for initialization
	void Start () {
        clicked = false;
        scale = new Vector3(1, 1, 1);
        rect = GameObject.Find("Canvas/TechnologyWindow").GetComponent<RectTransform>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Toggle() {
        if (SelectionController._instance.selectedObj != null) {
            SelectionController._instance.DeselectObj();
            SelectionController._instance.selectedObj = null;
            SelectionController._instance.repairActionsContainer.SetActive(false);
            SelectionController._instance.resourceActionBtn.gameObject.SetActive(false);
            SelectionController._instance.townActionsContainer.SetActive(false);
            SelectionController._instance.SetObjText();
            SelectionController._instance.SetObjPortrait();
        }

        clicked = !clicked;
        if (!clicked) {
            SelectionController._instance.isTechnologyOpen = false;

            SoundManager.instance.RandomizeSfx(closeTechTree);

            rect.localScale = new Vector3(0, 0, 0);
        }
        else {
            SelectionController._instance.isTechnologyOpen = true;

            SoundManager.instance.RandomizeSfx(openTechTree);

            rect.localScale = scale;
        }
    }
}
