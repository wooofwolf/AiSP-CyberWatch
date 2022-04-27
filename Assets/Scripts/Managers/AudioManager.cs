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
                go.name = "AudioManager";
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
        Debug.Log("Destroyed AudioManager");
    }
    #endregion

    // Audio players components.
    public AudioSource MusicSource;
    public List<AudioSource> EffectsSource;
    public AudioSource DialogueSource;

    // Components
    [SerializeField] private DialogueManager dialogueManager;

    // Global audio clips.
    public AudioClip buttonHover;

    void Start()
    {
        if (GameObject.Find("MusicSource") != null)
        {
            MusicSource = GameObject.Find("MusicSource").GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        // Dialogue
        if (!dialogueManager.isWriting)
        {
            DialogueSource.Stop();
        }

        // Effects
        for (int i = 0; i < EffectsSource.Count; ++i)
        {
            if (!EffectsSource[i].isPlaying)
            {
                Destroy(EffectsSource[i].gameObject);
                EffectsSource.Remove(EffectsSource[i]);
            }
        }
    }

    // Play a single clip through the sound effects source.
    public void Play(AudioClip clip)
    {
        // Create new gameobject
        GameObject soundEffect = new GameObject("Sound Effect");
        AudioSource source = soundEffect.AddComponent<AudioSource>();
        EffectsSource.Add(source);

        source.clip = clip;
        source.Play();
    }

    public void PlayDialogueClip(AudioClip clip)
    {
        if (dialogueManager.isWriting && !DialogueSource.isPlaying)
        {
            DialogueSource.clip = clip;
            DialogueSource.Play();
        }
    }

    // Play a single clip through the music source.
    public void PlayMusic(AudioClip clip)
    {
        MusicSource.clip = clip;
        MusicSource.Play();
    }

    public void StopMusic()
    {
        MusicSource.Stop();
    }

    private void OnMouseEnter()
    {
        // Create new gameobject
        GameObject soundEffect = new GameObject("Sound Effect");
        AudioSource source = soundEffect.AddComponent<AudioSource>();
        EffectsSource.Add(source);

        source.clip = buttonHover;
        source.Play();
    }
}
