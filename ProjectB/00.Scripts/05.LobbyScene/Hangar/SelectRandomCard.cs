using Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SelectRandomCard : MonoBehaviour
{
    public MeshRenderer MeshRenderer;
    Material material;
    AsyncOperationHandle Handle;
    public int SelectIndex { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer = GetComponentInChildren<MeshRenderer>();  
    }

    public void ChangeMaterial(string itemName)
    {
        ItemDataJson itemData = null;
        DataManager.instance.ItemDict.TryGetValue(itemName, out itemData);

        if (itemData == null)
            return;

        StringBuilder sb = new StringBuilder();
        sb.Append("Card/").Append(itemData.type).Append("/").Append(itemData.rarity).Append("/").Append(itemData.itemName).Append("-Card").Append(".mat");

        Addressables.LoadAssetAsync<Material>(sb.ToString()).Completed +=
        (AsyncOperationHandle<Material> Obj) =>
        {
            Handle = Obj;
            MeshRenderer.material = Obj.Result;
           // material = Obj.Result;
        };        
    }

    public void Clear()
    {
        Addressables.Release(Handle);
    }
}
