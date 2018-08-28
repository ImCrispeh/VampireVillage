using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneController : MonoBehaviour {
    public static SceneController Instance { get; set; }
    public GameObject pauseMenu;
    public bool togglePause;

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
        if (SceneManager.GetActiveScene().buildIndex == 1 && Input.GetKeyDown(KeyCode.Escape)) {
            if (!togglePause) {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);                
                togglePause = !togglePause;
            }
            else {
                Time.timeScale = 1;
                pauseMenu.SetActive(false);                
                togglePause = !togglePause;
            }
        }
	}

    public void StartGame() {
        SceneManager.LoadSceneAsync(1);
    }

    public void ResumeGame() {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        togglePause = !togglePause;
    }

    public void QuitToMainMenu() {
        SceneManager.LoadSceneAsync(0);
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        togglePause = !togglePause;
    }

    public void QuitGame() {
        Application.Quit();
    }
}
