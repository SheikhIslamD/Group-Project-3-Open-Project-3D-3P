using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCaller : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip[] clips;
    public string[] clipNames;

    public int soundID;

    public void PlaySound(int soundID)
    {
        if (clips[soundID] == null)
        {
            Debug.LogErrorFormat("Open sound slot {1} on {0} found, ensure to fill at some point.", gameObject, soundID);
            return;
        }
        audioSource.PlayOneShot(clips[soundID]);
    }
    public void PlaySound(string soundName)
    {
        for (int i = 0; i < clips.Length; i++)
        {
            if (clipNames[i] == soundName)
            {
                if (clips[i] == null)
                {
                    Debug.LogErrorFormat("Open sound slot with intended name \"{1}\" on {0} found, ensure to fill at some point.", gameObject, soundName);
                    return;
                }

                audioSource.PlayOneShot(clips[i]);
                return;
            }
        }
        Debug.LogErrorFormat("No sound with name {0} found on {1}.", soundName, gameObject);
    }

    /* These ones are not very helpful so they're commented out.

    public void PlayCurrentSound()
    {
        audioSource.PlayOneShot(clips[soundID]);
    }

    public void PlaySoundNonOneShot(int soundID)
    {
        audioSource.clip = clips[soundID];
        audioSource.Play();
    }
    public void PlayCurrentSoundNonOneShot()
    {
        audioSource.clip = clips[soundID];
        audioSource.Play();
    }
     */

    private void Awake()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (audioSource == null) enabled = false;
    }
}
