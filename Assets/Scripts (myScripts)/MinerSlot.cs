using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinerSlot : MonoBehaviour
{
    public ResourceManager resourceManager;
    public bool isOccupied;

    private Image thisSlotSprite;
    public List<Sprite> minerSlotSprites;

    private void Start()
    {
        thisSlotSprite = GetComponent<Image>();
        resourceManager = FindObjectOfType<ResourceManager>();

        int randomSprite = UnityEngine.Random.Range(0, 3);

        thisSlotSprite.sprite = minerSlotSprites[randomSprite];
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
