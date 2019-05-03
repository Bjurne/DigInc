using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public int numberOfDiamondsOwned;
    public int currentGold;

    public Text diamondQuantity;
    public Text GoldQuantity;
    public Text currentWeek;
    public Text deliveryDiamondQuantity;
    public Text deliveryWeeksLeft;

    public MiningShaft miningShaft;
    public GameObject sellingPrompt;
    //public GameSystem gameSystem;
    public Button sellButton;
    public GameObject strikeWarningPrompt;
    public GameObject GetSomeGoldImage;
    public GameObject DevelopThenGoImage;

    public int hireMinerCost = 3;
    public int shaftExpansionCost = 6;
    public int toolLevel1Cost = 6;
    public int toolLevel2Cost = 18;
    public int rankLevel1Cost = 6;
    public int rankLevel2Cost = 18;
    public int mineExpansion1Cost = 6;
    public int mineExpansion2Cost = 12;
    public int mineExpansion3Cost = 18;

    int sellingPrice;
    int allMinersTotalSalary = 0;
    public bool sellToAdvanceWeek;
    public bool allreadySoldThisTurn;


    private void Start()
    {
        AddGold(5);
        UpdateResourcesDisplayed();
    }

    public void UpdateResourcesDisplayed()
    {
        GoldQuantity.text = currentGold.ToString();
        diamondQuantity.text = numberOfDiamondsOwned.ToString();
        currentWeek.text = GameSystem.instance.currentWeek.ToString();
        miningShaft.UpdateExpectedExpenditure(CalculateAllMinersSalary());
        //deliveryDiamondQuantity.text = "Deliver " + gameSystem.goalDiamondQuantity.ToString() + " Diamonds";
        deliveryDiamondQuantity.text = GameSystem.instance.goalDiamondQuantity.ToString();

        int weeksLeft = GameSystem.instance.goalWeek - GameSystem.instance.currentWeek;
        if (weeksLeft <= 1)
        {
            foreach (Text childText in deliveryWeeksLeft.gameObject.GetComponentsInChildren<Text>())
            {
                if (childText != deliveryWeeksLeft) childText.gameObject.SetActive(false);
            }
            deliveryWeeksLeft.text = "after this week!";
        }
        else
        {
            foreach (Text childText in deliveryWeeksLeft.gameObject.GetComponentsInChildren<Text>(true))
            {
                childText.gameObject.SetActive(true);
            }
            deliveryWeeksLeft.text = weeksLeft.ToString();
        }
    }
    
    public bool CheckAffordability(int amount)
    {
        if (Mathf.Abs(amount) > currentGold)
        {
            Debug.Log("You can't afford that!");
            return false;
        }
        else
        {
            DrawGold(amount);
            return true;
        }
    }

    public void AddGold(int amount)
    {
        currentGold += Mathf.Abs(amount);
        UpdateResourcesDisplayed();
        iTween.PunchScale (GoldQuantity.transform.parent.gameObject, Vector3.one * 2f, 1f);
        AudioManager.INSTANCE.Play(AudioManager.INSTANCE.GoldSound);
    }

    public void DrawGold(int amount)
    {
        currentGold -= Mathf.Abs(amount);
        UpdateResourcesDisplayed();
    }

    public void AddDiamonds(int amount)
    {
        numberOfDiamondsOwned += Mathf.Abs(amount);
        UpdateResourcesDisplayed();
        if (amount > 0)
        {
            iTween.PunchScale(diamondQuantity.transform.parent.gameObject, Vector3.one * 2f, 0.25f);
            AudioManager.INSTANCE.Play(AudioManager.INSTANCE.digResultSound);
        }
    }

    public void DrawDiamonds(int amount)
    {
        numberOfDiamondsOwned -= Mathf.Abs(amount);
        UpdateResourcesDisplayed();
    }

    public void HireMiner(MiningShaft thisMiningShaft)
    {
        if (CheckAffordability(hireMinerCost))
        {
            thisMiningShaft.AddWorker();
            thisMiningShaft.UpdateExpectedExpenditure(CalculateAllMinersSalary());
        }
    }

    public void BuyMineExpansion(MiningShaft thisMiningshaft)
    {
        Button thisButton = thisMiningshaft.expandMiningShaftButton;

        if (thisMiningshaft.expansionLevel == 0)
        {
            if (CheckAffordability(mineExpansion1Cost))
            {
                thisMiningshaft.ExpandMiningShaft(2);
                thisMiningshaft.expansionLevel++;
                thisButton.GetComponentInChildren<PriceTagWidget>(true).SetNewPrice(mineExpansion2Cost);
            }
        }
        else if (thisMiningshaft.expansionLevel == 1)
        {
            if (CheckAffordability(mineExpansion2Cost))
            {
                thisMiningshaft.ExpandMiningShaft(2);
                thisMiningshaft.expansionLevel++;
                thisButton.GetComponentInChildren<PriceTagWidget>(true).SetNewPrice(mineExpansion3Cost);
            }
        }
        else if (thisMiningshaft.expansionLevel == 2)
        {
            if (CheckAffordability(mineExpansion3Cost))
            {
                thisMiningshaft.ExpandMiningShaft(2);
                thisMiningshaft.expansionLevel++;
                thisButton.GetComponentInChildren<PriceTagWidget>(true).SetNewPrice(0);
                thisButton.interactable = false;
            }
        }

    }

    public void ActivateSellingPrompt()
    {
        sellingPrompt.SetActive(true);
    }

    public void DeactivateSellingPrompt()
    {
        strikeWarningPrompt.SetActive(false);
        sellingPrompt.SetActive(false);
    }

    public void SellDiamonds(int numberOfDiamondsToSell)
    {
        sellingPrice = 0;
        if (numberOfDiamondsToSell == 1) sellingPrice = 10;
        if (numberOfDiamondsToSell == 2) sellingPrice = 18;
        if (numberOfDiamondsToSell == 3) sellingPrice = 24;
        if (numberOfDiamondsToSell == 4) sellingPrice = 30;
        if (numberOfDiamondsToSell == 5) sellingPrice = 35;

        if (numberOfDiamondsOwned >= numberOfDiamondsToSell)
        {
            AddGold(sellingPrice);
            DrawDiamonds(numberOfDiamondsToSell);
            sellButton.interactable = false;
            GetSomeGoldImage.SetActive(false);
            DevelopThenGoImage.SetActive(true);
            allreadySoldThisTurn = true;
            if (sellToAdvanceWeek)
            {
                sellToAdvanceWeek = false;
                GameSystem.instance.AdvanceWeek();
            }
        }
    }

    public int CalculateAllMinersSalary()
    {
        Miner[] allMiners = FindObjectsOfType<Miner>();
        int thisMinersSalary = 0;
        allMinersTotalSalary = 0;

        foreach (Miner miner in allMiners)
        {
            if (miner.minerStatus == Miner.Status.Active)
            {
                thisMinersSalary = miner.rankLevel + 1;
                allMinersTotalSalary += thisMinersSalary;
            }
        }

        return allMinersTotalSalary;
    }

    public void PayAllMinersSalary()
    {
        DrawGold(allMinersTotalSalary);

        if (currentGold < 0)
        {
            int missingGold = Mathf.Abs(currentGold);

            Miner[] allMiners = FindObjectsOfType<Miner>();

            foreach (Miner miner in allMiners)
            {
                if (miner.minerStatus == Miner.Status.Active)
                {
                    if (missingGold > 0)
                    {
                        missingGold -= 2;
                        miner.SendToSrike(1);
                    }
                }
            }
        }
    }

    public void CheckSalaryAffordability()
    {
        if (CalculateAllMinersSalary() <= currentGold) GameSystem.instance.AdvanceWeek();
        else if (!allreadySoldThisTurn)
        {
            sellToAdvanceWeek = true;
            ActivateSellingPrompt();
        }
        else GameSystem.instance.AdvanceWeek();
    }

    public void CheckStrikeWarningPrompt()
    {
        if (sellToAdvanceWeek && numberOfDiamondsOwned > 0)
        {
            strikeWarningPrompt.SetActive(true);
        }
        else CloseSellPrompt();
    }

    public void CloseSellPrompt()
    {
        strikeWarningPrompt.SetActive(false);
        sellingPrompt.SetActive(false);
        if (sellToAdvanceWeek)
        {
            sellToAdvanceWeek = false;
            GameSystem.instance.AdvanceWeek();
        }
    }
}
