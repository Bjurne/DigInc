using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;


public class ToolTipTrigger : EventTrigger
{
    private ToolTipWidget toolTipPrefab;
    private ToolTipWidget thisToolTip;

    public string toolTipString;
    private bool deactivated;

    private float toolTipTimer;
    private bool toolTipTimerTicking;

    private void Awake()
    {
        toolTipPrefab = Resources.Load<ToolTipWidget>("Prefabs/ToolTip");
        thisToolTip = Instantiate(toolTipPrefab, toolTipPrefab.transform.position, Quaternion.identity);
        thisToolTip.transform.SetParent(gameObject.transform, false);
        thisToolTip.gameObject.SetActive(false);
        SetToolTip();
    }

    private void SetToolTip()
    {
        thisToolTip.toolTipString = toolTipString;
    }

    private void Update()
    {
        if (toolTipTimerTicking)
        {
            if (toolTipTimer <= 3f)
            {
                toolTipTimer += Time.deltaTime;
            }
            else
            {
                toolTipTimerTicking = false;
                toolTipTimer = 0f;
                ShowToolTip();
            }
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        foreach (ToolTipTrigger trigger in FindObjectsOfType<ToolTipTrigger>())
        {
            trigger.HideToolTip();
        }
        if (!deactivated)
        {
            toolTipTimerTicking = true;
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        thisToolTip.gameObject.SetActive(false);
        toolTipTimerTicking = false;
        toolTipTimer = 0f;

        foreach (ToolTipTrigger trigger in FindObjectsOfType<ToolTipTrigger>())
        {
            trigger.HideToolTip();
        }

    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.INSTANCE.Play(AudioManager.INSTANCE.clickSound);
        HideToolTip();
    }

    private void ShowToolTip()
    {
        deactivated = false;
        thisToolTip.gameObject.SetActive(true);
        thisToolTip.transform.localScale = Vector3.one;
        iTween.ScaleFrom(thisToolTip.gameObject, Vector3.zero, 3f);
    }

    public void HideToolTip()
    {
        toolTipTimerTicking = false;
        toolTipTimer = 0f;
        thisToolTip.gameObject.SetActive(false);
    }

    public void Deactivate()
    {
        deactivated = true;
    }
}