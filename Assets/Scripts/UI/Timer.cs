using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Timer : MonoBehaviour {
    [Range (0,1)]
    public float currentTime, increaseGradient, decreaseGradient;
    public float secondsInFullDay, convertedTime;
    public string hours, minutes;
    public RectTransform sunAndMoon;
    public Image clockFace;

	void Start () {

    }
	
	void Update () {
        currentTime += (Time.deltaTime / secondsInFullDay);
        Debug.Log("Current time: " + currentTime);
        if (currentTime > 1) {
            currentTime = 0;
        }
        ConvertTime();
        RotateClockFace();
        ChangeClockColour();
    }

    public void ConvertTime() {
        hours = Mathf.Floor(currentTime * 24f).ToString("00");
        minutes = Mathf.Floor(currentTime * 1440f % 60).ToString("00");
        Debug.Log("24 hour time - " + hours + ":" + minutes);
    }

    public void RotateClockFace() {
        clockFace.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, currentTime * 720f);
        sunAndMoon.localRotation = Quaternion.Euler(0f, 0f, currentTime * 360f);                
    }

    public void ChangeClockColour() {
        increaseGradient = currentTime * 2f;
        decreaseGradient = 2f - increaseGradient;
        if (currentTime >= 0 && currentTime < 0.25) {       //midnight to 6am
            clockFace.color = new Color(0f, increaseGradient, 160f);
        }
        else if (currentTime >= 0.25 && currentTime < 0.5) {//6am to midday
            clockFace.color = new Color(0f, increaseGradient, 160f);
        }
        else if (currentTime >= 0.5 && currentTime < 0.75) {//midday to 6pm
            clockFace.color = new Color(0f, decreaseGradient, 160f);
        }
        else if (currentTime >= 0.75 && currentTime < 1) {//6pm to midnight
            clockFace.color = new Color(0f, decreaseGradient, 160f);
        }
        //Debug.Log("Increase: " + increaseGradient);
        //Debug.Log("Decrease: " + decreaseGradient);
    }
}
