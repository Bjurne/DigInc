using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PriceTagWidget : MonoBehaviour
{
    public string title;
    public int currentPrice;
    private Text price;
    private Text tagTitle;

    void Start()
    {
        price = transform.Find("Canvas/Panel/HorizontalLayout/Price").GetComponent<Text>();
        price.text = currentPrice.ToString();
        tagTitle = transform.Find("Canvas/Panel/PriceTagTitle").GetComponent<Text>();
        tagTitle.text = title;
    }

    private void Update()
    {
        //GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
        if (gameObject.activeInHierarchy)
        {
            transform.position = Input.mousePosition;
        }
    }

    public void SetNewPrice(int newPrice)
    {
        currentPrice = newPrice;
        price.text = currentPrice.ToString();
        if (currentPrice == 0) tagTitle.text = "Maxed out!";
    }
}
