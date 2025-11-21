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
   private SpriteRenderer sr;
   private bool isGrounded;
   private bool hasJumped = false;
   private float moveInput;
   private int lastMoveDirection = 1;
   private float timer;
   private float collectionLength = 50f;
   public Slider collectionSlider; // Reference to UI Slider for collecting
   private bool inRange = false;

   public float coyoteTime = 0.2f; // how long after leaving ground you can still jump
   private float coyoteTimeCounter;

   void Start()
   {
      rb = GetComponent<Rigidbody2D>();
      sr = GetComponent<SpriteRenderer>();
      timer = 0;
      collectionSlider.gameObject.SetActive(false);
   }

   void Update()
   {


       


      // Check if grounded
      isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

      // Reset coyote timer and jump flag if grounded
      if (isGrounded)
      {
         coyoteTimeCounter = coyoteTime;
         hasJumped = false;
      }
      else
      {
         coyoteTimeCounter -= Time.deltaTime;
      }

      // Horizontal movement input
      moveInput = Input.GetAxisRaw("Horizontal");
      rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

      // flip sprite
      if (moveInput != 0)
         lastMoveDirection = (moveInput > 0) ? 1 : -1;
      sr.flipX = lastMoveDirection == -1;

      // Jumping
      if (Input.GetButtonDown("Jump") && coyoteTimeCounter > 0f && !hasJumped)
      {
         rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
         hasJumped = true;        // prevent double jump
         coyoteTimeCounter = 0f;  // optional, stops further jumps until grounded
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
         if (collectibleScript != null)
         {
            // Set the player's transform as the target
            collectibleScript.SetTarget(this.transform);
         }
      }
   }

   //check if player in range
   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (collision.CompareTag("Bowl"))
         inRange = true;
   }

   private void OnTriggerExit2D(Collider2D collision)
   {
      if (collision.CompareTag("Bowl"))
         inRange = false;
   }
}
