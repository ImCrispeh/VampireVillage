using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupController : MonoBehaviour {
    public static PopupController _instance;

    public GameObject popupPanel;
    public Text popupText;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    public void SetPopupText(string popup) {
        popupText.text = popup;
        StartCoroutine("ShowPopup");
    }

    IEnumerator ShowPopup() {
        popupPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        popupPanel.SetActive(false);
    }
}
