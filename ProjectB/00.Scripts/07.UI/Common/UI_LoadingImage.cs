using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LoadingImage : MonoBehaviour
{
    private RectTransform _loadingImage;
    public float duration = 1;
    
    private void Awake()
    {
        _loadingImage = GetComponent<RectTransform>();
    }

    public void SetLoadingState(bool isLoading)
    {
        _loadingImage = GetComponent<RectTransform>();
        if (isLoading)
        {
            _loadingImage.gameObject.SetActive(true);
            _loadingImage.DOLocalRotate(new Vector3(0f, 0f, -360f), duration, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear);
        }
        else
        {
            _loadingImage.DOKill();
            gameObject.SetActive(false);
        }
    }

}
