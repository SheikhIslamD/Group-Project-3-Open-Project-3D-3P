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
    [SerializeField] int levelID = 1;

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

    //public void SwitchPrototypeLevel() => SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex == 1) ? 0 : 1);

    public void QuitToMenu() => SceneManager.LoadScene(Scenes.mainMenu);
    public void QuitApplication() => Application.Quit();

    public void LoadLevel(string levelname) => UnityEngine.SceneManagement.SceneManager.LoadScene(levelname);

    public void FinishLevel()
    {
        switch (levelID)
        {
            case 0: SaveSystem.i.SetTutorialComplete(true); break;
            case 1: SaveSystem.i.SetLevelComplete1(true); break;
            case 2: SaveSystem.i.SetLevelComplete2(true); break;
            case 3: SaveSystem.i.SetLevelComplete3(true); break;
            default:
                break;
        }
        if(levelID != 0) SceneManager.LoadScene(Scenes.win);
    }


}
