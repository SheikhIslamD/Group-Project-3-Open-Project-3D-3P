using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSwitch : MonoBehaviour
{
public AudioClip credits;
public AudioClip main;

    public void ReturnToMain()
    {
        AudioSource audio = GetComponent<AudioSource>();

        audio.Stop();
        audio.clip = main;
        audio.volume = 0.666f;
        audio.Play();
    }

    public void IntoCredits()
    {
        AudioSource audio = GetComponent<AudioSource>();

        audio.Stop();
        audio.clip = credits;
        audio.volume = 0.303f;
        audio.Play();
    }
}
