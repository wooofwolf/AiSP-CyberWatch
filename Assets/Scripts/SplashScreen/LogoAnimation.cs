using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoAnimation : MonoBehaviour
{
    public GameObject text;

    public bool isDone;

    void Start()
    {
        text.SetActive(false);
    }

    public void ShowText()
    {
        isDone = true;
        gameObject.GetComponent<Animator>().SetBool("Finish", isDone);
        text.SetActive(isDone);
    }
}
