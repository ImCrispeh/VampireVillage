using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BaseHealthBar : MonoBehaviour {
    public BaseController _base;
    public Image healthBar;

	void Start () {
        _base = this.GetComponent<BaseController>();
        healthBar = GameObject.Find("MiniCanvas/HealthBar").GetComponent<Image>();
    }
	
	void Update () {
        UpdateHealthBar();
	}

    public void UpdateHealthBar() {
        float currentHealth = _base.health;
        currentHealth = currentHealth / _base.maxHealth;
        healthBar.fillAmount = currentHealth;
    }
}
