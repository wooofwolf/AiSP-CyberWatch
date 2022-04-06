using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Public variables
    [HideInInspector] public int phonesCaught = 0, laptopsCaught = 0, chargersCaught = 0;

    // Private variables
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private TextMeshProUGUI phoneObjective, laptopObjective, chargerObjective;

    private ShoppingSceneManager sceneManager;

    private void Start()
    {
        sceneManager = FindObjectOfType<ShoppingSceneManager>();

        phoneObjective.text = "Phones (0/3)";
        laptopObjective.text = "Laptops (0/3)";
        chargerObjective.text = "Chargers (0/3)";
    }

    private void Update()
    {
        if (!sceneManager.gameStarted || sceneManager.gameEnded)
        {
            return;
        }

        PlayerMovement();
        CheckWin();
    }

    private void PlayerMovement()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Flip player if moving other direction
            if (transform.position.x < point.x)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f));
            }
            else if (transform.position.x > point.x)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0f, 180f));
            }

            // Move player
            transform.position = Vector3.Lerp(transform.position, new Vector3(point.x, transform.position.y, 10), movementSpeed * Time.deltaTime);
            // Clamp player position
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -7f, 7f), transform.position.y, 10);
        }
    }

    public void CollectObject(ProductStats stats)
    {
        switch (stats.productID)
        {
            case 0:     // Phone
                ++phonesCaught;
                phoneObjective.text = "Phones (" + phonesCaught + "/3)";
                StartCoroutine(BlinkColorGUI(phoneObjective, Color.green));
                break;
            case 2:     // Laptop
                ++laptopsCaught;
                laptopObjective.text = "Laptops (" + laptopsCaught + "/3)";
                StartCoroutine(BlinkColorGUI(laptopObjective, Color.green));
                break;
            case 4:     // Charger
                ++chargersCaught;
                chargerObjective.text = "Chargers (" + chargersCaught + "/3)";
                StartCoroutine(BlinkColorGUI(chargerObjective, Color.green));
                break;
        }
    }

    private IEnumerator BlinkColorGUI(TextMeshProUGUI text, Color color)
    {
        Color originalColor = text.color;
        text.color = color;
        yield return new WaitForSeconds(0.6f);
        text.color = originalColor;
    }

    private void CheckWin()
    {
        if (phonesCaught >= 3 && laptopsCaught >= 3 && chargersCaught >= 3)
        {
            sceneManager.WinGame();
        }
    }
}
