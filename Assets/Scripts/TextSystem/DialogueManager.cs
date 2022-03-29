using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    //public Image actorImage;

    // Public Variables
    public TextMeshProUGUI actorName;
    public TextMeshProUGUI messageText;

    public GameObject dialogueBox;

    public GameObject continueSprite;

    [HideInInspector] public GameObject choiceSelection;
    [HideInInspector] public bool hasChoices;
    [HideInInspector] public bool isActive = false;

    // Private Variables
    private Message[] currentMessages;
    private Actor[] currentActors;
    private int activeMessage = 0;

    private bool isWriting;

    private void Start()
    {
        dialogueBox.transform.localScale = new Vector3(0, 0, 0);
    }

    public void StartDialogue(Message[] messages, Actor[] actors)
    {
        if (choiceSelection != null)
        {
            choiceSelection.SetActive(false);
        }

        LeanTween.scale(dialogueBox, new Vector3(1, 1, 1), 1.0f).setEaseOutExpo();

        currentMessages = messages;
        currentActors = actors;
        activeMessage = 0;

        isActive = true;

        DisplayMessage();
    }

    public void DisplayMessage()
    {
        continueSprite.SetActive(false);

        Message messageToDisplay = currentMessages[activeMessage];
        //messageText.text = messageToDisplay.messages;
        StartCoroutine(WriteMessageOut(messageToDisplay.messages));

        Actor actorToDisplay = currentActors[messageToDisplay.actorID];
        actorName.text = actorToDisplay.name;
        //actorImage.sprite = actorToDisplay.sprite;
    }

    public void NextMessage()
    {
        if (isActive)
        {
            if (isWriting)
            {
                // If text is printing, skip the printing and finish the sentence
                StopAllCoroutines();
                messageText.text = currentMessages[activeMessage].messages;
                continueSprite.SetActive(true);

                isWriting = false;
            }
            else
            {
                // Go to next message
                activeMessage++;
                // If current message is not the end
                if (activeMessage < currentMessages.Length)
                {
                    DisplayMessage();
                }
                else
                {
                    //Debug.Log("Conversation Ended!");
                    // If there are choices, continue with choice. Otherwise, end the dialogue
                    if (hasChoices)
                    {
                        choiceSelection.SetActive(true);
                        isActive = false;
                    }
                    else
                    {
                        LeanTween.scale(dialogueBox, new Vector3(0, 0, 0), 1.0f).setEaseOutExpo();
                        isActive = false;
                    }
                }
            }
        }
    }

    // Writes out the text letter by letter
    IEnumerator WriteMessageOut(string messsageText)
    {

        isWriting = true;

        string sentence = messsageText;
        messageText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            messageText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        continueSprite.SetActive(true);

        isWriting = false;
    }
}
