using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BaseHealthBar : MonoBehaviour {
    public BaseController _base;
    public Canvas miniCanvas;
    public Camera mainCamera;
    public Image healthBar;

	void Start () {
        _base = this.GetComponent<BaseController>();
        miniCanvas = GameObject.Find("World/Vampire Base/MiniCanvas").GetComponent<Canvas>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        healthBar = GameObject.Find("World/Vampire Base/MiniCanvas/HealthBar").GetComponent<Image>();
    }
	
	void Update () {
        UpdateHealthBar();
        //miniCanvas.transform.LookAt(mainCamera.transform);
        //miniCanvas.transform.forward = mainCamera.transform.forward;
	}

    private void LateUpdate() {
        miniCanvas.transform.forward = mainCamera.transform.forward;
    }

    //sets the image fill by dividing the current health by the maximum health for a value between 0 and 1
    public void UpdateHealthBar() {
        float currentHealth = _base.health;
        currentHealth = currentHealth / _base.maxHealth;
        healthBar.fillAmount = currentHealth;
    }
}
