using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region SingletonCreation
    // Single Instance creation
    private static GameManager _INSTANCE = null;

    // Allow calling of singleton to other
    public static GameManager INSTANCE
    {
        get
        {
            _INSTANCE = FindObjectOfType<GameManager>();
            if (_INSTANCE == null)
            {
                GameObject go = new GameObject();
                _INSTANCE = go.AddComponent<GameManager>();
                go.name = "GameManager";
                DontDestroyOnLoad(go);
            }
            return _INSTANCE;
        }
    }

    // Sets up singleton
    void Awake()
    {
        if (_INSTANCE == null)
        {
            _INSTANCE = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Properly destroy singleton when quitting
    private void OnDestroy()
    {
        Destroy(gameObject);
        _INSTANCE = null;
        Debug.Log("Destroyed GameManager");
    }
    #endregion

    // FPS Target
    public int FPSTarget = 144;
    public int numbers = 0;

    public int shop_minigame_1_score;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        LeanTween.init();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            numbers++;
        }

        /*
        // If current FPS does not match target, try to get it to be the same.
        if (FPSTarget != Application.targetFrameRate)
        {
            Application.targetFrameRate = FPSTarget;
        }
        */
    }
}
