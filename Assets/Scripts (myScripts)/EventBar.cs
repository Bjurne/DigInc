using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventBar : MonoBehaviour
{
    public List<Event> activeEvents;
    public Transform eventSlot;
    public GameObject eventPrefab;
    public GameObject eventPanel;
    public float eventSpacing;

    public enum EventType { FoundGold, MinerHurt }

    public void RepopulateActiveEvents()
    {
        activeEvents.Clear();
        
        foreach (Event thisEvent in GetComponentsInChildren<Event>())
        {
            activeEvents.Add(thisEvent);
        }
    }

    public void ClearEventBar()
    {
        RepopulateActiveEvents();
        foreach (Event thisEvent in activeEvents)
        {
            thisEvent.DestroyEvent();
        }
    }

    public void AddEvent(EventType eventType)
    {
        if (eventType == EventType.FoundGold) InstantiateFoundGoldEvent();
        else if (eventType == EventType.MinerHurt) InstantiateMinerHurtEvent();
    }

    public void DebugAddEvent()
    {
        //InstantiateFoundGoldEvent();
        InstantiateMinerHurtEvent();
    }

    private void InstantiateFoundGoldEvent()
    {
        GameObject newEvent = Instantiate(eventPrefab, eventPanel.transform.position, Quaternion.identity);
        newEvent.transform.SetParent(eventPanel.transform);
        newEvent.GetComponent<Event>().FoundGold();
    }

    private void InstantiateMinerHurtEvent()
    {
        GameObject newEvent = Instantiate(eventPrefab, eventPanel.transform.position, Quaternion.identity);
        newEvent.transform.SetParent(eventPanel.transform);
        newEvent.GetComponent<Event>().MinerHurt();
    }
}
