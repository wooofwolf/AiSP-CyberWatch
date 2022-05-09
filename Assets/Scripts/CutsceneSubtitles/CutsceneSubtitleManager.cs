using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneSubtitleManager : MonoBehaviour
{
    // Cutscenes are 24 FPS

    [Header("Attributes")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] internal GameObject cutsceneObj;
    [SerializeField] internal GameObject skipButton;
    [SerializeField] internal TextMeshProUGUI captions;

    public bool isPlaying = false;
    public bool hasPlayed = false;

    // frame, caption
    private Dictionary<int[], string> captionsDictionary = new Dictionary<int[], string>();

    private void Update()
    {
        // frames might skip
        long frame = videoPlayer.frame;

        for (int i = captionsDictionary.Count - 1; i >= 0; --i)
        {
            int[] keys = captionsDictionary.Keys.ElementAt(i);

            if (frame >= keys[0] && frame <= keys[1])
            {
                captions.text = captionsDictionary.Values.ElementAt(i);
                break;
            }
            else
            {
                captions.text = "";
            }
        }
    }

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

    public void InitSubtitles(string filePath)
    {
        TextAsset captionsFile = Resources.Load<TextAsset>(filePath);
        string captionsStr = captionsFile.text;

        string[] strArray = Regex.Split(captionsStr, "\n|\r|\r\n").Where(s => s != string.Empty).ToArray();

        for (int i = 0; i < strArray.Length; i += 2)
        {
            string[] timestamps = Regex.Split(strArray[i], " - ");

            // Start timestamp
            string[] startTimestamp = Regex.Split(timestamps[0], ":");
            int start = (int.Parse(startTimestamp[0]) * 24) + int.Parse(startTimestamp[1]);

            // End timestamp
            string[] endTimestamp = Regex.Split(timestamps[1], ":");
            int end = (int.Parse(endTimestamp[0]) * 24) + int.Parse(endTimestamp[1]);

            int[] timestampsInt = { start, end };

            captionsDictionary.Add(timestampsInt, strArray[i + 1]);
        }
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
