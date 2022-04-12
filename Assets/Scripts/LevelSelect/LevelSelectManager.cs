using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    // Public variables
    public GameObject ShoppingMedal_1, ShoppingMedal_2;

    // Private variables
    [SerializeField] private float buttonAnimScale = 1.2f;
    [SerializeField] private float buttonAnimDuration = 1f;
    [SerializeField] private GameObject sceneTransition;

    private int shoppingScore1, shoppingScore2;

    private SceneController sceneController;

    private void Start()
    {
        sceneController = SceneController.INSTANCE;

        shoppingScore1 = GameManager.INSTANCE.shoppingScore1;
    }

    private void Update()
    {
        shoppingScore1 = GameManager.INSTANCE.shoppingScore1;

        if (shoppingScore1 >= 1250 && shoppingScore1 < 2500)
        {
            ShoppingMedal_1.GetComponent<Image>().color = new Color(255, 146, 90, 255);
        }
        else if (shoppingScore1 >= 2500 && shoppingScore1 < 3500)
        {
            ShoppingMedal_1.GetComponent<Image>().color = new Color(192, 192, 192, 255);
        }
        else if (shoppingScore1 >= 3500)
        {
            ShoppingMedal_1.GetComponent<Image>().color = new Color(255, 215, 0, 255);
        }
        else
        {
            ShoppingMedal_1.GetComponent<Image>().color = new Color(255, 255, 255, 0);
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
