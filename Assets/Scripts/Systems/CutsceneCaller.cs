using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneCaller : MonoBehaviour
{
    public int cutsceneID;

    public void CallCutscene()
    {
        CutsceneSystem.cutsceneID = cutsceneID;
        SceneManager.LoadScene(Scenes.cutsceneScene);
    }
}
