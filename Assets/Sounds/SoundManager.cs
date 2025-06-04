//Author: Small Hedge Games
//Updated: 13/06/2024

using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Sounds
{
    [RequireComponent(typeof(AudioSource))]
    
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private SoundsSO SO;
        private static SoundManager instance = null;
        private AudioSource audioSource;
        private AudioSource musicSource;
        private AudioClip[] musicPlaylist;
        private int currentMusicIndex = 0;
        private bool isPlayingMusic = false;

        private void Awake()
        {
            if(!instance)
            {
                instance = this;
                audioSource = GetComponent<AudioSource>();
                GameObject musicObj = new GameObject("MusicSource");
                musicObj.transform.parent = transform;
                musicSource = musicObj.AddComponent<AudioSource>();
                musicSource.loop = false;
                musicSource.playOnAwake = false;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Start()
        {
            PlayMusic(SoundType.AmbientMusic);
        }

        private void Update()
        {
            if (isPlayingMusic && !musicSource.isPlaying)
            {
                PlayNextMusicTrack();
            }
        }

        public static void PlaySound(SoundType sound, AudioSource source = null, float volume = 1)
        {
            SoundList soundList = instance.SO.sounds[(int)sound];
            AudioClip[] clips = soundList.sounds;
            AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];

            if(source)
            {
                source.outputAudioMixerGroup = soundList.mixer;
                source.clip = randomClip;
                source.volume = volume * soundList.volume;
                source.Play();
            }
            else
            {
                instance.audioSource.outputAudioMixerGroup = soundList.mixer;
                instance.audioSource.PlayOneShot(randomClip, volume * soundList.volume);
            }
        }
        public static void PlaySoundOneShot(SoundType sound, AudioSource source = null, float volume = 1)
        {
            SoundList soundList = instance.SO.sounds[(int)sound];
            AudioClip[] clips = soundList.sounds;
            AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];

            if(source)
            {
                source.outputAudioMixerGroup = soundList.mixer;
                source.PlayOneShot(randomClip, volume * soundList.volume);
            }
            else
            {
                instance.audioSource.outputAudioMixerGroup = soundList.mixer;
                instance.audioSource.PlayOneShot(randomClip, volume * soundList.volume);
            }
        }
        public static void PlayMusic(SoundType sound)
        {
            SoundList soundList = instance.SO.sounds[(int)sound];
            instance.musicPlaylist = soundList.sounds;
            instance.currentMusicIndex = 0;

            instance.musicSource.outputAudioMixerGroup = soundList.mixer;
            instance.musicSource.loop = false;
            instance.isPlayingMusic = true;

            instance.PlayNextMusicTrack();
        }
        private void PlayNextMusicTrack()
        {
            if (musicPlaylist == null || musicPlaylist.Length == 0) return;

            musicSource.clip = musicPlaylist[currentMusicIndex];
            musicSource.Play();

            currentMusicIndex = (currentMusicIndex + 1) % musicPlaylist.Length;
        }


    }
    
   
    [Serializable]
    public struct SoundList
    {
        [HideInInspector] public string name;
        [Range(0, 1)] public float volume;
        public AudioMixerGroup mixer;
        public AudioClip[] sounds;
    }
}