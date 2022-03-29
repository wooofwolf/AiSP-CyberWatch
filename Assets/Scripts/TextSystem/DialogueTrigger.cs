using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    // Public Variables
    public TextAsset dialogueFile;

    public GameObject choicesObj;

    public List<Message> messageList = new List<Message>();
    public List<Actor> actorList = new List<Actor>();

    // Private Variables
    private bool hasChoices;
    private DialogueManager dialogueManager;


    private void Start()
    {
        if (dialogueManager == null)
        {
            dialogueManager = FindObjectOfType<DialogueManager>();
        }

        if (choicesObj != null)
        {
            hasChoices = true;
            choicesObj.SetActive(false);
        }
        else
        {
            hasChoices = false;
        }

        if (dialogueFile == null)
        {
            Debug.LogWarning("Dialogue file is empty.");
        }
        else
        {
            GetActorsAndMessagesFromFile();
        }
    }

    public void GetActorsAndMessagesFromFile()
    {
        string str = dialogueFile.text;
        string[] strArray = Regex.Split(str, "\n|\r|\r\n").Where(s => s != string.Empty).ToArray();

        if (strArray[0].Contains("[Actors]"))
        {
            string allActors = strArray[0].Remove(0, 8);
            string[] indivActors = Regex.Split(allActors, ",");
            for (int i = 0; i < indivActors.Length; ++i)
            {
                Actor actor = new Actor() { name = indivActors[i] };
                actorList.Add(actor);
            }
        }
        else
        {
            Debug.LogError("No Actors Found");
        }

        for (int i = 1; i < strArray.Length; ++i)
        {
            string[] messageArr = Regex.Split(strArray[i], ":");
            Message message = new Message() { actorID = int.Parse(messageArr[0]), messages = messageArr[1] };
            messageList.Add(message);
        }
    }

    public void StartDialogue()
    {
        if (!dialogueManager.isActive)
        {
            dialogueManager.StartDialogue(messageList.ToArray(), actorList.ToArray());
            dialogueManager.hasChoices = hasChoices;
            if (hasChoices)
            {
                dialogueManager.choiceSelection = choicesObj;
            }
        }
    }
}

[System.Serializable]
public class Message
{
    public int actorID;

    [TextArea(2, 4)]
    public string messages;

    //public string[] messages;
}

[System.Serializable]
public class Actor
{
    public string name;
    //public Sprite avatar;
}
