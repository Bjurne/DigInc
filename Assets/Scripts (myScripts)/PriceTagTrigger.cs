﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;


public class PriceTagTrigger : EventTrigger
{
    private PriceTagWidget priceTagPrefab;
    private PriceTagWidget thisTag;
    private ResourceManager resourceManager;

    private Text price;
    private Text tagTitle;
    private bool deactivated;

    private enum TagType {MinerSlot, Tool, Rank, ExpandShaft};
    [SerializeField] //Set type in Inspector, add new if needed
    private TagType thisTagType;

    private void Awake()
    {
        priceTagPrefab = Resources.Load<PriceTagWidget>("Prefabs/PriceTag");
        thisTag = Instantiate(priceTagPrefab, transform.position, Quaternion.identity);
        thisTag.transform.SetParent(this.transform);
        thisTag.gameObject.SetActive(false);
        resourceManager = FindObjectOfType<ResourceManager>();
        SetTagType();
    }

    private void SetTagType()
    {
        switch (thisTagType)
        {
            case TagType.MinerSlot:
                SetMinerTag();
                break;
            case TagType.Tool:
                SetToolTag();
                break;
            case TagType.Rank:
                SetRankTag();
                break;
            case TagType.ExpandShaft:
                SetExpandShaftTag();
                break;
            default:
                break;
        }
    }

    private void SetRankTag()
    {
        thisTag.title = "Educate";
        thisTag.currentPrice = resourceManager.rankLevel1Cost;
    }

    private void SetToolTag()
    {
        thisTag.title = "Upgrade Tool";
        thisTag.currentPrice = resourceManager.toolLevel1Cost;
    }

    private void SetMinerTag()
    {
        thisTag.title = "Hire Miner";
        thisTag.currentPrice = resourceManager.hireMinerCost;
    }

    private void SetExpandShaftTag()
    {
        thisTag.title = "Expand shaft";
        thisTag.currentPrice = resourceManager.shaftExpansionCost;
    }

    public void UpdatePrice(int newPrice)
    {
        thisTag.SetNewPrice(newPrice);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (!deactivated) thisTag.gameObject.SetActive(true);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        thisTag.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        thisTag.gameObject.SetActive(false);
    }

    public void Deactivate()
    {
        deactivated = true;
    }
}