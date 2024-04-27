using UnityEngine;
using System.Collections.Generic;

public class GameplayPauseManager : Singleton<GameplayPauseManager>
{

    public static bool paused { get => instance._paused; private set => instance._paused = value; }
    private bool _paused;
    private List<Pauseable> pauseables = new();

    private void _SetPause(bool value)
    {
        if (paused == value) return;
        paused = value;

        for (int i = 0; i < pauseables.Count; i++) pauseables[i].SetPause(paused);

    }

    public static void SetPause(bool value) => instance._SetPause(value);
    public static void Pause() => instance._SetPause(true);
    public static void UnPause() => instance._SetPause(false);
    public static void TogglePause() => instance._SetPause(!paused);


    public static void RegisterPausable(Pauseable pauseable)
    {
        instance.pauseables.Add(pauseable);
        pauseable.registered = true;
    }
    public static void UnRegisterPausable(Pauseable pauseable)
    {
        instance.pauseables.Remove(pauseable);
        pauseable.registered = false;
    }

    private void OnDisable() => UnRegisterAll();
    private void OnDestroy() => UnRegisterAll();

    private void UnRegisterAll()
    {
        Debug.Log("Unregistering Pausables");
        for (int i = 0; i < pauseables.Count; i++) UnRegisterPausable(pauseables[i]);
    }

}
