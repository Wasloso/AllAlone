using Sounds;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(AudioSource))]
public class PlayerSounds : MonoBehaviour

{
   [SerializeField] private AudioSource audioSource;
   
   private void Awake()
   {
       if (!audioSource )
           audioSource = GetComponent<AudioSource>();
   }
    public void PlayFootstep()
    {
        SoundManager.PlaySoundOneShot(SoundType.PlayerWalk, audioSource);
    }


    public void PlayAttackSound()
    {
        SoundManager.PlaySoundOneShot(SoundType.PlayerAttack, audioSource);
    }

    public void PlayDiedSound()
    {
        SoundManager.PlaySoundOneShot(SoundType.PlayerDied, audioSource);
    }
}
