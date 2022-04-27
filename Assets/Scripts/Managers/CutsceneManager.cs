using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneManager : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] internal GameObject cutsceneObj;

    internal bool isPlaying = false;
    internal bool hasPlayed = false;

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
        isPlaying = false;
        hasPlayed = true;
    }

    public void SkipCutscene(int frame)
    {
        videoPlayer.frame = frame;
    }
}
