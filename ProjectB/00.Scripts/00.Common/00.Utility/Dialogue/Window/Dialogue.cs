using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueJson
{
    [System.Serializable]
    public class Data
    {
        public string npc;
        public string script;
        public string sound;
    }
    public List<Data> dialogue = new List<Data>();
}

public class Dialogue : MonoBehaviour
{
    public DialogueView dialogueView;
   
    private DialogueJson savedDialogueJson;

    private int dialogueIndex = 0;
    private bool isPlaying = false;

    private AudioSource currentPlayAudioSource;

    public Action<Dialogue> OnDialogueEnd;

    private void Awake()
    {
        dialogueView.FadeOut(DefineManager.DEFAULT_FADE_DURATION);
    }

    public void Init(DialogueJson dialogueJson)
    {
        savedDialogueJson = dialogueJson;

        dialogueView.skipButton.onClick.AddListener(EventDialogueEnd);

        SetDialogue();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            SetDialogue();
    }

    private void SetDialogue()
    {
        currentPlayAudioSource?.Stop();
        currentPlayAudioSource = null;

        if (isPlaying)
        {
            SetDialogueText(isAnimation: false, isPlaySound: false);
        }
        else
        {
            if (savedDialogueJson.dialogue.Count >= dialogueIndex + 1)
            {
                SetDialogueText(isAnimation: true, isPlaySound: true);
            }
            else
            {
                EventDialogueEnd();
            }
        }
    }

    private void SetDialogueText(bool isAnimation, bool isPlaySound)
    {
        isPlaying = true;

        DialogueJson.Data data = savedDialogueJson.dialogue[dialogueIndex];

        dialogueView.SetNPCName(data.npc);
        dialogueView.SetNPCSprite(ResourceManager.instance.Load<Sprite>(data.npc));

        if(isPlaySound)
            if (!string.IsNullOrEmpty(data.sound))
                currentPlayAudioSource = SoundManager.instance.PlaySound(data.sound);

        dialogueView.SetDialogueText(data.script, isAnimation,
            OnComplete: () =>
            {
                dialogueIndex += 1;
                isPlaying = false;
            });
    }

    private void EventDialogueEnd()
    {
        currentPlayAudioSource?.Stop();

        OnDialogueEnd?.Invoke(this);

        Destroy(this.gameObject);
    }
}
