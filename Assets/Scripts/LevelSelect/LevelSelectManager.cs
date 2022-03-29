using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelSelectManager : MonoBehaviour
{
    // Private variables
    [SerializeField] private float buttonAnimScale = 1.2f;
    [SerializeField] private float buttonAnimDuration = 1f;

    private SceneController sceneController;

    private void Start()
    {
        sceneController = SceneController.INSTANCE;
    }

    public void OnButtonPress(int sceneIndex)
    {
        // Play animation and start to load next scene
        LeanTween.scale(EventSystem.current.currentSelectedGameObject, new Vector3(buttonAnimScale, buttonAnimScale), buttonAnimDuration).setEasePunch();
        sceneController.LoadSceneAsync(sceneIndex);

        // Change scenes after animation is done
        Invoke("ActivateLoadedScene", buttonAnimDuration);
    }

    private void ActivateLoadedScene()
    {
        sceneController.ActivateLoadedScene();
    }
}
