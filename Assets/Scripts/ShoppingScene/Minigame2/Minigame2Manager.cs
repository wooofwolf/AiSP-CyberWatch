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

    private bool star1Anim = false, star2Anim = false, star3Anim = false;

    private int qnNumber = 0;
    private int correct = 0;
    private int localScore = 0, score;

    private AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        gameObject.GetComponent<ProductRandomizer>().RandomizeProducts(productLists);

        InitGameObjects();

        score = GameManager.INSTANCE.currentShoppingScore;
    }

    private void FixedUpdate()
    {
        if (results.activeInHierarchy == true)
        {
            endScoreText.text = "Total Score: " + localScore;

            if (localScore != score)
            {
                localScore += 10;

                scoreSlider.value = Mathf.Lerp(600, localScore, 1.0f);
            }
            else
            {
                if (score >= 600)
                {
                    confettiParticle.SetActive(true);
                }
            }
        }
    }

    private void Update()
    {
        if (localScore >= 900)
        {
            if (star3Anim != true)
            {
                stars[2].SetActive(true);
                stars[2].LeanScale(new Vector3(1, 1, 1), 1.0f).setEaseOutExpo();
                star3Anim = true;
            }
        }
        else if (localScore >= 750)
        {
            if (star2Anim != true)
            {
                stars[1].SetActive(true);
                stars[1].LeanScale(new Vector3(1, 1, 1), 1.0f).setEaseOutExpo();
                star2Anim = true;
            }
        }
        else if (localScore >= 600)
        {
            if (star1Anim != true)
            {
                stars[0].SetActive(true);
                stars[0].LeanScale(new Vector3(1, 1, 1), 1.0f).setEaseOutExpo();
                star1Anim = true;
            }
        }

        scoreText.text = "Score: " + score;
    }

    private void InitGameObjects()
    {
        star1Anim = false;
        star2Anim = false;
        star3Anim = false;

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
        infographic.SetActive(false);

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
            audioManager.Play(wrongEffect);

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
            audioManager.Play(correctEffect);

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
                audioManager.Play(swooshEffect);
            }
        }

        ++qnNumber;

        yield return new WaitForSeconds(1f);

        canvasGroup.blocksRaycasts = true;
        Destroy(ui);
    }

    private void PlayCutscene()
    {
        audioManager.StopMusic();

        minigame.SetActive(false);
        resultsScreen.SetActive(true);
        endCutscene.gameObject.SetActive(true);

        if (score >= 600)
        {
            endCutscene.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Cutscenes/Jennie_Win.mp4");
        }
        else
        {
            endCutscene.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Cutscenes/Jennie_Lose.mp4");
        }
        StartCoroutine(PrepareCutscene(endCutscene));
        endCutscene.loopPointReached += OnCutsceneEnd;
    }

    private IEnumerator PrepareCutscene(VideoPlayer vp)
    {
        vp.Prepare();

        while (!vp.isPrepared)
        {
            yield return null;
        }

        vp.Play();
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
