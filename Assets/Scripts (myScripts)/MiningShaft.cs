using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiningShaft : MonoBehaviour
{
    private MinerSlot[] allMinerSlots;
    public GameObject minerPrefab;
    public GameObject minerSlotPrefab;
    private Miner[] allMinersInMiningShaft;
    //private GameSystem gameSystem;
    public Button expandMiningShaftButton;

    public Text numberOfMinersInShaft;
    public Text maxNumberOfMinersInShaft;
    private RectTransform shaftFrameImage;
    public Text expectedExpenditure;

    private ResourceManager resourceManager;

    public int expansionLevel;

    public void Awake()
    {
        allMinerSlots = GetComponentsInChildren<MinerSlot>();
    }

    private void Start()
    {
        //gameSystem = FindObjectOfType<GameSystem>();
        resourceManager = FindObjectOfType<ResourceManager>();
        shaftFrameImage = transform.Find("ShaftFrame/FrameImage").GetComponent<RectTransform>();


        AddWorker();
    }

    private void FillAllMinerSlots()
    {
        foreach (MinerSlot minerSlot in allMinerSlots)
        {
            if (minerSlot.isOccupied) continue;
            else
            {
                GameObject newMiner = Instantiate(minerPrefab, minerSlot.transform.position, Quaternion.identity);
                newMiner.transform.SetParent(minerSlot.transform);
                minerSlot.isOccupied = true;
            }
        }
    }

    public void AddWorker()
    {
        foreach (MinerSlot minerSlot in allMinerSlots)
        {
            if (minerSlot.isOccupied) continue;
            else
            {
                GameObject newMiner = Instantiate(minerPrefab, minerSlot.transform.position, Quaternion.identity);
                newMiner.transform.SetParent(minerSlot.transform, false);
                newMiner.GetComponent<RectTransform>().localPosition = Vector3.zero;
                minerSlot.MinerAdded();
                break;
            }
        }
        SetNumberOfMinersInMiningShaft();
    }

    public void SetNumberOfMinersInMiningShaft()
    {
        allMinersInMiningShaft = GetComponentsInChildren<Miner>();
        numberOfMinersInShaft.text = allMinersInMiningShaft.Length.ToString();

        allMinerSlots = GetComponentsInChildren<MinerSlot>();
        maxNumberOfMinersInShaft.text = allMinerSlots.Length.ToString();
    }

    public void BuyMineExpansion()
    {
        resourceManager.BuyMineExpansion(this.GetComponent<MiningShaft>());
    }

    public void ExpandMiningShaft(int numberOfMinerSlots)
    {
        for (int i = 0; i < numberOfMinerSlots; i++)
        {
            GameObject newMinerSlot = Instantiate(minerSlotPrefab, this.transform.position, Quaternion.identity);
            newMinerSlot.transform.SetParent(this.transform, false);
        }

        allMinerSlots = GetComponentsInChildren<MinerSlot>();

        if (allMinerSlots.Length >= 14)
        {
            expandMiningShaftButton.interactable = false;
        }

        int newNumberOfMinerSlots = allMinerSlots.Length;
        maxNumberOfMinersInShaft.text = newNumberOfMinerSlots.ToString();

        float newShaftFrameWidth = Mathf.Clamp( newNumberOfMinerSlots * 46f, 400f, 630f);
        shaftFrameImage.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newShaftFrameWidth);
        iTween.PunchScale(shaftFrameImage.gameObject, Vector3.one * 0.1f, 1f);
    }

    public IEnumerator SendAllMinersToWork()
    {
        allMinersInMiningShaft = GetComponentsInChildren<Miner>();

        foreach (Button button in GetComponentsInChildren<Button>(true))
        {
            button.interactable = false;
        }

        yield return new WaitForSeconds(0.1f);

        foreach (Miner miner in allMinersInMiningShaft)
        {
            if (miner.minerStatus == Miner.Status.Active)
            {
                miner.isCurrentlyDigging = true;
                StartCoroutine(miner.MiningAnimation());
            }
        }

        yield return new WaitForSeconds(0.25f);

        foreach (Miner miner in allMinersInMiningShaft)
        {
            if (miner.minerStatus == Miner.Status.Active)
            {
                miner.Dig();
                yield return new WaitForSeconds(1f);
            }
        }
        //yield return new WaitForSeconds(0.5f);

        foreach (Miner miner in allMinersInMiningShaft)
        {
            miner.UpdateStatus();
        }
        GameSystem.instance.nextWeekButton.interactable = true;
        GameSystem.instance.sellDiamondsButton.interactable = true;
        GameSystem.instance.currentWeek++;
        resourceManager.GetSomeGoldImage.SetActive(true);
        GameSystem.instance.CheckIfGoalWasReached();
        UpdateExpectedExpenditure(resourceManager.CalculateAllMinersSalary());
        resourceManager.UpdateResourcesDisplayed();

        foreach (Button button in GetComponentsInChildren<Button>(true))
        {
            if (button == expandMiningShaftButton)
            {
                if (expansionLevel < 3) button.interactable = true;
            }
            else button.interactable = true;
        }
        //TODO fix find buttons properly
        yield return null;
    }

    public void EmptyMinerSlotPressed(MinerSlot minerSlot)
    {
        if (!minerSlot.isOccupied) AddWorker();
    }

    public void UpdateExpectedExpenditure(int allMinersSalary)
    {
        expectedExpenditure.text = allMinersSalary.ToString();
    }
}
