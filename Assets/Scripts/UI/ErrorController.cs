using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorController : MonoBehaviour {
    public static ErrorController _instance;

    public GameObject errorPanel;
    public Text errorText;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    public void SetErrorText(string error) {
        errorText.text = error;
        StartCoroutine("ShowError");
    }

    IEnumerator ShowError() {
        errorPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        errorPanel.SetActive(false);
    }
}
