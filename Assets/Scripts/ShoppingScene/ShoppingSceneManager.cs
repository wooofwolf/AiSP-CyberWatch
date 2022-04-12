using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ShoppingSceneManager : MonoBehaviour
{
    // Public variables
    public TextMeshProUGUI scoreText;

    public AudioClip music;

    public TextMeshProUGUI endScoreText;

    public GameObject infographic;

    [HideInInspector] public bool gameStarted = false;
    [HideInInspector] public bool gameEnded = false;

    [HideInInspector] public int score = 0;

    // Private variables
    [SerializeField] private GameObject startingFade, sceneTransition;
    [SerializeField] private GameObject skipButton;
    [SerializeField] private GameObject objectivesObj;
    [SerializeField] private GameObject endPanel;

    [SerializeField] private GameObject[] instructionsObj; // Array to control instructions

    [SerializeField] private VideoPlayer cutscene, instructonsVideo;

    [SerializeField] private Sprite[] objectSprites;

    [SerializeField] private float ySpawn;

    private int curPage = -1; // Int to control the current page of the instructions
    private int counter; // Used to count the score up in the end screen.

    private ObjectPool objectPool;
    private Gacha gacha;

    // Audio Manager
    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        infographic.SetActive(false);
        GameManager.INSTANCE.currentShoppingScore = 0;
        counter = 0;
        objectPool = GameObject.Find("CurrentSceneManager").GetComponent<ObjectPool>();
        gacha = GameObject.Find("CurrentSceneManager").GetComponent<Gacha>();
        audioManager = AudioManager.INSTANCE;

        InitGameObjects();
    }

    private void Update()
    {
        if (cutscene.url.Contains("Jennie_Start.mp4") && cutscene.frame == 650L)
        {
            // Hide skip
            skipButton.SetActive(false);
        }

        UpdateInstructionVideo();
        UpdateInstructions();

        if (endPanel.activeInHierarchy)
        {
            if (counter != score)
            {
                counter += 1;
                endScoreText.text = "Score: " + counter;
            }
        }
    }

    private void FixedUpdate()
    {
        scoreText.text = "Score: " + score;
    }

    private void InitGameObjects()
    {
        // Init transitions
        startingFade.SetActive(true);
        sceneTransition.SetActive(false);

        // Init cutscene
        cutscene.gameObject.SetActive(true);
        cutscene.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Cutscenes/Jennie_Start.mp4");
        cutscene.loopPointReached += OnStartCutsceneEnd;
        skipButton.SetActive(true);

        // Init instructions
        if (curPage == -1)
        {
            instructonsVideo.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Instructions/Instructions_pt1pg1.mp4");
        }

        // Init GUI
        objectivesObj.SetActive(false);
        endPanel.SetActive(false);
    }

    public void SkipStartCutscene()
    {
        cutscene.frame = 650L;
    }

    private void OnStartCutsceneEnd(VideoPlayer videoPlayer)
    {
        StartCoroutine(TransitionToGame());
    }

    private IEnumerator TransitionToGame()
    {
        sceneTransition.SetActive(true);

        yield return new WaitForSeconds(1.3f);

        curPage += 1;
        sceneTransition.SetActive(false);
        startingFade.SetActive(true);
        cutscene.gameObject.SetActive(false);
        audioManager.PlayMusic(music);
    }

    // Updates if instructions is active to play or to stop only before gameplay
    private void UpdateInstructionVideo()
    {
        if (curPage != 2 && curPage != -1)
        {
            if (instructionsObj[0].gameObject.activeSelf)
            {
                instructonsVideo.Play();
            }
            else
            {
                instructonsVideo.Stop();
            }
        }
    }

    // Updates instructions array to see which page is currently being shown
    private void UpdateInstructions()
    {
        if (curPage != -1 && curPage != 3)
        {
            instructionsObj[curPage].SetActive(true);

            for (int i = 0; i < instructionsObj.Length; i++)
            {
                if (i != curPage)
                {
                    instructionsObj[i].SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < instructionsObj.Length; i++)
            {
                instructionsObj[i].SetActive(false);
            }
        }
    }

    public void NextInstruction()
    {
        cutscene.frame = 0;
        instructonsVideo.Stop();

        curPage += 1;
    }

    public void PreviousInstruction()
    {
        cutscene.frame = 0;
        curPage -= 1;

        //instructonsVideo.isLooping = true;
        //instructonsVideo.Play();
        //cutscene.loopPointReached += OnStartCutsceneEnd;
    }

    public void StartGame()
    {
        for (int i = 0; i < instructionsObj.Length; i++)
        {
            instructionsObj[i].SetActive(false);
        }

        curPage += 1;

        objectivesObj.SetActive(true);
        gameStarted = true;

        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (gameStarted && !gameEnded)
        {
            // Spawn products
            GameObject newObject = objectPool.GetPooledObject();
            if (newObject != null)
            {
                int productID = gacha.GetProductID();
                if (productID != -1)
                {
                    ProductStats stats = newObject.GetComponent<ProductStats>();
                    stats.SetStats(productID);
                    stats.RandomSpeed();

                    newObject.GetComponent<SpriteRenderer>().sprite = objectSprites[productID];

                    newObject.transform.position = new Vector3(Random.Range(-7f, 7f), ySpawn, 0);
                    newObject.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-90f, 90f));
                    newObject.SetActive(true);
                }
            }

            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        }
    }

    public int GetNumberOfObjects()
    {
        return objectSprites.Length;
    }

    public void WinGame()
    {
        gameEnded = true;
        objectivesObj.SetActive(false);

        //audioManager.StopMusic();

        objectPool.SetAllToInactive();

        GameManager.INSTANCE.currentShoppingScore = score;

        endPanel.SetActive(true);
    }

    public void ShowInfoGraphic()
    {
        infographic.SetActive(true);
    }

    public void LoseGame()
    {
        gameEnded = true;
        objectivesObj.SetActive(false);
    }

    public void NextMinigame()
    {
        sceneTransition.SetActive(true);
        SceneController.INSTANCE.LoadSceneAsync(3);
    }

    public void RestartMinigame()
    {
        SceneController.INSTANCE.Retry();
    }
}
