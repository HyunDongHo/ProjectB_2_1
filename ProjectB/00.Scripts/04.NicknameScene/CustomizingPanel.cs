using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizingPanel : MonoBehaviour
{
    public enum NowCustomizingMode
    {
        None,
        Preset,
        Hair,
        Face,
        Cloth
    }

    NowCustomizingMode nowCustomizingMode = NowCustomizingMode.Preset;

    [Space]
    public NicknameSceneManager nicknameSceneManager;

    [Space]
    public Button backButton, PresetButton, HairButton, FaceButton, CostumeButton, CreateButton;
    public Nickname nicknamePanel;

    [Space]
    public GameObject presetPanel, hairListPanel, hairColorPanel, faceColorListPanel, clothListPanel;

    [Space]
    public GridLayoutGroup clothGroup;
    public GameObject clothItem;

    [Space]
    public GridLayoutGroup presetGroup;    
    public GameObject presetItem;

    [Space]
    public GameObject hairStyleObj;
    public GridLayoutGroup hairListGroup;

    [Space]
    public GridLayoutGroup hairColorGroup;
    public GameObject hairColorObj;

    [Space]
    public GridLayoutGroup faceColorGroup;
    public GameObject faceColorObj;

    bool isPresetListInit = false;
    bool isHairListInit = false;
    bool isfaceColorListInit = false;
    bool isClothListInit = false;

    List<HairColorItem> hairColorList = new List<HairColorItem>();

    private void Awake()
    {
        AddEvent();
        SetMode(NowCustomizingMode.None);
    }

    public void AddEvent()
    {
    }

    public void RemoveEvent()
    {
    }

    void HandleBackButton()
    {
        SceneSettingManager.instance.LoadAccountScene(isFade: true);
    }
    void HandleFaceButton()
    {
        Camera.main.GetComponent<NicknameCamera>().ZoomIn();
        SetMode(NowCustomizingMode.Face); 
    }

    void HandleCreateButton()
    {
        nicknamePanel.gameObject.SetActive(true);
    }

    void SetMode(NowCustomizingMode customizingMode)
    {
        if (nowCustomizingMode == customizingMode)
            return;

        nowCustomizingMode = customizingMode;

        switch(nowCustomizingMode)
        {
            case NowCustomizingMode.None:
                presetPanel.gameObject.SetActive(false);
                hairListPanel.gameObject.SetActive(false);
                hairColorPanel.gameObject.SetActive(false);
                faceColorListPanel.gameObject.SetActive(false);
                clothListPanel.gameObject.SetActive(false);
                break;
            case NowCustomizingMode.Preset:
                presetPanel.gameObject.SetActive(true);
                hairListPanel.gameObject.SetActive(false);
                hairColorPanel.gameObject.SetActive(false);
                faceColorListPanel.gameObject.SetActive(false);
                clothListPanel.gameObject.SetActive(false);
                break;
            case NowCustomizingMode.Hair:
                presetPanel.gameObject.SetActive(false);
                hairListPanel.gameObject.SetActive(true);
                hairColorPanel.gameObject.SetActive(true);
                faceColorListPanel.gameObject.SetActive(false);
                clothListPanel.gameObject.SetActive(false);
                break;
            case NowCustomizingMode.Face:
                presetPanel.gameObject.SetActive(false);
                hairListPanel.gameObject.SetActive(false);
                hairColorPanel.gameObject.SetActive(false);
                faceColorListPanel.gameObject.SetActive(true);
                clothListPanel.gameObject.SetActive(false);
                break;
            case NowCustomizingMode.Cloth:
                presetPanel.gameObject.SetActive(false);
                hairListPanel.gameObject.SetActive(false);
                hairColorPanel.gameObject.SetActive(false);
                faceColorListPanel.gameObject.SetActive(false);
                clothListPanel.gameObject.SetActive(true);
                break;
        }
    }
}
