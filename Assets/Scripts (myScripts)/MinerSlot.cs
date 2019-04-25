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
            resourceManager.HireMiner();
        }
    }

    public void MinerAdded()
    {
        isOccupied = true;
        //GetComponentInChildren<Text>().gameObject.SetActive(false);
        //"Hire Miner" text in MinerSlot is deactivated by default now, we have PriceTags for this
        GetComponentInChildren<PriceTagTrigger>().Deactivate();
    }
}
