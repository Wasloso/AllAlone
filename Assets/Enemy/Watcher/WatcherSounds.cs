using Sounds;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class WatcherSounds : MonoBehaviour

{
    [SerializeField] private AudioSource audioSource;
   
    private void Awake()
    {
        if (!audioSource )
            audioSource = GetComponent<AudioSource>();
    }
    public void PlayFootstep()
    {
        
    }


    public void PlayAttackSound()
    {
        SoundManager.PlaySoundOneShot(SoundType.WatcherAttack, audioSource);
    }

    public void PlayDiedSound()
    {
        SoundManager.PlaySoundOneShot(SoundType.WatcherDied, audioSource);
    }
}