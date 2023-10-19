using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
    public Dialogue CreateDialogue(DialogueJson dialogueJson)
    {
        Dialogue dialogue = Instantiate(ResourceManager.instance.Load<GameObject>("Dialogue")).GetComponent<Dialogue>();
        dialogue.Init(dialogueJson);

        return dialogue;
    }
}
