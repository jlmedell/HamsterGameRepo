using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 30f;
    public float jumpForce = 0.5f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;

    private float timer;
    private float collectionLength = 50f;
    public Slider collectionSlider; // Reference to UI Slider for collecting
   private bool inRange = false;
   



   void Start()
    {
        rb = GetComponent<Rigidbody2D>();
      timer = 0;
      collectionSlider.gameObject.SetActive(false);
    }

    void Update()
    {
      // Increment the timer by the time elapsed since the last frame
      
      
      // Horizontal movement input
      moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Jumping
        if (Input.GetButtonDown("Jump"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (transform.position.y < -5f)
        {
            RestartLevel();
        }

        //collecting food
        if (Input.GetKey(KeyCode.E) && inRange)
      {
         timer += Time.deltaTime;
         collectionSlider.gameObject.SetActive(true);
         collectionSlider.value += collectionLength * Time.deltaTime;

      }
        if (collectionSlider.value == collectionSlider.maxValue)
      {
         Debug.Log("Starting Collection");
         StartCollection();
         timer = 0;
         collectionSlider.value = 0;
      }
      if (Input.GetKeyUp(KeyCode.E) || !inRange)
      {
         timer = 0;
         collectionSlider.gameObject.SetActive(false);
         collectionSlider.value = 0;

      }
      collectionSlider.value = Mathf.Clamp(collectionSlider.value, collectionSlider.minValue, collectionSlider.maxValue);
   }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void FixedUpdate()
    {
        // Check if player is touching the ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void OnDrawGizmosSelected()
    {
        // Visualize ground check
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
   public void StartCollection()
   {
      GameObject[] collectibles = GameObject.FindGameObjectsWithTag("Food");
      if (collectibles.Length > 0)
      {
         int randomIndex = Random.Range(0, collectibles.Length);
         FeederScript collectibleScript = collectibles[randomIndex].GetComponent<FeederScript>();
         if(collectibleScript != null)
         {
            // Set the player's transform as the target
            collectibleScript.SetTarget(this.transform);
         }
      }
   }

   //check if player in range
   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (collision.CompareTag("Bowl")) {
         inRange = true;
      } else
      {
         inRange = false;
      }
   }
}
