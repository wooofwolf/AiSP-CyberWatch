using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoAnimation : MonoBehaviour
{
    public GameObject ContinueText;

    public bool isDone = false;

    private SplashScreenManager splashManager;

    public void Start()
    {
        ContinueText.SetActive(isDone);
        splashManager = GameManager.FindObjectOfType<SplashScreenManager>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && isDone)
        {
            splashManager.swipeTransition.SetActive(true);
        }
    }

    public void ShowText()
    {
        isDone = true;
        ContinueText.SetActive(isDone);
        GetComponent<Animator>().SetBool("Finish", isDone);
    }
}
