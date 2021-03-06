﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class OldFortifications : Technology, IPointerEnterHandler, IPointerExitHandler {

    public Image unresearchedImage;
    public GameObject technologyObject;
    public Transform technologyPosition;
    public Technology requiredTechnology;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        technologyName = "Fortifications";
        technologyDescription = "Fortifications are added to your stone walls, adding to your defenses";
        woodCost = 150; //This will need to be changed once we discuss resources
        researchTime = 30f; //20 secs - currently not linked to the timer
        researchTimer = researchTime;
        researched = false;
        researching = false;
        applyTechnology = false;
        technologyImage = unresearchedImage;
        mainBase = BaseController._instance;
    }
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
        if (researched && !researching && !applyTechnology) {
            EndResearch();
            applyTechnology = true;
        }
	}

    public override void TechnologyEffect() {
        //The effects of the technology which are active once research ends
        //something like Town.Defense += 25;
        mainBase.defense += 10;
        Debug.Log("Added " + technologyName + " to the town");
        Destroy(technologyPosition.GetChild(0).gameObject);
        Instantiate(technologyObject, technologyPosition);
    }

    public override void StartResearch() {
        if (!researched && !researching && requiredTechnology.researched) {
            if (ResourceStorage._instance.wood >= woodCost) {
                researchTimer = 0;
                researching = true;
                ResourceStorage._instance.SubtractWood(woodCost);
                ResourceStorage._instance.UpdateResourceText();
                Debug.Log("Researching: " + technologyName);
            } else {
                PopupController._instance.SetPopupText("Not enough resources available");
            }
        }
    }

    public override void EndResearch() {
        TechnologyEffect();
        Debug.Log("Finished researching: " + technologyName);
    }

    public override void OnPointerEnter(PointerEventData pointer) {
        Debug.Log("Mouse is over: " + technologyName);
    }

    public override void OnPointerExit(PointerEventData pointer) {
        Debug.Log("Mouse is not over: " + technologyName + " anymore");
    }
}
