using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnounceManager : Singleton<AnnounceManager>
{
    public void ShowAnnounce(string text)
    {
        Announce announce = ObjectPoolManager.instance.CreateObject(ResourceManager.instance.Load<GameObject>("Announce"), transform).GetComponent<Announce>();
        announce.ShowAnnounce(text);
    }
}
