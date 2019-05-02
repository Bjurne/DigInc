using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ToolTipWidget : MonoBehaviour
{
    public string toolTipString;
    public Text toolTipText;

    private Vector3 offset;


    void Start()
    {
        toolTipText = transform.Find("ToolTipText").GetComponent<Text>();
        toolTipText.text = toolTipString;

        StartCoroutine(FixedSetTextProperties());
        //offset.Set(-toolTipText.preferredWidth / 3f, - Screen.height / 12f, 0f);

        //Vector2 backgroundSize = new Vector2(toolTipText.preferredWidth + 8f, toolTipText.preferredHeight + 8f);
        //transform.Find("BackgroundPanel").GetComponent<RectTransform>().sizeDelta = backgroundSize;
    }

    private IEnumerator FixedSetTextProperties()
    {
        yield return new WaitForFixedUpdate();
        offset.Set(-toolTipText.preferredWidth / 2f, -Screen.height / 12f, 0f);

        Vector2 backgroundSize = new Vector2(toolTipText.preferredWidth + 8f, toolTipText.preferredHeight + 8f);
        transform.Find("BackgroundPanel").GetComponent<RectTransform>().sizeDelta = backgroundSize;
        yield return null;
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            transform.position = Input.mousePosition + offset;
        }
    }
}