using MoreMountains.NiceVibrations;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace Nasser.SBW.Core
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] AudioMixerGroup musicMixer;
        [SerializeField] AudioMixerGroup audioMixer;
        public Sound[] sounds;

        public static AudioManager instance;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            // lives through transitioning
            DontDestroyOnLoad(gameObject);
            foreach (Sound sound in sounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;

                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;
                sound.source.outputAudioMixerGroup = sound.mixer;
            }

            Play("Theme");
        }

        public void Play(string name)
        {
            Sound snd = Array.Find(sounds, sound => sound.name == name);
            try
            {
                if (!snd.source.isPlaying)
                    snd.source.Play();
            }
            catch (Exception e)
            {
                Debug.LogWarning("sound not found");
            }
        }
        public void Stop(string name)
        {
            Sound snd = Array.Find(sounds, sound => sound.name == name);
            try
            {
                if (snd.source.isPlaying)
                    snd.source.Stop();
            }
            catch (Exception e)
            {
                Debug.LogWarning("sound not found");
            }


        }

        public void ActivateSound(int ind, bool currentState)
        {
            // music
            if (ind == 0)
            {
                if (currentState)
                    musicMixer.audioMixer.SetFloat("SoundVol", 0);
                else
                    musicMixer.audioMixer.SetFloat("SoundVol", -80);
                MMVibrationManager.Haptic(HapticTypes.LightImpact);
            }

            else if (ind == 1)
            {
                if (currentState)
                    audioMixer.audioMixer.SetFloat("MusicVol", -15);
                else
                    audioMixer.audioMixer.SetFloat("MusicVol", -80);
                MMVibrationManager.Haptic(HapticTypes.LightImpact);
            }
        }
    }

    
}