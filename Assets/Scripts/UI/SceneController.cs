﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Audio;

public class SceneController : MonoBehaviour {
    public static SceneController Instance { get; set; }
    public GameObject pauseMenu;
    public bool togglePause;
    public bool togglePseudoPause;

    public AudioMixer mixer;
    public AudioMixerSnapshot[] snapshots;
    public float[] weights;

    public bool isGameOver;
    public GameObject gameOverScreen;
    public Text gameOverText;

    void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start () {
        //creates a singleton, much like a static reference, there can only be one of this at any time
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        togglePause = false;
	}
	
	void Update () {
        //checks if we're in the game scene then pauses the game by flipping the bool
        if (SceneManager.GetActiveScene().buildIndex == 1 && !isGameOver) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                if (!togglePause) {
                    Timer._instance.PauseTimer();
                    CameraController._instance.PauseCamera();
                    pauseMenu.SetActive(true);
                    togglePause = !togglePause;
                } else {
                    Timer._instance.UnpauseTimer();
                    CameraController._instance.UnpauseCamera();
                    pauseMenu.SetActive(false);
                    togglePause = !togglePause;
                }
            }

            if (Input.GetKeyDown(KeyCode.P)) {
                if (!togglePseudoPause) {
                    Timer._instance.PauseTimer();
                } else {
                    Timer._instance.UnpauseTimer();
                }
                togglePseudoPause = !togglePseudoPause;
            }
        }

        if(togglePause){
            weights[0] = 0.0f;
            weights[1] = 0.0f;
            weights[2] = 1.0f;
            mixer.TransitionToSnapshots(snapshots, weights, 0.0f);
        }else{
            weights[0] = 0.5f;
            weights[1] = 0.5f;
            weights[2] = 0.0f;
            mixer.TransitionToSnapshots(snapshots, weights, 0.0f);
        }
	}

    public void StartGame() {
        SceneManager.LoadSceneAsync(1);
    }

    public void ResumeGame() {
        Timer._instance.UnpauseTimer();
        CameraController._instance.UnpauseCamera();
        pauseMenu.SetActive(false);
        pauseMenu.transform.GetChild(0).gameObject.SetActive(true);
        gameOverScreen.SetActive(false);
        togglePause = !togglePause;
    
    }

    public void QuitToMainMenu() {
        SceneManager.LoadSceneAsync(0);
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        pauseMenu.transform.GetChild(0).gameObject.SetActive(true);
        gameOverScreen.SetActive(false);
        togglePause = !togglePause;
        isGameOver = false;
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void RestartGame() {
        Timer._instance.UnpauseTimer();
        isGameOver = false;
        pauseMenu.SetActive(false);
        pauseMenu.transform.GetChild(0).gameObject.SetActive(true);
        gameOverScreen.SetActive(false);
        SceneManager.LoadScene(1);
    }

    public void EndGame(bool isWin, string message) {
        isGameOver = true;
        Timer._instance.PauseTimer();
        pauseMenu.SetActive(true);
        pauseMenu.transform.GetChild(0).gameObject.SetActive(false);
        if (isWin) {
            gameOverText.text =
                "You Win" + "\n"
                + message;
        } else {
            gameOverText.text =
                "You Lose" + "\n"
                + message;
        }
        gameOverScreen.SetActive(true);
    }
}
