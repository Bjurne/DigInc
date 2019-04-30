using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public int GoalWeek;
    public int goalDiamondQuantity;

    public bool firstDigGuarantee = true;


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
        resourceManager.PayAllMinersSalary();
        currentWeek++;
        eventBar.ClearEventBar();

        foreach (MiningShaft miningShaft in FindObjectsOfType<MiningShaft>())
        {
            StartCoroutine(miningShaft.SendAllMinersToWork());
        }

        resourceManager.allreadySoldThisTurn = false;
    }

    public void CheckIfGoalWasReached()
    {
        if (currentWeek >= GoalWeek)
        {
            if (resourceManager.numberOfDiamondsOwned >= goalDiamondQuantity)
            {
                Debug.Log("You've made it!");
                winScreen.SetActive(true);
            }
            else
            {
                Debug.Log("You didn't reach the goal! Game over!");
                looseScreen.SetActive(true);
            }
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("PrototypeScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
