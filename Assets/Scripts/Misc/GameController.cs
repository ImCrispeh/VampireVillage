using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public static GameController _instance;

    public GameObject gameOverScreen;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    void Start () {
		
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
	}

    public void RestartGame() {
        Timer._instance.UnpauseTimer();
        SceneManager.LoadScene(0);
    }

    public void EndGame() {
        Timer._instance.PauseTimer();
        gameOverScreen.SetActive(true);
    }
}
