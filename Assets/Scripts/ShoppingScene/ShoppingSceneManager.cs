using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ShoppingSceneManager : MonoBehaviour
{
    // Private variables
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject grayBG;
    [SerializeField] private Button buy1, buy2, buy3;
    [SerializeField] private GameObject chatIcon, chatPanel;
    [SerializeField] private Animator chatIconAnimator;

    // Start is called before the first frame update
    void Start()
    {
        InitGameObjects();
    }

    private void InitGameObjects()
    {
        // Play cutscene
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Cutscenes/JennieCutscene.mp4");
        videoPlayer.loopPointReached += CutsceneEnd;

        // Init gray background
        grayBG.SetActive(false);

        // Init buy buttons
        buy1.interactable = true;
        buy2.interactable = true;
        buy3.interactable = true;

        // Init chat
        chatPanel.transform.localScale = new Vector3(0, 0, 0);
        chatIcon.SetActive(false);
        chatPanel.SetActive(false);
    }

    private void CutsceneEnd(VideoPlayer videoPlayer)
    {
        RectTransform rectTransform = videoPlayer.GetComponent<RectTransform>();
        LeanTween.moveY(rectTransform, 72, 1f).setEaseInOutExpo();
        LeanTween.scale(rectTransform, new Vector3(1.57f, 1.57f), 1f).setEaseInOutExpo();
        Invoke("DeactivateCutscene", 1f);
    }

    private void DeactivateCutscene()
    {
        videoPlayer.gameObject.SetActive(false);
    }

    public void OnProductBuy(int productID)
    {
        // Set all buttons to uninteractible after buying
        buy1.interactable = false;
        buy2.interactable = false;
        buy3.interactable = false;

        // Activate chat icon
        chatIcon.SetActive(true);
        LeanTween.scale(chatIcon, new Vector3(1.2f, 1.2f, 1.2f), 0.8f).setEasePunch();

        // Activate gray background
        grayBG.SetActive(true);
    }

    public void OnChatOpen()
    {
        // Stop chat icon animation
        chatIconAnimator.SetTrigger("IconClicked");

        // Animate chat panel
        chatPanel.SetActive(true);
        LeanTween.scale(chatPanel, new Vector3(1, 1, 1), 1f).setEaseOutExpo();
    }
}
