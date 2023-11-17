using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMgr : MonoBehaviour
{
    public static AudioMgr instance;

    public void Awake()
    {
        instance = this;
    }

    [SerializeField] Audio[] audioClips;

    [System.Serializable]
    struct Audio
    {
        public string name;
        public AudioClip clip;
        public float volume;
    }

    public void PlayAudioClip(string name)
    {
        for (int i = 0; i < audioClips.Length; i++)
        {
            if (name == audioClips[i].name)
            {
                AudioSource.PlayClipAtPoint(audioClips[i].clip, Camera.main.transform.position);
                return;
            }
        }
    }

    public void StopMusic()
    {
        var musicSource = GetComponent<AudioSource>();
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }
}
