using UnityEngine;

public class MunchFoodScript : MonoBehaviour
{
   public AudioClip munchSound;
   public AudioClip BurrowSound;
   private AudioSource audiosource;

   void Start()
    {
      audiosource = GetComponent<AudioSource>();
      if (audiosource == null)
      {
         audiosource = gameObject.AddComponent<AudioSource>();
      }
   }

   public void playMunchSound()
   {
      if (!audiosource.isPlaying)
      {
         audiosource.PlayOneShot(munchSound);
      }
   }
   public void playBurrowSound()
   {
      if (!audiosource.isPlaying)
      {
         audiosource.PlayOneShot(BurrowSound);
      }
   }
   public void stopSound()
   {
      if (audiosource.isPlaying)
      {
         audiosource.Stop();
      }
   }
}
