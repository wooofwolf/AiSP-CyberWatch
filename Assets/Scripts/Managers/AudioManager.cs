using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region SingletonCreation
    // Single Instance creation
    private static AudioManager _INSTANCE = null; 

    // Allow calling of singleton to other
    public static AudioManager INSTANCE 
    {
        get
        {
            _INSTANCE = FindObjectOfType<AudioManager>();
            if (_INSTANCE == null)
            {
                GameObject go = new GameObject();
                _INSTANCE = go.AddComponent<AudioManager>();
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

        Debug.Log("AudioManager has been created.");
    }
    #endregion

    // Add code below
}
