using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gacha : MonoBehaviour
{
    // Private variables
    [SerializeField] private int startingWeight = 100;

    private ShoppingSceneManager manager;
    private PlayerController player;
    private ObjectPool objectPool;
    private int singularWeight, properWeight;
    private int amountVariables;
    private int randomNumber;
    private bool phoneRemoved = false, laptopRemoved = false, chargerRemoved = false;
    private List<int> table;

    private void Start()
    {
        manager = FindObjectOfType<ShoppingSceneManager>();
        player = FindObjectOfType<PlayerController>();
        objectPool = FindObjectOfType<ObjectPool>();
        amountVariables = manager.GetNumberOfObjects();

        singularWeight = startingWeight / amountVariables;
        table = new List<int>();

        for (int i = 0; i < amountVariables; i++)
        {
            table.Add(singularWeight);
            properWeight += singularWeight;
        }
    }

    private void Update()
    {
        // Recheck amount here
        RecheckWeights();
        UpdateWeights();
    }

    public int GetProductID()
    {
        randomNumber = Random.Range(0, properWeight);
        for (int i = 0; i < table.Count; ++i)
        {
            if (randomNumber <= table[i])
            {
                return i;
            }
            else
            {
                randomNumber -= table[i];
            }
        }

        return -1;
    }

    public void RecheckWeights()
    {
        if (amountVariables == 0)
        {
            return;
        }

        properWeight = 0;
        singularWeight = startingWeight / amountVariables;
        for (int i = 0; i < table.Count; i++)
        {
            if (table[i] == 0)
            {
                continue;
            }
            else
            {
                table[i] = singularWeight;
                properWeight += singularWeight;
            }
        }
    }

    public void SetCurrentToZero(int curElement)
    {
        for (int i = 0; i < table.Count; i++)
        {
            if (i == curElement)
            {
                amountVariables -= 1;
                table[i] = 0;
            }
        }
    }

    public void SetCurrentToOne(int curElement)
    {
        for (int i = 0; i < table.Count; i++)
        {
            if (i == curElement)
            {
                amountVariables += 1;
                table[i] = 1;
            }
        }
    }

    public void UpdateWeights()
    {
        if (player.phonesCaught + objectPool.GetObjectsOnScreen(0) >= 3 && !phoneRemoved)
        {
            SetCurrentToZero(0);
            SetCurrentToZero(1);

            phoneRemoved = true;
        }
        else if (player.phonesCaught + objectPool.GetObjectsOnScreen(0) < 3 && phoneRemoved)
        {
            SetCurrentToOne(0);
            SetCurrentToOne(1);

            phoneRemoved = false;
        }

        if (player.laptopsCaught + objectPool.GetObjectsOnScreen(2) >= 3 && !laptopRemoved)
        {
            SetCurrentToZero(2);
            SetCurrentToZero(3);

            laptopRemoved = true;
        }
        else if (player.laptopsCaught + objectPool.GetObjectsOnScreen(2) < 3 && laptopRemoved)
        {
            SetCurrentToOne(2);
            SetCurrentToOne(3);

            laptopRemoved = false;
        }

        if (player.chargersCaught + objectPool.GetObjectsOnScreen(4) >= 3 && !chargerRemoved)
        {
            SetCurrentToZero(4);
            SetCurrentToZero(5);

            chargerRemoved = true;
        }
        else if (player.chargersCaught + objectPool.GetObjectsOnScreen(4) < 3 && chargerRemoved)
        {
            SetCurrentToOne(4);
            SetCurrentToOne(5);

            chargerRemoved = false;
        }
    }
}
