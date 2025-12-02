using UnityEngine;

public class HamsterBall : MonoBehaviour
{
    [Header("References")]
    public Collider2D entryTrigger;
    public string playerTag = "Player";

    [Header("Layers")]
    public string playerLayerName = "Player";
    public string ballLayerName = "HamsterBall";
    public SpriteRenderer playerSprite;
    public int playerInFrontOrder = 1;   // player appears in front of ball
    public int playerInsideOrder = -1;   // player appears behind ball

    public bool playerInside = false;
    private bool canEnter = true;
    private Transform player;

    void Start()
    {
        // Disable collisions initially
        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer(playerLayerName),
            LayerMask.NameToLayer(ballLayerName),
            true
        );
        if (playerSprite != null)
            playerSprite.sortingOrder = playerInFrontOrder;
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            ExitBall();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Trigger Enter: {other.name}"); // <-- DEBUG

        if (!canEnter) return;
        if (!other.CompareTag(playerTag)) return;

        // Only react when entering through the entry trigger
        if (other.IsTouching(entryTrigger))
        {
            Debug.Log("Player entered hamster ball!"); // <-- DEBUG
            EnterBall(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"Trigger Exit: {other.name}"); // <-- DEBUG

        // Lock entry until player fully leaves trigger area
        if (other.CompareTag(playerTag))
        {
            canEnter = true;
            Debug.Log("Player exited trigger, canEnter = true"); // <-- DEBUG
        }
    }

    private void EnterBall(Transform target)
    {
        canEnter = false;
        playerInside = true;
        player = target;

        // Enable collision with ball
        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer(playerLayerName),
            LayerMask.NameToLayer(ballLayerName),
            false
        );

        // Parent player to ball
        player.SetParent(transform);

        // Set player behind the ball
        if (playerSprite != null)
            playerSprite.sortingOrder = playerInsideOrder;
    }

    private void ExitBall()
    {
        playerInside = false;

        // Disable collision with ball
        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer(playerLayerName),
            LayerMask.NameToLayer(ballLayerName),
            true
        );

        // Unparent player
        player.SetParent(null);

        // Set player in front of ball
        if (playerSprite != null)
            playerSprite.sortingOrder = playerInFrontOrder;

        player = null;
    }

}
