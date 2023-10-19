using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupGameTutorial : Popup<PopupGameTutorial>
{
    public Image guideImage;

    private int currentGuideIndex = 0;
    public Sprite[] gudieSprites;

    private void Start()
    {
        Time.timeScale = 0;
        ShowGuideSprite(currentGuideIndex);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(gudieSprites.Length <= currentGuideIndex)
            {
                Time.timeScale = 1;
                RemovePopup();
            }
            else
            {
                ShowGuideSprite(currentGuideIndex++);
            }
        }
    }

    private void ShowGuideSprite(int index)
    {
        guideImage.sprite = gudieSprites[index];
    }

    public override PopupGameTutorial GetPopup()
    {
        return this;
    }
}

