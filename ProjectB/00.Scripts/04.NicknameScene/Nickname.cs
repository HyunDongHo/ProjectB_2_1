using BackEnd;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class Nickname : UIManagers
{
    public List<Text> textList;
    public Text inputNicknameText;
    public Button okButton;
    public Button cancleButton;

    public Action nickCreateComp = null;

    bool _isClickOkButton = false;
    string _inputName;

    void Start()
    {
        okButton.onClick.RemoveListener(CreateNickName);
        okButton.onClick.AddListener(CreateNickName);
        cancleButton.onClick.AddListener(CancleNicknameButton);
        RefreshUI();
    }

    private void OnEnable()
    {
        _isClickOkButton = false;
    }

    public override void SetText()
    {
        if (textList == null)
            return;

        for(int i=0; i<textList.Count; ++i)
        {
            textList[i].text = DataManager.instance.GetText(textList[i].gameObject.name);
        }
    }

    public override void RefreshUI()
    {
        SetText();
    }
    public void CreateNickName()
    {
        if (_isClickOkButton == true)
            return;

        _isClickOkButton = true;

        if (CheckNickname(inputNicknameText.text) == false)
        {
            AnnounceManager.instance.ShowAnnounce("닉네임은 한글, 영어, 숫자로만 만들 수 있습니다");
            _isClickOkButton = false;
            return;
        }

        if (inputNicknameText.text.Length == 0)
        {
            AnnounceManager.instance.ShowAnnounce("빈 닉네임입니다");
            _isClickOkButton = false;
            return;
        }
        else if (inputNicknameText.text.Length > 10)
        {
            AnnounceManager.instance.ShowAnnounce("닉네임은 최대 10글자를 넘길 수 없습니다");
            _isClickOkButton = false;
            return;
        }
        else if (inputNicknameText.text.Length < 2)
        {
            AnnounceManager.instance.ShowAnnounce("닉네임은 최소 2글자를 넘어야합니다");
            _isClickOkButton = false;
            return;
        }
        else  //클라이언트 성공
        {
            Backend.BMember.CreateNickname(inputNicknameText.text, callback =>
            {
                if (callback.IsSuccess())
                {
                    nickCreateComp?.Invoke();
                }
                else
                {
                    if (callback.GetStatusCode() == "409")
                    {
                        AnnounceManager.instance.ShowAnnounce("이미 존재하는 이름입니다.");
                        _isClickOkButton = false;
                    }
                    else if (callback.GetStatusCode() == "400")
                    {
                        AnnounceManager.instance.ShowAnnounce("비정상적인 이름입니다.");
                        _isClickOkButton = false;
                    }
                }
            });
        }

    }
    
    public void CancleNicknameButton()
    {
        gameObject.SetActive(false);        
    }
    private bool CheckNickname(string nickName)
    {
        return Regex.IsMatch(nickName, "^[0-9a-zA-Z가-힣]*$");
    }

}
