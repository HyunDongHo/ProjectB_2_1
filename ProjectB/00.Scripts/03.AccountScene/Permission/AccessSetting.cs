using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccessSetting : UIManagers
{
    public Text AccessTitleText;
    public Text AccessDetailText1;
    public Text AccessDetailText2;
    public Text ConfirmButtonText;
    public Button ConfirmButton;
    public bool bOnCheckPermission = false;

    void Start()
    {
        Button okButton = GetComponentInChildren<Button>();
        ConfirmButton.onClick.AddListener(() =>
        {
            //TODO : 안드로이드 액세스 권한 API호출
            //권한 허용후 가정
            {
                AccountSceneManager accountSceneManager = GameObject.Find("AccountSceneManager").GetComponent<AccountSceneManager>();
                accountSceneManager.OnEventCheckPermission();                
            }            
        });

        RefreshUI();
    }

    // Update is called once per frame
    public override void RefreshUI()
    {
        base.RefreshUI();

        AccessTitleText.text = DataManager.instance.GetText(AccessTitleText.name);
        AccessDetailText1.text = DataManager.instance.GetText(AccessDetailText1.name);
        AccessDetailText2.text = DataManager.instance.GetText(AccessDetailText2.name);
        ConfirmButtonText.text = DataManager.instance.GetText(ConfirmButtonText.name);
    }
}
