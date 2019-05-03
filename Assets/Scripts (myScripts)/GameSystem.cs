using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameSystem : MonoBehaviour
{
    public int currentWeek = 1;
    //public MiningShaft miningShaft;
    public Button nextWeekButton;
    public Button sellDiamondsButton;
    public ResourceManager resourceManager;
    public GameObject debugManager;
    public GameObject winScreen;
    public GameObject looseScreen;
    public EventBar eventBar;

    public enum DifficultySetting {Easy, Normal, Hard};
    public DifficultySetting currentDifficultyLevel;

    public int goalWeek;
    public int goalDiamondQuantity;
    private int currentDeliveryNumber = 1;
    private int TotalNumberOfDeliveries;

    public bool firstDigGuarantee = true;

    public static GameSystem instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("destroyed gs");
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            debugManager.SetActive(!debugManager.activeInHierarchy);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            looseScreen.SetActive(!looseScreen.activeInHierarchy);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    public void AdvanceWeek()
    {
        nextWeekButton.interactable = false;
        sellDiamondsButton.interactable = false;
        resourceManager.DevelopThenGoImage.SetActive(false);
        resourceManager.GetSomeGoldImage.SetActive(false);
        resourceManager.PayAllMinersSalary();
        //currentWeek++;
        eventBar.ClearEventBar();

        foreach (MiningShaft miningShaft in FindObjectsOfType<MiningShaft>())
        {
            StartCoroutine(miningShaft.SendAllMinersToWork());
        }

        resourceManager.allreadySoldThisTurn = false;
    }

    public void CheckIfGoalWasReached()
    {
        if (currentWeek >= goalWeek)
        {
            if (resourceManager.numberOfDiamondsOwned >= goalDiamondQuantity)
            {
                Debug.Log("You've made it! New delivery request");
                TurnOtherCanvasGroupsOff(winScreen.GetComponent<CanvasGroup>());
                winScreen.SetActive(true);
                DeliveryRequestFulfilled();
            }
            else
            {
                Debug.Log("You didn't reach the goal! Game over!");
                TurnOtherCanvasGroupsOff(looseScreen.GetComponent<CanvasGroup>());
                looseScreen.SetActive(true);
            }
        }
    }

    private void DeliveryRequestFulfilled()
    {
        resourceManager.numberOfDiamondsOwned -= goalDiamondQuantity;
        resourceManager.AddGold(goalDiamondQuantity / 2);
        if (currentDeliveryNumber >= TotalNumberOfDeliveries)
        {
            RestartGame();
        }
    }
    
    public void SetNewDeliveryRequest()
    {
        if (currentDifficultyLevel == DifficultySetting.Easy)
        {
            goalWeek = Mathf.RoundToInt(goalWeek * 1.7f);
            goalDiamondQuantity *= 2;
            resourceManager.UpdateResourcesDisplayed();
        }
        else if(currentDifficultyLevel == DifficultySetting.Normal)
        {
            goalWeek = Mathf.RoundToInt(goalWeek * 1.6f);
            goalDiamondQuantity *= 3;
            resourceManager.UpdateResourcesDisplayed();
        }
        else if (currentDifficultyLevel == DifficultySetting.Hard)
        {
            goalWeek = Mathf.RoundToInt(goalWeek * 1.5f);
            goalDiamondQuantity *= 4;
            resourceManager.UpdateResourcesDisplayed();
        }
        currentDeliveryNumber++;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("PrototypeScene");
    }

    public void RestartGame()
    {
        Destroy(this.gameObject);
        SceneManager.LoadScene("PrototypeScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void TurnOtherCanvasGroupsOff(CanvasGroup activeCanvasGroup)
    {
        foreach (CanvasGroup cg in FindObjectsOfType<CanvasGroup>())
        {
            if (cg != activeCanvasGroup) cg.interactable = false;
        }
    }

    public void TurnAllCanvasGroupsOn()
    {
        foreach (CanvasGroup cg in FindObjectsOfType<CanvasGroup>())
        {
            cg.interactable = true;
        }
    }

    public void SetDifficultyLevel(int chosenSetting)
    {
        if (chosenSetting == 1) currentDifficultyLevel = DifficultySetting.Easy;
        if (chosenSetting == 2) currentDifficultyLevel = DifficultySetting.Normal;
        if (chosenSetting == 3) currentDifficultyLevel = DifficultySetting.Hard;

        if (currentDifficultyLevel == DifficultySetting.Easy)
        {
            goalWeek = 3;
            goalDiamondQuantity = 6;
            TotalNumberOfDeliveries = 3;
        }
        else if (currentDifficultyLevel == DifficultySetting.Normal)
        {
            goalWeek = 5;
            goalDiamondQuantity = 16;
            TotalNumberOfDeliveries = 5;
        }
        else if (currentDifficultyLevel == DifficultySetting.Hard)
        {
            goalWeek = 6;
            goalDiamondQuantity = 32;
            TotalNumberOfDeliveries = 8;
        }
        
        StartCoroutine(FixStartingGoalDisplayed());
    }

    private IEnumerator FixStartingGoalDisplayed()
    {
        yield return new WaitForSeconds(0.3f);
        resourceManager.UpdateResourcesDisplayed();
    }

    //private IEnumerator Setup()
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    nextWeekButton = transform.Find("/GameCanvas/GameButtonsPanel/NextWeekButton").GetComponent<Button>();
    //    sellDiamondsButton = transform.Find("/GameCanvas/GameButtonsPanel/SellDiamondsButton").GetComponent<Button>();
    //    resourceManager = FindObjectOfType<ResourceManager>();
    //    debugManager = transform.Find("/GameCanvas/DebugManager").gameObject;
    //    debugManager.SetActive(false);
    //    winScreen = transform.Find("/GameCanvas/WinScreen").gameObject;
    //    winScreen.SetActive(false);
    //    looseScreen = transform.Find("/GameCanvas/LooseScreen").gameObject;
    //    looseScreen.SetActive(false);
    //    eventBar = FindObjectOfType<EventBar>();
    //}
}
