using System.Collections;
using UnityEngine;

public class MenuButtons : MonoBehaviour
{

    public void LoadLevel(string levelname) => UnityEngine.SceneManagement.SceneManager.LoadScene(levelname);

    public void LoadLevel(int id)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(Scenes.levelNames[id]);
    }

    public void QuitGame() => Application.Quit();

    public void ReturnToCurrentLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(Scenes.levelNames[GameplayStateManager.currentLevel]);
    }

    public void BeginGame()
    {
        if(!SaveSystem.saveData.tutorialComplete) UnityEngine.SceneManagement.SceneManager.LoadScene("CutsceneScene");
        else UnityEngine.SceneManagement.SceneManager.LoadScene("Hub");
    }
    
}
