using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DownResources : UIManagers
{
    public Text DownTitleText, DownDetailText1, DownDetailText2, ConfirmButtonText, CancleButtonText, DownResourcesSizeText;
    public Button ConfirmButton, CancleButton;
    // Start is called before the first frame update
    void Start()
    {
        ConfirmButton.onClick.AddListener(() => { ConfirmDownload(); });
        CancleButton.onClick.AddListener(() => { CancleDownload(); });
        RefreshUI();
    }

    public void ConfirmDownload()
    {

    }
    public void CancleDownload()
    {

    }

    public override void RefreshUI()
    {
        base.RefreshUI();

        DownTitleText.text = DataManager.instance.GetText(DownTitleText.name);
        DownDetailText1.text = DataManager.instance.GetText(DownDetailText1.name);
        DownDetailText2.text = DataManager.instance.GetText(DownDetailText2.name);
        ConfirmButtonText.text = DataManager.instance.GetText(ConfirmButtonText.name);
        CancleButtonText.text = DataManager.instance.GetText(CancleButtonText.name);
        DownResourcesSizeText.text = "250M";
    }
}
