using Data;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
public interface ILoaderXml<Key, Item>
{
    Dictionary<Key, Item> MakeDic();
    
    bool Validate();
}
public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager : Singleton<DataManager>{

    public Dictionary<string, TextData> Texts { get; private set; }
    public Dictionary<string, Data.ItemDataJson> ItemDict { get; private set; } = new Dictionary<string, Data.ItemDataJson>();
    //public Dictionary<string, List<HairColor>> HairColorDict { get; private set; } = new Dictionary<string, List<HairColor>>();

    public void Awake()
    {
        Texts = LoadXml<TextDataLoader, string, TextData>("TextData").MakeDic();
        ItemDict = LoadJson<Data.ItemDataLoader, string, Data.ItemDataJson>("ItemData").MakeDict();
      //  HairColorDict = LoadJson<HairColorLoader, string, List<HairColor>>("HairColorData").MakeDict();
    }
    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Resources.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }

    public string GetText(string id)
    {
        if (Texts.TryGetValue(id, out TextData value) == false)
            return "";

        return UserDataManager.instance.GetIsKorean() ? value.kor.Replace("{userName}", "테스트 유저") : value.eng.Replace("{userName}", "테스트 유저");
    }

    private Item LoadSingleXml<Item>(string name)
    {
        XmlSerializer xs = new XmlSerializer(typeof(Item));
        TextAsset textAsset = Resources.Load<TextAsset>(name);
        using (MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(textAsset.text)))
            return (Item)xs.Deserialize(stream);
    }

    private Loader LoadXml<Loader, Key, Item>(string name) where Loader : ILoaderXml<Key, Item>, new()
    {
        XmlSerializer xs = new XmlSerializer(typeof(Loader));
        TextAsset textAsset = Resources.Load<TextAsset>(name);
        using (MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(textAsset.text)))
            return (Loader)xs.Deserialize(stream);
    }
}
