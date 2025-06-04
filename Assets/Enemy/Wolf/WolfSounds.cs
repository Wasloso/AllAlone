using Sounds;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class WolfSounds : MonoBehaviour

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
        SoundManager.PlaySoundOneShot(SoundType.EnemyAttack, audioSource);
    }

    public void PlayDiedSound()
    {
        SoundManager.PlaySound(SoundType.EnemyDied);
    }
}
