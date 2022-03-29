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

        Debug.Log("GameManager has been created.");
    }
    #endregion

    // FPS Target
    public int FPStarget = 144;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
    }

    private void Update()
    {   
        // If current FPS does not match target, try to get it to be the same.
        if (FPStarget != Application.targetFrameRate)
        {
            Application.targetFrameRate = FPStarget;
        }
    }
}
