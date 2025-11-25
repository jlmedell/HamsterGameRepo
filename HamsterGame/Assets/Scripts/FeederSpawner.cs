using System.Collections;
using UnityEngine;

public class FeederSpawner : MonoBehaviour
{
   public GameObject objectToSpawn; // Assign your 2D prefab here in the Inspector
   public float spawnInterval = 5f; // Time in seconds between spawns
   private float timer;
   public Feeder feeder;

   //spawn between these ranges
   public float minX = -1f;
   public float maxX = 1f;
   public float onBowl = 1f;

   void Update()
   {
      timer += Time.deltaTime; // Increment the timer by the time elapsed since the last frame

      if (timer >= spawnInterval && feeder.canSpawn)
      {
         SpawnRandomObject();
         timer = 0f; // Reset the timer
      }
   }

   void SpawnRandomObject()
   {
      float randomX = Random.Range(minX, maxX);


      // Create a new Vector2 for the spawn position (for 2D)
      Vector2 spawnPosition = new(transform.position.x + randomX, transform.position.y + onBowl);
      float randomZRotation = Random.Range(0f, 360f); // Random angle between 0 and 360 degrees
      Quaternion randomRotation = Quaternion.Euler(0, 0, randomZRotation); // X and Y are 0 for 2D
      Instantiate(objectToSpawn, spawnPosition, randomRotation, this.transform); // Spawn the object at the spawner's position
   }
}