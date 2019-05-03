using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellDiamondsPrompt : MonoBehaviour
{
    public Button sellButton;

    private void OnEnable()
    {
        sellButton.interactable = false;
    }

    private void OnDisable()
    {
        sellButton.interactable = true;
    }
}
