using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RandomCardPriceItem : MonoBehaviour
{

    [Space]
    public Image NeedItemImage;
    public TextMeshProUGUI NeedItemText;

    float xMinValue = 11.32f;
    float xMaxValue = 13.8f;

    private void FixedUpdate()
    {
        if (xMinValue > transform.position.x || xMaxValue < transform.position.x)
        {
            NeedItemImage.enabled = false;
            NeedItemText.enabled = false;
        }
        else
        {
            NeedItemImage.enabled = true;
            NeedItemText.enabled = true;
        }
    }

    public void SetNeedItemUI(string itemRarity, int needItemCount)
    {
        switch(itemRarity)
        {
            case "Common":
            case "Magic":
                NeedItemImage.sprite = ResourceManager.instance.Load<Sprite>("Gold-Icon");
                break;
            case "Rare":
            case "Unique":
            case "Legendry":
            case "Mythic":
                NeedItemImage.sprite = ResourceManager.instance.Load<Sprite>("Ruby-Icon");
                break;
        }

        NeedItemText.text = needItemCount.ToString();
    }


}
