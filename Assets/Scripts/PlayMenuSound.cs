using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayMenuSound : MonoBehaviour
{
    public AudioClip[] clipsForButtons;
    public AudioSource SFXAudioSource;

    public void PlayButtonClip(int indexOfSound)
    {
        SFXAudioSource.clip = clipsForButtons[indexOfSound];
        SFXAudioSource.Play();
    }
}
