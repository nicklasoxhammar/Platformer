using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueText
{
    [SerializeField]
    public enum Name
    {
        Elda, President
    }
    [SerializeField] public Name name;
    [TextArea(4, 10)] [SerializeField] public string text;
    public bool textcontinuing = false;
}

public class DialogueSystem : MonoBehaviour {

    [SerializeField] List<DialogueText> dialogues;

    private int index = 0;

    public int GetSize()
    {
        return dialogues.Count;
    }

    public DialogueText GetNextDialogue()
    {
        if(index < dialogues.Count)
        {
            index++;
            if(index != dialogues.Count)
            {
                if (dialogues[index].name == dialogues[index - 1].name)
                    dialogues[index - 1].textcontinuing = true;
            }
            return dialogues[index-1];
        }
        else return null;
    }


}
