using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonInfo : MonoBehaviour {
    public Text partialFeed;
    public Text fullFeed;
    public Text convert;
    public Text collect;

    public void setTownActionInfo(HumanTownController town) {
        partialFeed.text = "Restores " + town.partialFeedAmt + " hunger";
    }

    public void setResourceActionInfo(ResourceController resource) {
        collect.text = "Collect " + resource.resourceCollectionAmt + " " + resource.gameObject.tag.ToLower();
    }
}
