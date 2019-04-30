using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Miner : MonoBehaviour
{
    private MiningShaft miningShaft;
    private ResourceManager resourceManager;
    public GameObject digResultPopUpPrefab;
    private EventBar eventBar;
    private GameSystem gameSystem;

    public int toolLevel;
    public int rankLevel;

    private int thisWeeksResult;
    private int thisDiggingResult;

    public enum Status { Active, Studying, Hurt, Striking }
    public Status minerStatus;
    public bool isCurrentlyDigging;
    public int numberOfTurnsStudying;
    public int numberOfTurnsStriking;
    public int numberOfTurnsHurt;

    public GameObject StudyIndicator;
    public GameObject StrikeIndicator;
    public GameObject HurtIndicator;

    public Image toolImage;
    public Image rankImage;
    public Image statusImage;

    public Image minerSprite;

    public Sprite rank1Sprite;
    public Sprite rank2Sprite;
    public Sprite tool1Sprite;
    public Sprite tool2Sprite;
    public Sprite eventSprite;

    public void Start()
    {
        resourceManager = FindObjectOfType<ResourceManager>();
        eventBar = FindObjectOfType<EventBar>();
        minerStatus = Status.Active;
        StudyIndicator.gameObject.SetActive(false);
        StrikeIndicator.gameObject.SetActive(false);
        miningShaft = GetComponentInParent<MiningShaft>();
        gameSystem = FindObjectOfType<GameSystem>();
    }

    public void Dig()
    {
        thisWeeksResult = 0;
        thisDiggingResult = 0;

        for (int i = 0; i <= rankLevel; i++)
        {
            if (toolLevel == 2) thisDiggingResult = ToolLevel2Dig();
            else if (toolLevel == 1) thisDiggingResult = ToolLevel1Dig();
            else thisDiggingResult = ToolLevel0Dig();

            if (thisDiggingResult > thisWeeksResult) thisWeeksResult = thisDiggingResult;
            
            if (gameSystem.firstDigGuarantee)
            {
                thisWeeksResult = 2;
                gameSystem.firstDigGuarantee = false;
            }
        }

        isCurrentlyDigging = false;
        resourceManager.AddDiamonds(thisWeeksResult);
        if (thisWeeksResult == 0) DigEvent();
        CreateDigResultPopUp(thisWeeksResult);
    }

    public void UpdateStatus()
    {
        if (numberOfTurnsStriking > 0)
        {
            numberOfTurnsStriking--;
            StrikeIndicator.gameObject.SetActive(true);
            StrikeIndicator.GetComponentInChildren<Text>().text = ("Striking " + numberOfTurnsStriking);
            StudyIndicator.gameObject.SetActive(false);
            HurtIndicator.gameObject.SetActive(false);
        }
        if (numberOfTurnsStudying > 0)
        {
            numberOfTurnsStudying--;
            StudyIndicator.gameObject.SetActive(true);
            StudyIndicator.GetComponentInChildren<Text>().text = ("Studying " + numberOfTurnsStudying);
            StrikeIndicator.gameObject.SetActive(false);
            HurtIndicator.gameObject.SetActive(false);
        }
        if (numberOfTurnsHurt > 0)
        {
            numberOfTurnsHurt--;
            HurtIndicator.gameObject.SetActive(true);
            HurtIndicator.GetComponentInChildren<Text>().text = ("Hurt " + numberOfTurnsHurt);
            StudyIndicator.gameObject.SetActive(false);
            StrikeIndicator.gameObject.SetActive(false);
        }
        if (numberOfTurnsStudying == 0 && numberOfTurnsStriking == 0 && numberOfTurnsHurt == 0)
        {
            minerStatus = Status.Active;
            StudyIndicator.gameObject.SetActive(false);
            StrikeIndicator.gameObject.SetActive(false);
            HurtIndicator.gameObject.SetActive(false);
        }

        miningShaft.UpdateExpectedExpenditure(resourceManager.CalculateAllMinersSalary());
    }

    public void LevelUpTool()
    {
        if (toolLevel == 0)
        {
            if (resourceManager.CheckAffordability(resourceManager.toolLevel1Cost))
            {
                toolLevel++;
                toolImage.sprite = tool1Sprite;
                PriceTagTrigger priceTagTrigger = toolImage.gameObject.GetComponentInChildren<PriceTagTrigger>();
                priceTagTrigger.UpdatePrice(resourceManager.toolLevel2Cost);
            }
        }
        else if (toolLevel == 1)
        {
            if (resourceManager.CheckAffordability(resourceManager.toolLevel2Cost))
            {
                toolLevel++;
                toolImage.sprite = tool2Sprite;
                PriceTagTrigger priceTagTrigger = toolImage.gameObject.GetComponentInChildren<PriceTagTrigger>();
                priceTagTrigger.UpdatePrice(0);
            }
        }
    }

    public void SendToStudy(int days)
    {
        minerStatus = Status.Studying;
        numberOfTurnsStudying += days;
        StudyIndicator.gameObject.SetActive(true);
        StudyIndicator.GetComponentInChildren<Text>().text = ("Studying " + numberOfTurnsStudying);
        miningShaft.UpdateExpectedExpenditure(resourceManager.CalculateAllMinersSalary());
    }

    public void SendToSrike(int days)
    {
        minerStatus = Status.Striking;
        numberOfTurnsStriking += days;
        StrikeIndicator.gameObject.SetActive(true);
        StrikeIndicator.GetComponentInChildren<Text>().text = ("Striking " + numberOfTurnsStriking);
        miningShaft.UpdateExpectedExpenditure(resourceManager.CalculateAllMinersSalary());
    }

    public void MinerGotHurt(int days)
    {
        minerStatus = Status.Hurt;
        numberOfTurnsHurt += days;
        HurtIndicator.gameObject.SetActive(true);
        HurtIndicator.GetComponentInChildren<Text>().text = ("Hurt " + numberOfTurnsHurt);
        miningShaft.UpdateExpectedExpenditure(resourceManager.CalculateAllMinersSalary());
    }

    public void LevelUpRank()
    {
        if (rankLevel == 0)
        {
            if (resourceManager.CheckAffordability(resourceManager.rankLevel1Cost))
            {
                rankLevel++;
                rankImage.sprite = rank1Sprite;
                minerSprite.sprite = rank1Sprite;
                SendToStudy(1);
                PriceTagTrigger priceTagTrigger = rankImage.gameObject.GetComponentInChildren<PriceTagTrigger>();
                priceTagTrigger.UpdatePrice(resourceManager.rankLevel2Cost);
            }
        }
        else if (rankLevel == 1)
        {
            if (resourceManager.CheckAffordability(resourceManager.rankLevel2Cost))
            {
                rankLevel++;
                rankImage.sprite = rank2Sprite;
                minerSprite.sprite = rank2Sprite;
                SendToStudy(2);
                PriceTagTrigger priceTagTrigger = rankImage.gameObject.GetComponentInChildren<PriceTagTrigger>();
                priceTagTrigger.UpdatePrice(0);
            }
        }
    }

    public void CreateDigResultPopUp(int amount)
    {
        GameObject newPopUp = Instantiate(digResultPopUpPrefab, transform.position, Quaternion.identity);
        newPopUp.transform.SetParent(transform);
        newPopUp.GetComponent<DigResultPopUp>().SetDigResult(amount);
    }

    public IEnumerator MiningAnimation()
    {
        Sprite originalSprite = minerSprite.sprite;
        minerSprite.sprite = toolImage.sprite;
        while (isCurrentlyDigging)
        {
            iTween.PunchRotation(minerSprite.gameObject, new Vector3(0f, 0f, 150f), 0.5f);
            yield return new WaitForSeconds(0.5f);
        }
        minerSprite.sprite = originalSprite;
        yield return null;
    }

    private int ToolLevel0Dig()
    {
        int level0Roll = UnityEngine.Random.Range(1,100);

        if (level0Roll <= 20) return 2;
        if (level0Roll <= 80) return 1;
        else return 0;
    }


    private int ToolLevel1Dig()
    {
        int level1Roll = UnityEngine.Random.Range(1, 100);

        if (level1Roll <= 20) return 3;
        if (level1Roll <= 70) return 2;
        if (level1Roll <= 90) return 1;
        else return 0;
    }
    private int ToolLevel2Dig()
    {
        int level2Roll = UnityEngine.Random.Range(1, 100);

        if (level2Roll <= 25) return 4;
        if (level2Roll <= 55) return 3;
        if (level2Roll <= 80) return 2;
        if (level2Roll <= 90) return 1;
        else return 0;
    }

    private void DigEvent()
    {
        int eventRoll = UnityEngine.Random.Range(1, 100);

        if (eventRoll <= 35 + (rankLevel * 10))
        {
            resourceManager.AddGold(UnityEngine.Random.Range(4, 14));
            iTween.PunchPosition(resourceManager.GoldQuantity.gameObject, UnityEngine.Random.insideUnitSphere * 15, 1f);
            eventBar.AddEvent(EventBar.EventType.FoundGold);
        }
        else if (eventRoll >= 85 + (rankLevel*10))
        {
            MinerGotHurt(UnityEngine.Random.Range(2, 3));
            eventBar.AddEvent(EventBar.EventType.MinerHurt);
        }
    }

    public void FireMiner()
    {
        Destroy(GetComponentInParent<MinerSlot>().gameObject);
        miningShaft.ExpandMiningShaft(1);
        Destroy(this.gameObject);
        miningShaft.SetNumberOfMinersInMiningShaft();
    }
}
