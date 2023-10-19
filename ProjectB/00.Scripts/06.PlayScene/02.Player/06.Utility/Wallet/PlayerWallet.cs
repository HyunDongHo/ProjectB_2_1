using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

public class PlayerWallet : MonoBehaviour
{
    private ObscuredInt gold;
    public Action<int> OnGoldSet;
    public Action<int> OnGoldAdd;

    private ObscuredInt ruby;
    public Action<int> OnRubySet;
    public Action<int> OnRubyAdd;

    private ObscuredInt fameCoin;
    public Action<int> OnFameCoinSet;
    public Action<int> OnFameCoinAdd;

    private ObscuredInt core;
    public Action<int> OnCoreSet;
    public Action<int> OnCoreAdd;

    private void Start()
    {
        AntiCheatRandomizeCryptoKey.instance.Add(gold);
        AntiCheatRandomizeCryptoKey.instance.Add(ruby);
        AntiCheatRandomizeCryptoKey.instance.Add(fameCoin);
        AntiCheatRandomizeCryptoKey.instance.Add(core);
    }

    #region Gold

    public void SetGold(int gold)
    {
        this.gold = gold;

        OnGoldSet?.Invoke(this.gold);
    }

    public void AddGold(int gold)
    {
        SetGold(this.gold + gold);

        OnGoldAdd?.Invoke(this.gold);
    }

    public bool IsAvailiableRemoveCoin(int removeAmount)
    {
        return this.gold - removeAmount >= 0;
    }

    public int GetCoin()
    {
        return gold;
    }

    #endregion

    #region Ruby

    public void SetRuby(int ruby)
    {
        this.ruby = ruby;

        OnRubySet?.Invoke(this.ruby);
    }

    public void AddRuby(int ruby)
    {
        SetRuby(this.ruby + ruby);

        OnRubyAdd?.Invoke(this.ruby);
    }

    public bool IsAvailiableRemoveRuby(int removeAmount)
    {
        return this.ruby - removeAmount >= 0;
    }

    public int GetRuby()
    {
        return ruby;
    }

    #endregion

    #region FameCoin

    public void SetFameCoin(int fameCoin)
    {
        this.fameCoin = fameCoin;

        OnFameCoinSet?.Invoke(this.gold);
    }

    public void AddFameCoin(int fameCoin)
    {
        SetFameCoin(this.fameCoin + fameCoin);

        OnFameCoinAdd?.Invoke(this.fameCoin);
    }

    public bool IsAvailiableRemoveFameCoin(int removeAmount)
    {
        return this.fameCoin - removeAmount >= 0;
    }

    public int GetFameCoin()
    {
        return fameCoin;
    }

    #endregion

    #region Core

    public void SetCore(int core)
    {
        this.core = core;

        OnCoreSet?.Invoke(this.core);
    }

    public void AddCore(int core)
    {
        SetCore(this.core + core);

        OnCoreAdd?.Invoke(this.core);
    }

    public bool IsAvailiableRemoveCore(int removeAmount)
    {
        return this.core - removeAmount >= 0;
    }

    public int GetCore()
    {
        return core;
    }

    #endregion
}
