using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class SaveSystem : Singleton<SaveSystem>
{

    protected override void OnAwake()
    {

        if (saveData == null)
        {
            saveData = new();
            saveData.ReadData();
        }

        Level1DataEvent?.Invoke(saveData.levelComplete1);
        Level2DataEvent?.Invoke(saveData.levelComplete2);
        Level3DataEvent?.Invoke(saveData.levelComplete3);

        if(finalCutsceneTrigger)
        {
            int stars = 0;
            if (saveData.levelComplete1) stars++;
            if (saveData.levelComplete2) stars++;
            if (saveData.levelComplete3) stars++;

            completionText.text = stars.ToString() + "/3";
            if(stars == 3) finalCutsceneTrigger.SetActive(true);
        }
    }

    public UnityEvent<bool> Level1DataEvent;
    public UnityEvent<bool> Level2DataEvent;
    public UnityEvent<bool> Level3DataEvent;
    public TextMeshProUGUI completionText;
    public GameObject finalCutsceneTrigger;

    public void SetLevelComplete1(bool value) 
    { 
        saveData.levelComplete1 = value;
        saveData.WriteData();
    }
    public void SetLevelComplete2(bool value) 
    { 
        saveData.levelComplete2 = value;
        saveData.WriteData();
    }
    public void SetLevelComplete3(bool value) 
    { 
        saveData.levelComplete3 = value;
        saveData.WriteData();
    }
    public void SetTutorialComplete(bool value) 
    { 
        saveData.tutorialComplete = value;
        saveData.WriteData();
    }

    public void WriteData() => saveData.WriteData();
    public void ResetSaveData() => saveData.ReadData();
    



    public static SaveData saveData;
    [Serializable]
    public class SaveData
    {
        public bool levelComplete1;
        public bool levelComplete2;
        public bool levelComplete3;
        public bool tutorialComplete;

        string dataPath => Application.persistentDataPath + "/Saves/SaveData.json";

        public void WriteData()
        {

            Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
            using (StreamWriter save = File.CreateText(dataPath))
            {
                save.WriteLine(JsonUtility.ToJson(this, true));
            }
        }

        public void ReadData()
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
            using (StreamReader load = File.OpenText(dataPath))
            {
                JsonUtility.FromJsonOverwrite(load.ReadToEnd(), this);

            }
        }

    }

}
