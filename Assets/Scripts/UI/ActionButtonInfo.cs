using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonInfo : MonoBehaviour {
    public Text partialFeed;
    public Text fullFeed;
    public Text convert;
    public Text collect;
    public Text repair20;
    public Text repair50;
    public Text repairFull;

    public void setTownActionInfo(HumanTownController town) {
		partialFeed.text = "Kill " + town.populationPartialLoss + " to restore " + town.partialFeedAmt + " hunger " + "("
			+ town.partialFeedThreat + " Threat)";
		fullFeed.text = "Kill " + town.populationFullLoss + " to restore " + town.fullFeedAmt + " hunger " + "("
			+ town.fullFeedThreat + " Threat)";
        convert.text = "Convert 1 human to a unit (" + town.convertThreat + " - " + (town.convertThreat + 10) + ")";
    }

    public void setResourceActionInfo(ResourceController resource) {
        collect.text = "Collect " + resource.resourceCollectionAmt + " " + resource.gameObject.tag.ToLower();
    }

    public void setRepairActionInfo() {
        int toRepair20 = BaseController._instance.CalculateRepair(20, false);
        int toRepair50 = BaseController._instance.CalculateRepair(50, false);
        int toRepairFull = BaseController._instance.CalculateRepair(BaseController._instance.maxHealth, false);
        repair20.text = "Repairs up to 20 health (uses: " + toRepair20 * 3 + " of your current wood/stone";
        repair50.text = "Repairs up to 50 health (uses: " + toRepair50 * 3 + " of your current wood/stone";
        repairFull.text = "Repairs up to max health (uses: " + toRepairFull * 3 + " of your current wood/stone";
    }
}
