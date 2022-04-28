using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SplashScreenManager : MonoBehaviour
{
    // Public Variables
    public GameObject swipeTransition;

    public GameObject passwordObject;
    //public InputField passwordField;

    public TMP_InputField passwordField;

    public GameObject splashLogoObject;

    // Private Variables
    private SceneController sceneController;
    private GameManager gameManager;
    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        sceneController = SceneController.INSTANCE;
        gameManager = GameManager.INSTANCE;
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        sceneController.LoadSceneAsync(1);
    }

    void Update()
    {
        if (passwordObject.activeSelf != false)
        {
            splashLogoObject.SetActive(false);
        }
        else
        {
            splashLogoObject.SetActive(true);
        }
    }

    public void CheckPassword()
    {
        if (passwordField.text.Equals("Cyberwatch"))
        {
            passwordObject.SetActive(false);
        }
    }
}
