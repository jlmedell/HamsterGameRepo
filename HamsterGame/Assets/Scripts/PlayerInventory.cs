using UnityEngine;
using TMPro; // Only if using TextMeshPro
using UnityEngine.UI; // Use this instead if you’re using legacy UI Text

public class PlayerInventory : MonoBehaviour
{
    [Header("Food Settings")]
    public int foodInInventory = 3;
    public int foodInBurrows = 0;

    [Header("UI References")]
    public TextMeshProUGUI inventoryText;
    public TextMeshProUGUI burrowText;

    [Header("Burrow Settings")]
    public float burrowHoldTime = 2f;

    private float burrowTimer = 0f;
    private bool isHoldingBurrow = false;

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        // Detect when player holds the B key
        if (Input.GetKey(KeyCode.B))
        {
            burrowTimer += Time.deltaTime;

            if (!isHoldingBurrow && burrowTimer >= burrowHoldTime)
            {
                MakeBurrow();
                isHoldingBurrow = true;
            }
        }
        else
        {
            burrowTimer = 0f;
            isHoldingBurrow = false;
        }
    }

    void MakeBurrow()
    {
        if (foodInInventory > 0)
        {
            foodInBurrows += 1;
            foodInInventory -= 1;
            UpdateUI();
            Debug.Log("Burrow made! Food moved to burrows.");
            // (Play burrow animation here later)
        }
        else
        {
            Debug.Log("No food in inventory to store!");
        }
    }

    public void UpdateUI()
    {
        if (inventoryText != null)
            inventoryText.text = $"Food In Inventory: {foodInInventory}";
        if (burrowText != null)
            burrowText.text = $"Food In Burrows: {foodInBurrows}";
    }
}
