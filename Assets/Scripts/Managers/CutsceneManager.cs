using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneManager : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] internal GameObject cutsceneObj;
    [SerializeField] internal GameObject skipButton;

    public bool isPlaying = false;
    public bool hasPlayed = false;

    public void StartCutscene(string url)
    {
        if (isPlaying)
        {
            return;
        }

        cutsceneObj.SetActive(true);
        videoPlayer.url = url;
        videoPlayer.loopPointReached += FinishCutscene;
        videoPlayer.Play();

        isPlaying = true;
    }

    private void FinishCutscene(VideoPlayer videoPlayer)
    {
        videoPlayer.Stop();
        isPlaying = false;
        hasPlayed = true;
    }

    public void SkipCutscene(int frame)
    {
        videoPlayer.frame = frame;
        skipButton.SetActive(false);
    }

    public void ResetCutscene()
    {
        videoPlayer.Stop();
        isPlaying = false;
        hasPlayed = false;
    }
}
