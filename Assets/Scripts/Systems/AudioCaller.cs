using AYellowpaper.SerializedCollections;
using Unity.VisualScripting;
using UnityEngine;

public class AudioCaller : MonoBehaviour
{

    public AudioSource audioSource;

    [SerializedDictionary("Name", "Audio Clip")]
    public SerializedDictionary<string, AudioClip> clips;

    public void PlaySound(string soundName) => PlaySound(soundName, false);
    public void PlaySound(string soundName, bool warn = true)
    {
        if (remote) { remote.PlaySound(soundName); return; }

        bool nameExists = clips.TryGetValue(soundName, out AudioClip clip);
        if (!nameExists) if (warn) Debug.LogWarningFormat("No sound with name {0} found on {1}.", soundName, gameObject);
        else if (clip == null) Debug.LogWarningFormat("Open sound slot with intended name \"{1}\" on {0} found, ensure to fill at some point.", gameObject, soundName);
        else audioSource.PlayOneShot(clip);
    }

    public AudioCaller remote;




    /* These ones are not very helpful/no longer work so they're commented out.

    
    public int soundID;

        public void PlaySound(string soundName)
    {
        for (int i = 0; i < clips.Length; i++)
        {
            if (clipNames[i] == soundName)
            {
                if (clips[i] == null)
                {
                    Debug.LogWarningFormat("Open sound slot with intended name \"{1}\" on {0} found, ensure to fill at some point.", gameObject, soundName);
                    return;
                }

                audioSource.PlayOneShot(clips[i]);
                return;
            }
        }
        Debug.LogWarningFormat("No sound with name {0} found on {1}.", soundName, gameObject);
    }

    [Obsolete]
    public void PlaySoundID(int soundID)
    {
        if (clips[soundID] == null)
        {
            Debug.LogWarningFormat("Open sound slot {1} on {0} found, ensure to fill at some point.", gameObject, soundID);
            return;
        }
        audioSource.PlayOneShot(clips[soundID]);
    }

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
        if (!remote && !audioSource) audioSource = gameObject.GetOrAddComponent<AudioSource>();
    }
}
