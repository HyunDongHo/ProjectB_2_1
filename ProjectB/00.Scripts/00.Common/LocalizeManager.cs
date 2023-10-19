using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Language
{
    KR
}

public class LocalizeData
{
    public string kr;
}

public class LocalizeManager : Singleton<LocalizeManager>
{
    private Dictionary<string, LocalizeData> localizeDatas = new Dictionary<string, LocalizeData>();

    public void AddLocalizeDatas(string key, LocalizeData localizeData)
    {
        localizeDatas.Add(key, localizeData);
    }

    public string GetLocalize(string key)
    {
        Language language = Language.KR;

        string localize = key;

        if(localizeDatas.ContainsKey(key))
        {
            switch (language)
            {
                case Language.KR:
                    localize = localizeDatas[key].kr;
                    break;
            }
        }

        return localize;
    }
}
