using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Timer : MonoBehaviour {
    [Range (0,1)]
    public float currentTime;
    public float secondsInFullDay, convertedTime;
    public string hours, minutes;

	void Start () {
        currentTime = 0f;
        secondsInFullDay = 120f;
        convertedTime = 0f;
	}
	
	void Update () {
        currentTime += (Time.deltaTime / secondsInFullDay);
        Debug.Log("Current time: " + currentTime);
        if (currentTime > 1) {
            currentTime = 0;
        }
        ConvertTime();
        /*if (currentTime >= 0 && currentTime < 0.25) {
            Debug.Log("Midnight from 0 to 0.25");
        }
        else if (currentTime > 0.25 && currentTime < 0.5) {
            Debug.Log("Sunrise from 0.25 to 0.5");
        }
        else if (currentTime > 0.5 && currentTime < 0.75) {
            Debug.Log("Midday from 0.5 to 0.75");
        }
        else if (currentTime > 0.75 && currentTime < 1) {
            Debug.Log("Sunset from 0.75 to 1");
        }*/
    }

    public void ConvertTime() {
        hours = Mathf.Floor(currentTime * 24f).ToString("00");
        minutes = Mathf.Floor(currentTime * 1440f % 60).ToString("00");
        Debug.Log(hours + ":" + minutes);
    }
}
