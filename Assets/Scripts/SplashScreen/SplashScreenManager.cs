using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreenManager : MonoBehaviour
{
    // Public Variables
    public GameObject swipeTransition;

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
}
