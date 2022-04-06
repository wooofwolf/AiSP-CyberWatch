using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ShoppingSceneManager : MonoBehaviour
{
    // Public variables
    [HideInInspector] public bool gameStarted = false;
    [HideInInspector] public bool gameEnded = false;

    // Private variables
    [SerializeField] private GameObject instructionsObj, instructions2;
    [SerializeField] private VideoPlayer cutscene, instructions1;
    [SerializeField] private Sprite[] objectSprites;
    [SerializeField] private float ySpawn;
    [SerializeField] private GameObject objectivesObj;

    private ObjectPool objectPool;
    private Gacha gacha;

    // Start is called before the first frame update
    void Start()
    {
        objectPool = FindObjectOfType<ObjectPool>();
        gacha = FindObjectOfType<Gacha>();

        InitGameObjects();
    }

    private void InitGameObjects()
    {
        // Init cutscene
        cutscene.gameObject.SetActive(true);
        cutscene.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Cutscenes/Jennie_Start.mp4");
        cutscene.loopPointReached += OnStartCutsceneEnd;

        // Init instructions
        instructionsObj.SetActive(false);
        instructions1.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Instructions/Shopping_Instructions.mp4");
        instructions2.SetActive(false);

        // Init GUI
        objectivesObj.SetActive(false);
    }

    public void SkipStartCutscene()
    {
        cutscene.frame = (long)cutscene.frameCount;
    }

    private void OnStartCutsceneEnd(VideoPlayer videoPlayer)
    {
        cutscene.Stop();
        cutscene.gameObject.SetActive(false);
        instructionsObj.SetActive(true);
        instructions1.Play();
    }

    public void NextInstruction()
    {
        instructions1.Stop();
        cutscene.loopPointReached -= OnStartCutsceneEnd;

        instructions1.gameObject.SetActive(false);
        instructions2.SetActive(true);
    }

    public void StartGame()
    {
        instructions2.SetActive(false);
        instructionsObj.gameObject.SetActive(false);

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
                ProductStats stats = newObject.GetComponent<ProductStats>();
                stats.SetStats(productID);

                newObject.GetComponent<SpriteRenderer>().sprite = objectSprites[productID];

                newObject.transform.position = new Vector3(Random.Range(-7f, 7f), ySpawn, 0);
                newObject.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-90f, 90f));
                newObject.SetActive(true);
            }

            yield return new WaitForSeconds(Random.Range(1f, 3f));
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

        cutscene.gameObject.SetActive(true);
        cutscene.Prepare();
        cutscene.targetTexture.Release();
        cutscene.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Cutscenes/Jennie_Win.mp4");
        cutscene.Play();
    }

    public void LoseGame()
    {
        gameEnded = true;
        objectivesObj.SetActive(false);
    }
}
