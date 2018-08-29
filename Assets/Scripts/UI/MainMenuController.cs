using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuController : MonoBehaviour {

    public void StartGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
}