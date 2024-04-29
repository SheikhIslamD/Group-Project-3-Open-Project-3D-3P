using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SaveSystem : Singleton<SaveSystem>
{

    protected override void OnAwake()
    {

        SaveData s = saveData;

        Level1DataEvent?.Invoke(saveData.levelComplete1);
        Level2DataEvent?.Invoke(saveData.levelComplete2);
        Level3DataEvent?.Invoke(saveData.levelComplete3);
        TutorialDataEvent?.Invoke(saveData.tutorialComplete);

        if (finalCutsceneTrigger)
        {
            int stars = 0;
            if (saveData.levelComplete1) stars++;
            if (saveData.levelComplete2) stars++;
            if (saveData.levelComplete3) stars++;

            completionText.text = stars.ToString() + "/3";
            if (stars == 3) finalCutsceneTrigger.SetActive(true);
        }
    }

    public UnityEvent<bool> Level1DataEvent;
    public UnityEvent<bool> Level2DataEvent;
    public UnityEvent<bool> Level3DataEvent;
    public UnityEvent<bool> TutorialDataEvent;
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




    public static SaveData saveData
    {
        get
        {
            if (_saveData == null)
            {
                _saveData = new();
                _saveData.ReadData();
            }
            return _saveData;
        }
    }
    public static SaveData _saveData;
    [Serializable]
    public class SaveData
    {
        public bool levelComplete1;
        public bool levelComplete2;
        public bool levelComplete3;
        public bool tutorialComplete;

        /*
        private string dataScope() =>
#if UNITY_EDITOR
            Application.persistentDataPath + "/UNITYPROJECTDATA";
#else
            return Application.dataPath;
#endif
         */
        private string dataScope() => Application.dataPath;

        private const string dataPath = "/Saves/SaveData.json";

        public void WriteData()
        {
            Directory.CreateDirectory(dataScope() + "/Saves");
            using StreamWriter save = File.CreateText(dataScope() + dataPath);
            save.WriteLine(JsonUtility.ToJson(this, true));
        }

        public void ReadData()
        {
            Directory.CreateDirectory(dataScope() + "/Saves");
            if (!File.Exists(dataScope() + dataPath)) WriteData();
            using StreamReader load = File.OpenText(dataScope() + dataPath);
            JsonUtility.FromJsonOverwrite(load.ReadToEnd(), this);
        }

    }

}
