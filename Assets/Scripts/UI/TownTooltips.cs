using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownTooltips : MonoBehaviour {
    public Text partialFeed;
    public Text fullFeed;
    public Text convert;

    public void setTooltips(HumanTownController town) {
        partialFeed.text = "Restores " + town.partialFeedAmt + " hunger";
    }

}
