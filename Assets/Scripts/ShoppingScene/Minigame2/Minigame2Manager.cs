using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;
using TMPro;

public class Minigame2Manager : MonoBehaviour
{
    // Public variables
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI endScoreText;

    public GameObject[] stars;

    // Private variables
    [SerializeField] private GameObject startingFade, sceneTransition;

    [SerializeField] private AudioClip correctEffect, wrongEffect;

    [SerializeField] private GameObject minigame, resultsScreen;
    [SerializeField] private VideoPlayer endCutscene;
    [SerializeField] private GameObject instructions1, instructions2, results;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject[] productLists;
    [SerializeField] private GameObject tick, cross;

    private int qnNumber = 0;
    private int correct = 0;

    private int score;

    private void Start()
    {
        score = GameManager.INSTANCE.currentShoppingScore;

        gameObject.GetComponent<ProductRandomizer>().RandomizeProducts(productLists);

        InitGameObjects();
    }

    private void Update()
    {
        scoreText.text = "Score: " + score;

        if (results.activeInHierarchy == true)
        {
            endScoreText.text = "Score: " + score;

            if (score >= 150 && score < 300)
            {
                stars[0].SetActive(true);
            }
            else if (score >= 300 && score < 450)
            {
                stars[0].SetActive(true);
                stars[1].SetActive(true);
            }
            else if (score >= 450)
            {
                foreach (GameObject gameObject in stars)
                {
                    gameObject.SetActive(true);
                }
            }
        }
    }

    private void InitGameObjects()
    {
        startingFade.SetActive(true);
        sceneTransition.SetActive(false);

        minigame.SetActive(true);
        resultsScreen.SetActive(false);
        endCutscene.gameObject.SetActive(false);
        canvasGroup.blocksRaycasts = false;
        instructions1.gameObject.SetActive(true);
        instructions2.gameObject.SetActive(false);
        results.SetActive(false);

        foreach (GameObject gameObject in stars)
        {
            gameObject.SetActive(false);
        }
    }

    public void NextInstruction()
    {
        instructions1.SetActive(false);
        instructions2.SetActive(true);
    }

    public void PrevInstruction()
    {
        instructions1.SetActive(true);
        instructions2.SetActive(false);
    }

    public void StartGame()
    {
        instructions2.gameObject.SetActive(false);
        canvasGroup.blocksRaycasts = true;
    }

    public void BuyProduct()
    {
        StartCoroutine(DoBuyProduct());
    }

    private IEnumerator DoBuyProduct()
    {
        canvasGroup.blocksRaycasts = false;

        GameObject buttonPressed = EventSystem.current.currentSelectedGameObject;
        GameObject ui = null;

        if (buttonPressed.transform.parent.CompareTag("ScamPurchase"))
        {
            ui = Instantiate(cross, buttonPressed.transform.parent.transform);
            AudioManager.INSTANCE.Play(wrongEffect);

            score -= 100;
        }
        else
        {
            ++correct;

            ui = Instantiate(tick, buttonPressed.transform.parent.transform);
            AudioManager.INSTANCE.Play(correctEffect);

            score += 150;
        }

        yield return new WaitForSeconds(1f);

        // Bring to next stage
        if (qnNumber == 2)
        {
            PlayCutscene();
        }

        else
        {
            for (int i = 0; i < productLists.Length; ++i)
            {
                LeanTween.moveLocalY(productLists[i], productLists[i].transform.localPosition.y + 800f, 1f).setEaseInOutBack();
            }
        }
        ++qnNumber;

        yield return new WaitForSeconds(1f);

        canvasGroup.blocksRaycasts = true;
        Destroy(ui);
    }

    private void PlayCutscene()
    {
        AudioManager.INSTANCE.StopMusic();

        minigame.SetActive(false);
        resultsScreen.SetActive(true);
        endCutscene.gameObject.SetActive(true);

        if (correct == 3)
        {
            endCutscene.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Cutscenes/Jennie_Win.mp4");
        }
        else
        {
            endCutscene.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Cutscenes/Jennie_Win.mp4");
        }

        endCutscene.Play();
        endCutscene.loopPointReached += OnCutsceneEnd;
    }

    private void OnCutsceneEnd(VideoPlayer videoPlayer)
    {
        endCutscene.gameObject.SetActive(false);
        results.SetActive(true);

        if (score > GameManager.INSTANCE.globalShoppingScore)
        {
            GameManager.INSTANCE.globalShoppingScore = score;
        }
    }

    public void BackToMainMenu()
    {
        SceneController.INSTANCE.LoadScene(1);
    }

    public void RestartLevel()
    {
        SceneController.INSTANCE.Retry();
    }
}
