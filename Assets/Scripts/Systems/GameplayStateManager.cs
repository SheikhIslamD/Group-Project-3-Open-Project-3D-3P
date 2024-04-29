using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class GameplayStateManager : Singleton<GameplayStateManager>
{
    public static int currentLevel;

    public bool paused;

    [SerializeField] private bool inGame = true;
    [SerializeField] private GameObject pauseMenuHolder;
    [SerializeField] private int levelID = 1;
    [SerializeField] private int endCutsceneID;
    [SerializeField] UnityEvent unPauseEvent;
    [SerializeField] Image blackout;

    #region In Game Functionality

    private void Awake()
    {
        if (!inGame) return;

        currentLevel = levelID;
        if (currentLevel == 0) TUTORIALPOSITION();
    }

    public void TogglePause()
    {
        //Debug.Log("Test");
        paused.Toggle();
        GameplayPauseManager.SetPause(paused);
        if (!paused)
        {
            unPauseEvent?.Invoke();
            return;
        }
        pauseMenuHolder.SetActive(paused);
    }

    public void FinishTutorial() => SaveSystem.i.SetTutorialComplete(true);
    private void TUTORIALPOSITION()
    {
        if (SaveSystem.saveData.tutorialComplete) transform.position = GameObject.Find("PostTutorialSpawn").transform.position;
    }

    public void FinishLevel()
    {
        switch (currentLevel)
        {
            case 1: SaveSystem.i.SetLevelComplete1(true); break;
            case 2: SaveSystem.i.SetLevelComplete2(true); break;
            case 3: SaveSystem.i.SetLevelComplete3(true); break;
            default: break;
        }
        CallCutscene(endCutsceneID);
    }

    #endregion

    #region Scene Management Methods

    public void LoadLevel(string levelname) => StartCoroutine(LevelLoadCor(levelname));
    public void LoadLevel(int levelID) => StartCoroutine(LevelLoadCor(Scenes.levelNames[levelID]));

    IEnumerator LevelLoadCor(string name)
    {
        float f = 0.5f;
        if (blackout == null) f = 0;

        while (f > 0)
        {
            f -= Time.deltaTime;
            blackout.color = new(0, 0, 0, 1 - f/0.5f);
            yield return null;
        }

        SceneManager.LoadScene(name);
    }




    public void BeginGame() => StartCoroutine(LevelLoadCor(SaveSystem.saveData.tutorialComplete ? Scenes.hub : Scenes.cutsceneScene));

    public void ResetLevel() => StartCoroutine(LevelLoadCor(SceneManager.GetActiveScene().name));

    public void ReturnToCurrentLevel() => StartCoroutine(LevelLoadCor(Scenes.levelNames[currentLevel]));

    public void QuitToHub() => StartCoroutine(LevelLoadCor(Scenes.hub));
    public void QuitToMenu() => StartCoroutine(LevelLoadCor(Scenes.mainMenu));
    public void QuitApplication()
    {
#if UNITY_EDITOR
        if (Application.isEditor) EditorApplication.ExitPlaymode();
        else 
#endif      
            Application.Quit();
    }

    public void CallCutscene(int id)
    {
        CutsceneSystem.cutsceneID = id;
        StartCoroutine(LevelLoadCor(Scenes.cutsceneScene));
    }

    #endregion

}
