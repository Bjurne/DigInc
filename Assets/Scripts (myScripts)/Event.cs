using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Event : MonoBehaviour
{
    EventBar eventBar;
    public Text eventTitle;
    public Text eventDescription;

    public Sprite GoldSprite;
    public Sprite HurtSprite;
    
    public void MinerHurt()
    {
        eventTitle.text = "Miner hurt!";
        eventDescription.text = "A miner was hurt while working!";
        GetComponent<Image>().sprite = HurtSprite;
    }

    public void FoundGold()
    {
        eventTitle.text = "Gold found!";
        eventDescription.text = "A miner found some gold!";
        GetComponent<Image>().sprite = GoldSprite;
    }

    public void DestroyEvent()
    {
        Destroy(this.gameObject);
    }
}
