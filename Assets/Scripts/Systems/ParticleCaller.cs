using AYellowpaper.SerializedCollections;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class ParticleCaller : MonoBehaviour
{

    [SerializedDictionary("Name", "Particle Effect")]
    public SerializedDictionary<string, Effect> clips;

    [Serializable]
    public struct Effect
    {
        public GameObject prefab;
        public Transform refTransform;
    }

    public void PlayEffect(string name) => PlayEffect(name, false);
    public void PlayEffect(string name,  bool warn = true)
    {
        if (remote) { remote.PlayEffect(name, warn); return; }

        bool nameExists = clips.TryGetValue(name, out Effect effect);
        if (!nameExists) { if (warn) Debug.LogWarningFormat("No effect with name {0} found on {1}.", name, gameObject); }
        else if (effect.prefab == null) Debug.LogWarningFormat("Open effect slot with intended name \"{1}\" on {0} found, ensure to fill at some point.", gameObject, name);
        else
        {
            if(effect.refTransform == null) Instantiate(effect.prefab, transform.position, transform.rotation, null);
            else Instantiate(effect.prefab, effect.refTransform.position, effect.refTransform.rotation, null);
        }

    }

    public ParticleCaller remote;



}
