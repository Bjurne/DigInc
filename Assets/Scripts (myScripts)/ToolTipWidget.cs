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
    private Vector3 deltaOffset;

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
        offset.Set(0f, -Screen.height / 32f, 0f);

        //Vector2 backgroundSize = new Vector2(toolTipText.preferredWidth + 8f, toolTipText.preferredHeight + 8f);
        Vector2 backgroundSize = new Vector2(100 + 8f, toolTipText.preferredHeight + 8f);
        transform.Find("BackgroundPanel").GetComponent<RectTransform>().sizeDelta = backgroundSize;
        yield return null;
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            transform.position = Input.mousePosition + offset;

            float posClamp = 100f;

            if (transform.position.x < posClamp) deltaOffset.Set(posClamp * 2, 0f, 0f);
            if (transform.position.y < posClamp) deltaOffset.Set(0f, posClamp * 2, 0f);
            if (transform.position.x > Screen.width - posClamp) deltaOffset.Set(-posClamp * 2, 0f, 0f);
            if (transform.position.y > Screen.height - posClamp) deltaOffset.Set(0f, -posClamp * 2, 0f);
            //else deltaOffset.Set(0f, 0f, 0f);

            transform.position += deltaOffset;

        }
    }
}