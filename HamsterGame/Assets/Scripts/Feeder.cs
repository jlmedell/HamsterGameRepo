using UnityEngine;
using UnityEngine.UI;

public class Feeder : MonoBehaviour
{
   public GameObject foodObject; // anything you want to activate
   public Slider chargeSlider; // Reference to your UI Slider
   public float chargeRate = 10f; // How fast the bar charges per second
   public float dischargeRate = 20f; // How fast the bar discharges per second
   public float stationaryDischargeRate = 10f;
   public Rigidbody2D playerRb;     //used to get velocity of player

   private bool isCharging = false;
   public bool canSpawn = false;

   public void Activate()
   {
      isCharging = true;
   }

   public void Deactivate()
   {
      isCharging = false;
      // Decrease the slider value (if not charging and not at min)

   }

   //Check if player is on wheel, & increase slider if player
   //is moving on wheel
   //
   //otherwise slider will go down if player is moving outside of the wheel.
   void Update()
   {
      if (isCharging && (playerRb.linearVelocity.x != 0))
      {

         // Increase the slider value
         chargeSlider.value += chargeRate * Time.deltaTime;

      }
      else
      {
         // Decrease the slider value (if not charging and not at min)
         if (chargeSlider.value > chargeSlider.minValue && playerRb.linearVelocity.x != 0)
         {
            chargeSlider.value -= dischargeRate * Time.deltaTime;

         }

         else if (chargeSlider.value > chargeSlider.minValue && playerRb.linearVelocity.y < 0)
         {
            chargeSlider.value -= dischargeRate * Time.deltaTime;



         }
         else
         {
            chargeSlider.value -= stationaryDischargeRate * Time.deltaTime;
         }

         chargeSlider.value = Mathf.Clamp(chargeSlider.value, chargeSlider.minValue, chargeSlider.maxValue);
      }
      if (chargeSlider.value > 0) { 
         canSpawn = true;
      } 
      else
      {
         canSpawn = false;
      }
   }
}