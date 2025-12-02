using UnityEngine;
using TMPro;

public class HamsterWheel : MonoBehaviour
{

    public TMP_Text exitPromptText;

    public float activationSpeed = 0.2f;  // how fast the player must move
    public Feeder feeder;                 // reference to feeder script
    public Collider2D wheelCollider;      // the circle collider on the wheel
    public Collider2D entryTrigger;       // trigger collider
    public string playerTag = "Player";

    [SerializeField]
    public PlayerController playerController;
    [SerializeField]
    Collider2D playerCollider;
    public Transform wheelSprite;
    public SpriteRenderer wheelSpriteRenderer;
    public int defaultSortingOrder = -5;   // outside wheel
    public int insideSortingOrder = 1;    // when player is inside
    public Rigidbody2D playerRb;         // reference to the player's Rigidbody2D
    public float rotationSpeed = 50f;   // multiplier for visual speed
    private bool wheelActive = false;
    void Start()
    {
        wheelCollider.enabled = false;
    }
    private void EnterWheel()
    {
        wheelCollider.enabled = true;
        wheelActive = true;
        feeder.Activate();
        if (wheelSpriteRenderer != null)
        {
            wheelSpriteRenderer.sortingOrder = insideSortingOrder; // in front of player
        }
    }

    private void ExitWheel()
    {
        wheelActive = false;
        wheelCollider.enabled = false;
        feeder.Deactivate();

        if (wheelSpriteRenderer != null)
        {
            wheelSpriteRenderer.sortingOrder = defaultSortingOrder; // behind player
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (other == playerCollider)
        {
            if (other.IsTouching(entryTrigger))
            {
                EnterWheel();
            }
        }
    }
    void Update()
    {
        if (playerCollider == null) return;
        if (playerRb == null) return;

        if (Input.GetButtonDown("Jump"))
        {
            ExitWheel();
        }

        if (wheelActive)
        {
            exitPromptText.text = "Jump to exit wheel";
            float horizontalSpeed = playerRb.linearVelocity.x;
            wheelSprite.Rotate(0f, 0f, -horizontalSpeed * rotationSpeed * Time.deltaTime);
        }
        else
        {
            exitPromptText.text = "";
        }
    }

}