using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThirstBar : MonoBehaviour {

    private float maxThirst;
    private float currentThirst;

    public Slider thirstBar;

	// Use this for initialization
	void Start () {
        maxThirst = 100f;
        currentThirst = maxThirst;

        thirstBar.value = thirstPercentage();
	}
	
	// Update is called once per frame
	void Update () {
        drainThirst(0.02f);
	}

    void drainThirst(float drainAmount)
    {
        currentThirst -= drainAmount;
        thirstBar.value = thirstPercentage();
    }

    void replenishThirst(float replenishAmount)
    {
        currentThirst += replenishAmount;
        thirstBar.value = thirstPercentage();
    }

    float thirstPercentage()
    {
        return currentThirst / maxThirst;
    }
}
