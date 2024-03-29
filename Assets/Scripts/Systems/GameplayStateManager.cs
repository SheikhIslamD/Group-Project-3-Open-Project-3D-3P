using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayStateManager : Singleton<GameplayStateManager>
{
    GameplayInputReader input;
    GameplayPauseManager pause;
    HUDUIManager hudUI;

    public bool paused;

    [SerializeField] GameObject pauseMenuHolder;

    void Awake()
    {
        GameplayInputReader.Get(ref input);
        GameplayPauseManager.Get(ref pause);
        HUDUIManager.Get(ref hudUI);
    }

    public void TogglePause()
    {
        paused.Toggle();
        GameplayPauseManager.SetPause(paused);
        pauseMenuHolder.SetActive(paused);
    }

    public void ResetLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void SwitchPrototypeLevel() => SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex == 1) ? 0 : 1);

    public void QuitApplication() => Application.Quit();

}
