using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Terms : UIManagers
{
    public List<Text> textList;
    public List<CheckToggleButton> checkToggleButtons;
    public Button StartButton, AllAgreeButton;
    // Start is called before the first frame update
    void Start()
    {
        StartButton.onClick.AddListener(PushStartButton);
        AllAgreeButton.onClick.AddListener(PushAllAgreeButton);

        RefreshUI();
    }

    public void PushStartButton()
    {
        if (checkToggleButtons == null)
            return;

        // 조건 만족
        if(checkToggleButtons[0].IsOn && checkToggleButtons[1].IsOn)
        {
            UserDataManager.instance.SetFirstTerms(checkToggleButtons[0].IsOn);
            UserDataManager.instance.SetSecondTerms(checkToggleButtons[1].IsOn);
            UserDataManager.instance.SetThirdTerms(checkToggleButtons[2].IsOn);
            UserDataManager.instance.SetForthTerms(checkToggleButtons[3].IsOn);
            AccountSceneManager accountSceneManager = GameObject.Find("AccountSceneManager").GetComponent<AccountSceneManager>();
            accountSceneManager.EndTermsProgress();           
        }
        else // 조건 불만족
        {
            AnnounceManager.instance.ShowAnnounce("동의 항목을 확인부탁드립니다.");
        }

    }

    public void PushAllAgreeButton()
    {
        for(int i=0; i < checkToggleButtons.Count; ++i)
        {
            checkToggleButtons[i].SetToggleButton(true);
        }
    }

    public override void SetText()
    {
        if (textList == null)
            return;

        for (int i = 0; i < textList.Count; ++i)
        {
            textList[i].text = DataManager.instance.GetText(textList[i].gameObject.name);
        }
    }

    public override void RefreshUI()
    {
        SetText();
    }
}
