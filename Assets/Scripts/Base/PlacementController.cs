using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementController : MonoBehaviour {

    private int selectedBuilding;
    private GameObject currentBuilding;

    [SerializeField]
    private GameObject[] selectableBuildings;

    private bool isSelected = false;


    // Use this for initialization
    void Start () {
        selectedBuilding = 0;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 spawnPosition = new Vector3(Mathf.Round(mousePosition.x), Mathf.Round(mousePosition.y), Mathf.Round(mousePosition.z));

        if (Input.GetKeyDown("q") && isSelected == false)
        {
            currentBuilding = (GameObject)Instantiate(selectableBuildings[selectedBuilding], spawnPosition, Quaternion.identity);
            isSelected = true;
        }

        //if right button is pressed, destroy the building template on the screen
        if (Input.GetMouseButtonDown(1) && isSelected == true)
        {
            Destroy(currentBuilding);
            isSelected = false;
            selectedBuilding = 0; 
        }
    }

    void SetBuildingInArray(int orderNumber)
    {
        selectedBuilding = orderNumber;
    }
}
