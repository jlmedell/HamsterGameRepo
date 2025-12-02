using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
   public TMP_Text eatPromptText;
   public float moveSpeed = 30f;
   public float jumpForce = 0.5f;
   public Transform groundCheck;
   public float groundCheckRadius = 0.2f;
   public LayerMask groundLayer;

   private Rigidbody2D rb;
   private SpriteRenderer sr;
   private bool isGrounded;
   private bool hasJumped = false;
   public float moveInput;
   private int lastMoveDirection = 1;
   private float timer;
   private float collectionLength = 65f;
   public Slider collectionSlider; // Reference to UI Slider for collecting
   private bool inRange = false;
   public bool inRangeBurrow = false;
   public MunchFoodScript munchFoodScript;

   public float coyoteTime = 0.2f; // how long after leaving ground you can still jump
   private float coyoteTimeCounter;

   private GameObject[] collectibles;

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
      if (Input.GetButtonDown("Jump") && coyoteTimeCounter > 0.1f && !hasJumped)
      {
         rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
         hasJumped = true;        // prevent double jump
         coyoteTimeCounter = 0f;  // optional, stops further jumps until grounded
      }

      if (transform.position.y < -5f)
      {
         RestartLevel();
      }

      // =========================collecting food=================
      if (inRange && collectibles.Length > 0)
      {
         eatPromptText.text = "Press E to obtain food";
      }
      else
      {
         eatPromptText.text = "";
      }
      if (Input.GetKey(KeyCode.E) && (inRange || inRangeBurrow))
      {
         timer += Time.deltaTime;
         collectionSlider.gameObject.SetActive(true);
         collectionSlider.value += collectionLength * Time.deltaTime;
         munchFoodScript.playMunchSound();
         
         

      }
      collectibles = GameObject.FindGameObjectsWithTag("Food");
      if (collectionSlider.value == collectionSlider.maxValue)
      {
         Debug.Log("Starting Collection");
         if (inRange)
            StartCollection();
         else if (inRangeBurrow)
            StartBurrowCollection();
         timer = 0;
         collectionSlider.value = 0;
      }
      if (Input.GetKeyUp(KeyCode.E) || (!inRange && !inRangeBurrow) || collectibles.Length <= 0)
      {
         timer = 0;
         collectionSlider.gameObject.SetActive(false);
         collectionSlider.value = 0;
         
      }
      if(Input.GetKeyUp(KeyCode.E))
      {
         munchFoodScript.stopSound();
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
      //collectibles = GameObject.FindGameObjectsWithTag("Food");
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
   public void StartBurrowCollection() {
      GameObject findNearbyFood = FindClosestObjectWithTag("BurrowFood");
      FeederScript collectibleScript = findNearbyFood.GetComponent<FeederScript>();
      if (collectibleScript != null)
      {
         // Set the player's transform as the target
         collectibleScript.SetTarget(this.transform);
      } 
   }

   //check if player in range
   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (collision.CompareTag("Bowl")) {
         inRange = true;
      }
      if (collision.CompareTag("BurrowFood"))
      {
         inRangeBurrow = true;
      }
   }

   private void OnTriggerExit2D(Collider2D collision)
   {
      if (collision.CompareTag("Bowl")) {
         inRange = false; 
         }
      if (collision.CompareTag("BurrowFood")) {
         inRangeBurrow = false;
      }
   }

//fix bug of picking up food far away
   GameObject FindClosestObjectWithTag(string tag)
    {
        GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag(tag);
        GameObject nearestObject = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject obj in gameObjectsWithTag)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestObject = obj;
            }
        }
        return nearestObject;
    }
}
