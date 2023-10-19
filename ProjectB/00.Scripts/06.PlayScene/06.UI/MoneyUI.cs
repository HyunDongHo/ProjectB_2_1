using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyUI : MonoBehaviour
{
    public Text coinText;
    public Text rubyText;

    private void Awake()
    {
        AddEvent();
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }

    private void AddEvent()
    {
        StageManager.instance.playerControl.utility.belongings.playerWallet.OnGoldSet += HandleOnCoinSet;
        StageManager.instance.playerControl.utility.belongings.playerWallet.OnRubySet += HandleOnRubySet;
    }

    private void RemoveEvent()
    {
        StageManager.instance.playerControl.utility.belongings.playerWallet.OnGoldSet -= HandleOnCoinSet;
        StageManager.instance.playerControl.utility.belongings.playerWallet.OnRubySet -= HandleOnRubySet;
    }

    private void HandleOnCoinSet(int coin)
    {
        coinText.text = coin.ToString();
    }

    private void HandleOnRubySet(int ruby)
    {
        rubyText.text = ruby.ToString();
    }
}
