using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeKillerButton : MonoBehaviour
{
    private Image tkbImage;
    public List<Sprite> sprites;
    private Button tkButton; 


    private void Start()
    {
        tkbImage = GetComponent<Image>();
        tkButton = GetComponent<Button>();
    }

    public void KillSomeTime()
    {
        StartCoroutine(TurnOffButton());

        int randomTimeKiller = UnityEngine.Random.Range(0, 3);

        if (randomTimeKiller == 0) Jump();
        if (randomTimeKiller == 1) Spin();
        if (randomTimeKiller == 2) ChangeSprite();
    }



    private IEnumerator TurnOffButton()
    {
        tkButton.interactable = false;
        yield return new WaitForSeconds(1);
        tkButton.interactable = true;
    }

    private void Jump()
        {
            iTween.PunchPosition(gameObject, UnityEngine.Random.insideUnitCircle * 128, 1f);
        }

    private void Spin()
    {
        iTween.ShakeRotation(gameObject, UnityEngine.Random.insideUnitCircle * 128, 1f);
    }

    private void ChangeSprite()
    {
        int randomSprite = UnityEngine.Random.Range(0, 3);
        tkbImage.sprite = sprites[randomSprite];
    }
}
