using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameplayPauseManager : Singleton<GameplayPauseManager>
{

    public static bool isPaused { get { return Get()._isPaused; } private set { Get()._isPaused = value; } }
    private bool _isPaused;
    private List<Pauseable> pauseables = new List<Pauseable>(); 

    private void _SetPause(bool value)
    {
        if(isPaused == value) return;
        isPaused = value;

        for (int i = 0; i < pauseables.Count; i++) pauseables[i].SetPause(isPaused);

    }

    public static void SetPause(bool value) => Get()._SetPause(value);
    public static void Pause() => Get()._SetPause(true);
    public static void UnPause() => Get()._SetPause(false);
    public static void TogglePause() => Get()._SetPause(!isPaused);


    public static void RegisterPausable(Pauseable pauseable) => Get().pauseables.Add(pauseable);
    public static void UnRegisterPausable(Pauseable pauseable) => Get().pauseables.Remove(pauseable);

    private void OnDisable() => UnRegisterAll();
    private void OnDestroy() => UnRegisterAll();
    void UnRegisterAll()
    {
        for (int i = 0; i < pauseables.Count; i++) Object.Destroy(pauseables[i]);
    }
}
