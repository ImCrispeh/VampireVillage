using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionTooltip : MonoBehaviour {
    public GameObject tooltip;

    public void ShowTooltip() {
        tooltip.SetActive(true);
    }

    public void HideTooltip() {
        tooltip.SetActive(false);
    }
}
