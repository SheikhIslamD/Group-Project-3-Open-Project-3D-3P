﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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

    }

    public UnityEvent<bool> Level1DataEvent;
    public UnityEvent<bool> Level2DataEvent;
    public UnityEvent<bool> Level3DataEvent;

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

    public void WriteData() => saveData.WriteData();
    public void ResetSaveData() => saveData.ReadData();
    



    public static SaveData saveData;
    [Serializable]
    public class SaveData
    {
        public bool levelComplete1;
        public bool levelComplete2;
        public bool levelComplete3;

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