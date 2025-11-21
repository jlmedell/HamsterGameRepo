using UnityEngine;
using TMPro;
using UnityEditor.Experimental.GraphView;

public class HamsterWheel : MonoBehaviour
{
    
    public TMP_Text exitPromptText;

    public float activationSpeed = 0.2f;  // how fast the player must move
    public Feeder feeder;                 // reference to feeder script
    public Collider2D wheelCollider;      // the circle collider on the wheel
    public Collider2D innerTrigger;       // trigger collider
    public string playerTag = "Player";

    [SerializeField]
    public PlayerController playerController;
    [SerializeField]
    Collider2D playerCollider;
    public Transform wheelSprite;
    public Rigidbody2D playerRb;         // reference to the player's Rigidbody2D
    public float rotationSpeed = 50f;   // multiplier for visual speed
    private bool wheelActive = false;
    void Start()
    {
        wheelCollider.enabled = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        // Player leaves inner trigger → turn off collider
        wheelCollider.enabled = false;
        wheelActive = false;
        feeder.Deactivate();
   }

    // void OnTriggerStay2D(Collider2D other)
    // {
    //     Rigidbody2D rb = other.attachedRigidbody;
    //     if (rb == null) return;

    //     // Check player horizontal speed
    //     float speed = Mathf.Abs(rb.linearVelocity.x);

    //     // if (speed > activationSpeed)
    //     // {
    //     //     feeder.Activate();
    //     // }
    //     // else
    //     // {
    //     //     feeder.Deactivate();
    //     // }
    // }

    void Update()
    {
        if (playerCollider == null) return;

        // Check if the player is fully inside the trigger
        if (innerTrigger.bounds.Contains(playerCollider.bounds.min) &&
            innerTrigger.bounds.Contains(playerCollider.bounds.max))
        {
            exitPromptText.text = "Press E to exit wheel";
            if (Input.GetKeyDown(KeyCode.E))
            {
                playerController.TeleportPlayer();
            }
            if (!wheelActive)
            {
                wheelCollider.enabled = true;
                wheelActive = true;
               
            }
        }
        else
        {
            exitPromptText.text = "";
        }

        if (playerRb == null) return;


        if (wheelActive)
        {
            // Rotate around Z axis based on horizontal velocity
            float horizontalSpeed = playerRb.linearVelocity.x;
            wheelSprite.Rotate(0f, 0f, -horizontalSpeed * rotationSpeed * Time.deltaTime);
         feeder.Activate();
        }


    }
}
