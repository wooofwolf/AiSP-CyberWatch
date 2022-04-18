using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;
using TMPro;
using UnityEngine.UI;

public class Minigame2Manager : MonoBehaviour
{
    // Public variables
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI endScoreText;

    public GameObject[] stars;

    public GameObject confettiParticle;

    public GameObject infographic;

    public Slider scoreSlider;

    // Private variables
    [SerializeField] private GameObject startingFade, sceneTransition;

    [SerializeField] private AudioClip correctEffect, wrongEffect, swooshEffect;

    [SerializeField] private GameObject minigame, resultsScreen, scoreUI;
    [SerializeField] private VideoPlayer endCutscene;
    [SerializeField] private GameObject instructions1, instructions2, results;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject[] productLists;
    [SerializeField] private GameObject tick, cross;

    private int qnNumber = 0;
    private int correct = 0;
    private int localScore = 0, score;

    private void Start()
    {
        infographic.SetActive(false);

        gameObject.GetComponent<ProductRandomizer>().RandomizeProducts(productLists);

        InitGameObjects();

        //GameManager.INSTANCE.currentShoppingScore = 500;

        score = GameManager.INSTANCE.currentShoppingScore;
    }

    private void FixedUpdate()
    {
        if (results.activeInHierarchy == true)
        {
            endScoreText.text = "Total Score: " + localScore;

            if (localScore != GameManager.INSTANCE.globalShoppingScore)
            {
                localScore += 10;

                scoreSlider.value = Mathf.Lerp(0, localScore, 1.0f);
            }
            else
            {
                confettiParticle.SetActive(true);
            }
        }
    }

    private void Update()
    {
        if (localScore >= 900)
        {
            stars[2].SetActive(true);
            LeanTween.scale(stars[2], new Vector3(1,1,1), 1.0f).setEaseOutExpo();
        }
        else if (localScore >= 750)
        {
            stars[1].SetActive(true);
            LeanTween.scale(stars[1], new Vector3(1,1,1), 1.0f).setEaseOutExpo();
        }
        else if (localScore >= 600)
        {
            stars[0].SetActive(true);
            LeanTween.scale(stars[0], new Vector3(1,1,1), 1.0f).setEaseOutExpo();
        }

        scoreText.text = "Score: " + score;
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
        scoreUI.SetActive(false);
        confettiParticle.SetActive(false);

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
        scoreUI.SetActive(true);
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

            if (score <= 50)
            {
                score = 0;
            }
            else
            {
                score -= 100;
            }
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
                AudioManager.INSTANCE.Play(swooshEffect);
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
            endCutscene.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Cutscenes/Jennie_Lose.mp4");
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

    public void ShowInfoGraphic()
    {
        infographic.SetActive(true);
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
