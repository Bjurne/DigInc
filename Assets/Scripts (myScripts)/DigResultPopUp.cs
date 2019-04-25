using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DigResultPopUp : MonoBehaviour
{
    public Image digResultImage;
    public Text digResultAmount;
    public float risingSpeed;

    void Start()
    {
        StartCoroutine(DestroyAfterDelay());
        iTween.MoveBy(gameObject, Vector3.up * risingSpeed * Time.deltaTime, 3f);
    }
    

    public void SetDigResult(int amount)
    {
        digResultAmount.text = amount.ToString();
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
        yield return null;
    }
}
