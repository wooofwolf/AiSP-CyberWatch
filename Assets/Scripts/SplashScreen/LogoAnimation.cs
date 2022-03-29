using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoAnimation : MonoBehaviour
{
    public GameObject ContinueText;

    public bool isDone = false;

    public void Start()
    {
        ContinueText.SetActive(isDone);
    }

    public void ShowText()
    {
        isDone = true;
        ContinueText.SetActive(isDone);
        GetComponent<Animator>().SetBool("Finish", isDone);
    }
}
