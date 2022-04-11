using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Public variables
    public GameObject niceParticleEffect;

    public AudioClip objectiveSFX, correctSFX, wrongSFX;

    public GameObject phoneTick, laptopTick, chargerTick;

    [HideInInspector] public int phonesCaught = 0, laptopsCaught = 0, chargersCaught = 0;

    // Private variables
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private TextMeshProUGUI phoneObjective, laptopObjective, chargerObjective;

    private ShoppingSceneManager manager;
    private AudioManager audioManager;
    private Rigidbody2D rb2d;

    private bool facingRight = true;

    Vector2 lastPos;
    Vector2 trackVelocity;

    private void Start()
    {
        manager = GameObject.Find("CurrentSceneManager").GetComponent<ShoppingSceneManager>();
        audioManager = AudioManager.INSTANCE;
        rb2d = GetComponent<Rigidbody2D>();

        phoneObjective.text = "0/3";
        laptopObjective.text = "0/3";
        chargerObjective.text = "0/3";
    }

    private void Update()
    {
        if (!manager.gameStarted || manager.gameEnded)
        {
            return;
        }

        trackVelocity = (rb2d.position - lastPos) * 50;
        lastPos = rb2d.position;

        //Debug.Log(Mathf.Abs(trackVelocity.x));

        if (manager.gameEnded == false)
        {
            //Time.timeScale = Mathf.Clamp(Mathf.Abs(trackVelocity.x), 0.25f, 1);
            Time.timeScale = Mathf.Clamp(Mathf.Abs(trackVelocity.x / 10), 0.3f, 1);
        }
        else
        {
            Time.timeScale = 1;
        }

        PlayerMovement();
        CheckWin();
    }

    private void PlayerMovement()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            point.z = Camera.main.nearClipPlane;

            Vector3 mouseToPlayerDir = point - transform.position;

            // Flip player if moving other direction
            if (mouseToPlayerDir.x < -0.01f && !facingRight)
            {
                facingRight = true;

                Vector3 playerScale = transform.localScale;
                playerScale.x = -1;
                transform.localScale = playerScale;
            }
            else if (mouseToPlayerDir.x > 0.01f && facingRight)
            {
                facingRight = false;

                Vector3 playerScale = transform.localScale;
                playerScale.x = 1;
                transform.localScale = playerScale;
            }

            transform.position = Vector3.MoveTowards(transform.position, new Vector3(point.x, transform.position.y, 10), 0.2f);
            // Move player
            //transform.position = Vector3.Lerp(transform.position, new Vector3(point.x, transform.position.y, 10), movementSpeed * Time.deltaTime);
            // Clamp player position
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -7f, 7f), transform.position.y, 10);
        }
    }

    public void CollectObject(ProductStats stats)
    {
        switch (stats.productID)
        {
            case 0: // Phone
                ++phonesCaught;
                phoneObjective.text = phonesCaught + "/3";
                StartCoroutine(BlinkColorGUI(phoneObjective, Color.green));
                manager.score += 500;
                CreateParticleEffect();

                if (phonesCaught == 3)
                {
                    phoneTick.SetActive(true);
                    audioManager.Play(objectiveSFX);
                }
                else
                {
                    audioManager.Play(correctSFX);
                }

                break;

            case 1: // Broken Phone
                if (manager.score != 0)
                {
                    manager.score -= 250;
                }
                StartCoroutine(BlinkColorGUI(manager.scoreText, Color.red));
                audioManager.Play(wrongSFX);
                break;

            case 2: // Laptop
                ++laptopsCaught;
                laptopObjective.text = laptopsCaught + "/3";
                StartCoroutine(BlinkColorGUI(laptopObjective, Color.green));
                manager.score += 500;
                CreateParticleEffect();

                if (laptopsCaught == 3)
                {
                    laptopTick.SetActive(true);
                    audioManager.Play(objectiveSFX);
                }
                else
                {
                    audioManager.Play(correctSFX);
                }

                break;

            case 3: //Broken Laptop
                if (manager.score != 0)
                {
                    manager.score -= 250;
                }
                StartCoroutine(BlinkColorGUI(manager.scoreText, Color.red));
                audioManager.Play(wrongSFX);
                break;

            case 4: // Charger
                ++chargersCaught;
                chargerObjective.text = chargersCaught + "/3";
                StartCoroutine(BlinkColorGUI(chargerObjective, Color.green));
                manager.score += 500;
                CreateParticleEffect();

                if (chargersCaught == 3)
                {
                    chargerTick.SetActive(true);
                    audioManager.Play(objectiveSFX);
                }
                else
                {
                    audioManager.Play(correctSFX);
                }

                break;

            case 5: // Broken Charger
                if (manager.score != 0)
                {
                    manager.score -= 250;
                }
                StartCoroutine(BlinkColorGUI(manager.scoreText, Color.red));
                audioManager.Play(wrongSFX);
                break;
        }
    }

    public void CreateParticleEffect()
    {
        GameObject effect = Instantiate(niceParticleEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1f);
    }

    private IEnumerator BlinkColorGUI(TextMeshProUGUI text, Color color)
    {
        Color originalColor = text.color;
        text.color = color;
        yield return new WaitForSecondsRealtime(0.6f);
        text.color = originalColor;
    }

    private void CheckWin()
    {
        if (phonesCaught >= 3 && laptopsCaught >= 3 && chargersCaught >= 3)
        {
            manager.WinGame();
            Time.timeScale = 1;
        }
    }
}
