using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreenManager : MonoBehaviour
{
    // Public Variables
    public GameObject sceneFadeChange;

    // Private Variables
    private SceneController sceneController;
    private GameManager gameManager;

    private LogoAnimation logo;

    // Start is called before the first frame update
    void Start()
    {
        sceneController = SceneController.INSTANCE;
        gameManager = GameManager.INSTANCE;

        sceneController.LoadSceneAsync(1);

        logo = GameObject.Find("SplashLogo").GetComponent<LogoAnimation>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonUp(0) && logo.isDone)
        {
            sceneController.ActivateLoadedScene();
        }
    }
}
