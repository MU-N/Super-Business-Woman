using UnityEngine.Audio;
using UnityEngine;

namespace Nasser.SBW.Core
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume = 1;
        [Range(0f, 3f)]
        public float pitch = 1;

        public bool loop;
        public AudioMixerGroup mixer;

        [HideInInspector] public AudioSource source;
    }
}