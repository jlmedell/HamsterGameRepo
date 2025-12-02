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

    public PlayerController player;
    public MunchFoodScript munchFoodScript;
    private float collectionLength = 90f;
    public Slider burrowSlider;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        foodInInventory = 0;
        inventoryText.text = $"Food In Inventory: {foodInInventory}";
        UpdateUI();
        burrowSlider.gameObject.SetActive(false);
    }

    void Update()
    {
        rb.gravityScale = 3 + (foodInInventory * 0.2f); // food in inventory affects jumping
        // Detect when player holds the B key
        if (Input.GetKey(KeyCode.B) && player.moveInput == 0 && foodInInventory > 0)
        {
            munchFoodScript.playBurrowSound();
            burrowTimer += Time.deltaTime;
            burrowSlider.gameObject.SetActive(true);
            burrowSlider.value += collectionLength * Time.deltaTime;
                if (!isHoldingBurrow && burrowSlider.value == burrowSlider.maxValue)
                {
                    MakeBurrow();
                    isHoldingBurrow = true;
                    burrowTimer = 0f;
                    isHoldingBurrow = false;
                    burrowSlider.value = 0f;
                }
        }
        else
        {
            burrowTimer = 0f;
            isHoldingBurrow = false;
            if (Input.GetKeyUp(KeyCode.E) || (!player.inRangeBowl && !player.inRangeBurrow))
            {
            burrowSlider.gameObject.SetActive(false);
            burrowSlider.value = 0f;
            }
        }
        if(player.collectibles.Length > 0 && Input.GetKey(KeyCode.E))
        {
            if(player.inRangeBowl || (player.inRangeBurrow && foodInInventory > 0))
            {
                munchFoodScript.playMunchSound();
                Debug.Log("playing munch");
            }
            else munchFoodScript.stopSound();
        }
    }

    void MakeBurrow()
    {
        if (foodInInventory > 0)
        {
            foodInBurrows += 1;
            foodInInventory -= 1;
            UpdateUI();
            // (Play burrow animation here later)
            BurrowAction();
        }
    }

    public void UpdateUI()
    {
        if (inventoryText != null)
            inventoryText.text = $"Food In Inventory: {foodInInventory}";
        if (burrowText != null)
            burrowText.text = $"Food In Burrows: {foodInBurrows}";
        if(foodInInventory <= 0)
        {
            burrowPromptText.text = "No food to burrow!";
        }
        else if (burrowTimer >= burrowHoldTime || burrowTimer == 0)
        {
            burrowPromptText.text = "Hold B to burrow";
        }
        else
        {
            burrowPromptText.text = "Burrowing...";
        }
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
