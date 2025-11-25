using UnityEngine;
using TMPro; // Only if using TextMeshPro
using UnityEngine.UI; // Use this instead if you're using legacy UI Text
using System.Collections;

public class PlayerInventory : MonoBehaviour
{
    public TMP_Text burrowPromptText;
    private Rigidbody2D rb;
    [Header("Food Settings")]
    public int foodInInventory = 0;
    public int foodInBurrows = 0;

    [Header("UI References")]
    public TextMeshProUGUI inventoryText;
    public TextMeshProUGUI burrowText;

    [Header("Burrow Settings")]
    public float burrowHoldTime = 0.25f;

    private float burrowTimer = 0f;
    private bool isHoldingBurrow = false;
    //Food object to spawn back
    public GameObject objectToSpawn; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        foodInInventory = 0;
        inventoryText.text = $"Food In Inventory: {foodInInventory}";
        UpdateUI();
    }

    void Update()
    {
        rb.gravityScale = 3 + (foodInInventory * 0.2f); //food in inventory affects jumping
        if (foodInInventory > 0)
        {
            burrowPromptText.text = "Hold B to burrow";
        }
        else
        {
            burrowPromptText.text = "";
        }
        // Detect when player holds the B key
        if (Input.GetKey(KeyCode.B))
        {
            burrowTimer += Time.deltaTime;

            if (!isHoldingBurrow && burrowTimer >= burrowHoldTime)
            {
                MakeBurrow();
                isHoldingBurrow = true;
                burrowTimer = 0f;
                isHoldingBurrow = false;
            }
        }
        else
        {
            burrowTimer = 0f;
            isHoldingBurrow = false;
        }
       // Debug.Log(burrowTimer);
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
            BurrowAction();
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
    // Spawn food object when burrowing
     void BurrowAction()
   {
      


      // Create a new Vector2 for the spawn position (for 2D)
      Vector2 spawnPosition = new(transform.position.x, transform.position.y - 1.1f);
      float randomZRotation = Random.Range(0f, 360f); // Random angle between 0 and 360 degrees
      Quaternion randomRotation = Quaternion.Euler(0, 0, randomZRotation); // X and Y are 0 for 2D
      GameObject spawnFood = Instantiate(objectToSpawn, spawnPosition, randomRotation, this.transform); // Spawn the object at the spawner's position
      spawnFood.transform.localScale = new Vector3(0.025f, 0.025f, 0.2f); //scale down food when spawned from burrow
      spawnFood.transform.parent = null;
   }
}
