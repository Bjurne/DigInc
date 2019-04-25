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
    public GameSystem gameSystem;
    public Button expandMiningShaftButton;

    public Text numberOfMinersInShaft;
    public Text maxNumberOfMinersInShaft;
    public Text expectedExpenditure;

    public ResourceManager resourceManager;

    public void Awake()
    {
        allMinerSlots = GetComponentsInChildren<MinerSlot>();

        AddWorker();
        //FillAllMinerSlots();
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
                newMiner.transform.SetParent(minerSlot.transform);
                minerSlot.MinerAdded();
                break;
            }
        }
        SetNumberOfMinersInMiningShaft();
    }

    private void SetNumberOfMinersInMiningShaft()
    {
        allMinersInMiningShaft = GetComponentsInChildren<Miner>();
        numberOfMinersInShaft.text = allMinersInMiningShaft.Length.ToString();
    }

    public void ExpandMiningShaft(int numberOfMinerSlots)
    {
        for (int i = 0; i < numberOfMinerSlots; i++)
        {
            GameObject newMinerSlot = Instantiate(minerSlotPrefab, this.transform.position, Quaternion.identity);
            //newMinerSlot.transform.localScale = Vector3.one;
            //newMinerSlot.transform.lossyScale.Set(1f, 1f, 1f);
            newMinerSlot.transform.SetParent(this.transform);
        }

        allMinerSlots = GetComponentsInChildren<MinerSlot>();

        if (allMinerSlots.Length >= 14)
        {
            expandMiningShaftButton.interactable = false;
        }

        maxNumberOfMinersInShaft.text = allMinerSlots.Length.ToString();
    }

    public IEnumerator SendAllMinersToWork()
    {
        allMinersInMiningShaft = GetComponentsInChildren<Miner>();

        foreach (Button button in GetComponentsInChildren<Button>(true))
        {
            button.interactable = false;
        }

        foreach (Miner miner in allMinersInMiningShaft)
        {
            if (miner.minerStatus == Miner.Status.Active)
            {
                miner.isCurrentlyDigging = true;
                StartCoroutine(miner.MiningAnimation());
            }
        }

        foreach (Miner miner in allMinersInMiningShaft)
        {
            if (miner.minerStatus == Miner.Status.Active)
            {
                miner.Dig();
                yield return new WaitForSeconds(2f);
            }
        }
        yield return new WaitForSeconds(1f);

        foreach (Miner miner in allMinersInMiningShaft)
        {
            miner.UpdateStatus();
        }
        gameSystem.nextWeekButton.interactable = true;
        gameSystem.sellDiamondsButton.interactable = true;
        gameSystem.CheckIfGoalWasReached();
        UpdateExpectedExpenditure(resourceManager.CalculateAllMinersSalary());

        foreach (Button button in GetComponentsInChildren<Button>(true))
        {
            button.interactable = true;
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
