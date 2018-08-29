using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ResourceBar : MonoBehaviour {
    public ResourceStorage resources;
    public Text woodT, stoneT, goldT;

	void Start () {
        resources = ResourceStorage._instance;
        woodT = GameObject.Find("ResourceBar/Wood").GetComponent<Text>();
        stoneT = GameObject.Find("ResourceBar/Stone").GetComponent<Text>();
        goldT = GameObject.Find("ResourceBar/Gold").GetComponent<Text>();
    }
	
	void Update () {
        UpdateResources();
	}

    public void UpdateResources() {
        woodT.text = resources.wood.ToString();
        stoneT.text = resources.stone.ToString();
        goldT.text = resources.gold.ToString();
    }
}
