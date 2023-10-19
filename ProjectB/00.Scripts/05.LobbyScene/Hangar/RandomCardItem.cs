using Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class RandomCardItem : MonoBehaviour
{
    public int cardNum;
    public MeshRenderer MeshRenderer;
    AsyncOperationHandle Handle;

    public GameObject SelectCard;

    public RandomCardPriceItem randomCardPriceItem;

    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer = GetComponentInChildren<MeshRenderer>();
        randomCardPriceItem = GetComponentInChildren<RandomCardPriceItem>();
        SelectCard = transform.Find("Random_Card_Outer").gameObject;
        SelectCard.SetActive(false);
    }

    public void OnRandomCardPriceItem()
    {
        if(randomCardPriceItem == null)
            GetComponentInChildren<RandomCardPriceItem>();

        randomCardPriceItem.gameObject.SetActive(true);
    }
    public void OffRandomCardPriceItem()
    {
        if (randomCardPriceItem == null)
            GetComponentInChildren<RandomCardPriceItem>();

        randomCardPriceItem.gameObject.SetActive(false);
    }

    public void ChangeMaterial(string itemName, int needItemCount)
    {
        ItemDataJson itemData = null;
        DataManager.instance.ItemDict.TryGetValue(itemName, out itemData);

        if (itemData == null)
            return;

        randomCardPriceItem.SetNeedItemUI(itemData.rarity, needItemCount);

        MeshRenderer.enabled = false;
        StringBuilder sb = new StringBuilder();
        sb.Append("Card/").Append(itemData.type).Append("/").Append(itemData.rarity).Append("/").Append(itemData.itemName).Append("-Card").Append(".mat");      

        Addressables.LoadAssetAsync<Material>(sb.ToString()).Completed +=
        (AsyncOperationHandle<Material> Obj) =>
        {
            Handle = Obj;
            MeshRenderer.material = Obj.Result;
            MeshRenderer.enabled = true;
        };        
    }

    public void Clear()
    {
        Addressables.Release(Handle);
    }
}
