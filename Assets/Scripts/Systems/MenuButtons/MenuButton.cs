using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public void LoadLevel1()
    {
        Debug.Log("Button pressed");
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
    }
}