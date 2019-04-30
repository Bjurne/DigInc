using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinerSlot : MonoBehaviour
{
    public ResourceManager resourceManager;
    public bool isOccupied;

    private void Start()
    {
        resourceManager = FindObjectOfType<ResourceManager>();
    }

    public void HireMiner()
    {
        if (!isOccupied)
        {
            resourceManager.HireMiner(this.GetComponentInParent<MiningShaft>());
        }
    }

    public void MinerAdded()
    {
        isOccupied = true;
        GetComponentInChildren<PriceTagTrigger>().Deactivate();
    }
}
