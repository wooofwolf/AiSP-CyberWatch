using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextReader : MonoBehaviour
{
    // Private Variables
    [SerializeField] private float textDelay;

    private TextMeshProUGUI textMeshPro;
    private bool isWriting;

    public void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        isWriting = false;

        StartCoroutine(WriteMessageOut(textDelay));
    }

    // Reads out text from UI in gameobject
    IEnumerator WriteMessageOut(float delay)
    {
        isWriting = true;

        string sentence = textMeshPro.text;
        textMeshPro.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            textMeshPro.text += letter;
            yield return new WaitForSeconds(delay);
        }

        isWriting = false;
    }
}
