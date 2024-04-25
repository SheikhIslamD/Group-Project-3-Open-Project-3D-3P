using System.Collections;
using UnityEngine;

public class MenuButtons : MonoBehaviour
{

    public void LoadLevel(string levelname) => UnityEngine.SceneManagement.SceneManager.LoadScene(levelname);

    public void QuitGame() => Application.Quit();

    public void BeginGame()
    {
        if(!SaveSystem.saveData.tutorialComplete) UnityEngine.SceneManagement.SceneManager.LoadScene("CutsceneScene");
        else UnityEngine.SceneManagement.SceneManager.LoadScene("Hub");
    }
    
}
