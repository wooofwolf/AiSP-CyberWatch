using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    // Public variables
    public GameObject[] shoppingStars;

    // Private variables
    [SerializeField] private float buttonAnimScale = 1.2f;
    [SerializeField] private float buttonAnimDuration = 1f;
    [SerializeField] private GameObject sceneTransition;

    private int shoppingScore;

    private SceneController sceneController;

    private void Start()
    {
        sceneController = SceneController.INSTANCE;

        shoppingScore = GameManager.INSTANCE.globalShoppingScore;

        foreach(GameObject i in shoppingStars)
        {
            i.SetActive(false);
        }
    }

    private void Update()
    {
        //shoppingScore = GameManager.INSTANCE.shoppingScore1;

        if (shoppingScore >= 150 && shoppingScore < 300)
        {
            shoppingStars[0].SetActive(true);
        }
        else if (shoppingScore >= 300 && shoppingScore < 450)
        {
            shoppingStars[0].SetActive(true);
            shoppingStars[1].SetActive(true);
        }
        else if (shoppingScore >= 450)
        {
            foreach (GameObject gameObject in shoppingStars)
            {
                gameObject.SetActive(true);
            }
        }
    }

    public void OnButtonPress(int sceneIndex)
    {
        // Play animation and start to load next scene
        LeanTween.scale(EventSystem.current.currentSelectedGameObject, new Vector3(buttonAnimScale, buttonAnimScale), buttonAnimDuration).setEasePunch();
        sceneController.LoadSceneAsync(sceneIndex);

        // Fade to black and change scenes after animation is done
        StartCoroutine(ActivateLoadedScene());
    }

    private IEnumerator ActivateLoadedScene()
    {
        sceneTransition.SetActive(true);

        yield return new WaitForSeconds(1.3f);

        sceneController.ActivateLoadedScene();
    }
}
