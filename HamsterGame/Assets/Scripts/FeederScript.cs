using UnityEngine;

public class FeederScript : MonoBehaviour
{
   private Transform target;
   public float speed = 5f; // Adjust speed in the Inspector
   public PlayerInventory inventory;

   // Call this method to start the collection process
   public void Start()
   {
      GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
      if (playerObject != null)
      {
          inventory = playerObject.GetComponent<PlayerInventory>();
      }
   }
   public void SetTarget(Transform newTarget)
   {
      target = newTarget;
   }

   void Update()
   {
      if (target != null)
      {
         // Calculate the distance to move this frame
         float step = speed * Time.deltaTime;

         // Move towards the target position
         transform.position = Vector3.MoveTowards(transform.position, target.position, step);

         // Optional: Destroy/deactivate object when it reaches close enough to the target
         if (Vector3.Distance(transform.position, target.position) < 0.1f)
         {
            Destroy(gameObject); 
            inventory.foodInInventory++;
            inventory.UpdateUI();

         }
      }
   }
}
