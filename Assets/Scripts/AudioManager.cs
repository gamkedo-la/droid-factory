using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource fxAudioSource;
    public AudioClip[] fxClips;
    
    public void PlayAudio()
    {
        audioSource.Play();
    }

    public void PlayAudio(AudioClip audioClip, bool shouldLoop)
    {
        audioSource.Stop();
        audioSource.loop = shouldLoop;
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }

    public void PauseAudio()
    {
        audioSource.Pause();
    }

}
