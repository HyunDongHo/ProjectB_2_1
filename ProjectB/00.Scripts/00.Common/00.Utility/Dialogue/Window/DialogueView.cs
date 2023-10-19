using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scheduler;
using DG.Tweening;

public class DialogueView : MonoBehaviour
{
    public const float DIALOGUE_SPEED = 25f;

    public CanvasGroup canvasGroup;

    [Space]

    public Text dialogueText;

    [Space]

    public Image npcImage;
    public Text npcNameText;

    [Space]

    public Button skipButton;

    public void FadeIn(float duration, Action OnComplete = null)
    {
        canvasGroup.alpha = 1;

        canvasGroup.DOFade(0, duration).OnComplete(() => OnComplete?.Invoke());
    }

    public void FadeOut(float duration, Action OnComplete = null)
    {
        canvasGroup.alpha = 0;

        canvasGroup.DOFade(1, duration).OnComplete(() => OnComplete?.Invoke());
    }

    public void SetNPCName(string npcName)
    {
        npcNameText.text = npcName;
    }

    public void SetNPCSprite(Sprite npcSprite)
    {
        npcImage.sprite = npcSprite;
    }

    public void SetDialogueText(string text, bool isAnimation, Action OnComplete)
    {
        dialogueText.text = string.Empty;

        dialogueText.DOKill();
        if (isAnimation)
        {
            float speed = text.Length / DIALOGUE_SPEED;
            dialogueText.DOText(text, speed).SetEase(Ease.Linear).OnComplete(() => OnComplete?.Invoke());
        }
        else
        {
            dialogueText.text = text;

            OnComplete?.Invoke();
        }
    }
}
