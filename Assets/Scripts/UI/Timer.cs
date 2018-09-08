using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Timer : MonoBehaviour {
    public static Timer _instance;

    [Range (0,1)]
    public float currentTime, increaseGradient, decreaseGradient;
    public float secondsInFullDay, convertedTime;
    public bool isPaused;
    public string hours, minutes;
    public int currentDay;
    private float currentDayTimer;
    public Text speedText;
    private RectTransform sunAndMoon;
    private Image clockFace;
    private Text timeT;
    private List<HumanTownController> humanTowns;

    void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }

        clockFace = GameObject.Find("Clock/Frame").transform.GetChild(0).GetComponent<Image>();
        sunAndMoon = GameObject.Find("Clock/Frame").transform.GetChild(1).GetComponent<RectTransform>();
        timeT = GameObject.Find("Clock/TimeIndicator").transform.GetChild(0).GetComponent<Text>();
    }

    void Start () {
        timeT.text = "";

        humanTowns = new List<HumanTownController>();
        foreach (HumanTownController town in FindObjectsOfType<HumanTownController>()) {
            humanTowns.Add(town);
        }
    }
	
	void Update () {
        currentTime += (Time.deltaTime / secondsInFullDay);
        //Debug.Log("Current time: " + currentTime);
        if (currentTime > 1) {
            currentTime = 0;
        }

        currentDayTimer += Time.deltaTime;
        if (currentDayTimer >= secondsInFullDay) {
            currentDay++;
            currentDayTimer -= secondsInFullDay;
            if (currentDay % 5 == 0) {
                EnemySpawner._instance.IncreaseDifficulty();
            }

            foreach (HumanTownController town in humanTowns) {
                town.RegenPopulation();
            }
        }

        ConvertTime();
        RotateClockFace();
        ChangeClockColour();
    }

    public void ConvertTime() {
        hours = Mathf.Floor(currentTime * 24f).ToString("00");
        minutes = Mathf.Floor(currentTime * 1440f % 60).ToString("00");
        timeT.text = "Day " + (currentDay+1) + " | " + hours + ":" + minutes;
        //Debug.Log("24 hour time - " + hours + ":" + minutes);
    }

    public void RotateClockFace() {
        clockFace.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, currentTime * 720f);
        sunAndMoon.localRotation = Quaternion.Euler(0f, 0f, currentTime * 360f);

        foreach (Transform child in transform)
        {
            if (secondsInFullDay != 0)
            {
                child.transform.RotateAround(Vector3.zero, new Vector3(0, 0, 1), Mathf.PI * Time.deltaTime * 1 / (secondsInFullDay / 120));
                child.transform.LookAt(Vector3.zero);
            }
        }
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
    }

    public void PauseTimer() {
        Time.timeScale = 0;
        isPaused = true;
    }

    public void UnpauseTimer() {
        Time.timeScale = 1;
        isPaused = false;
    }

    public void ToggleSpeed() {
        if (Time.timeScale == 1) {
            Time.timeScale = 2;
        } else if (Time.timeScale == 2) {
            Time.timeScale = 3;
        } else {
            Time.timeScale = 1;
        }

        speedText.text = "x" + Time.timeScale;
    }
}
