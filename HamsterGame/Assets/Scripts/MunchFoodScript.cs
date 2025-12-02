using UnityEngine;

public class MunchFoodScript : MonoBehaviour
{
   public AudioClip munchSound;
   public AudioClip BurrowSound;
   public AudioClip BGMTrack;
   private AudioSource audiosource;
   private AudioSource BGM;

   void Start()
    {
      audiosource = GetComponent<AudioSource>();
      if (audiosource == null)
      {
         audiosource = gameObject.AddComponent<AudioSource>();
      }
      if (BGM == null)
      {
         BGM = gameObject.AddComponent<AudioSource>();
      }
      BGM.volume = 0.5f;
      BGM.PlayOneShot(BGMTrack);
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
