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

    private Vector3 offset;

    //private Camera uiCamera;

    void Start()
    {
        price = transform.Find("Panel/HorizontalLayout/Price").GetComponent<Text>();
        price.text = currentPrice.ToString();
        tagTitle = transform.Find("Panel/PriceTagTitle").GetComponent<Text>();
        tagTitle.text = title;
        offset.Set(Screen.width/12, 0f, 0f);
    }

    private void Update()
    {
        //GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
        if (gameObject.activeInHierarchy)
        {
            transform.position = Input.mousePosition + offset;
        }
    }

    public void SetNewPrice(int newPrice)
    {
        currentPrice = newPrice;
        price.text = currentPrice.ToString();
        if (currentPrice == 0) tagTitle.text = "Maxed out!";
    }
}
