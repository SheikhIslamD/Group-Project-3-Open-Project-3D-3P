using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SettingsSystem : MonoBehaviour
{


    //Connections
    [SerializeField] TextMeshProUGUI sensSliderText;
    [SerializeField] Image autoPlaySlash;


    //Data
    private SettingsSave save;
    private int thumbstickSens = 3;
    private bool autoPlayCutscenes;
    private float musicVolume = 1;
    private float soundVolume = 1;

    private void OnEnable()
    {
        save = SettingsSave.save;
        thumbstickSens = save.thumbstickSens;
        SetAutoPlayCutscenes(save.autoPlayCutscenes);
        musicVolume = save.musicVolume;
        soundVolume = save.soundVolume;

    }

    private void SaveData()
    {
        save.thumbstickSens = thumbstickSens;
        save.autoPlayCutscenes = autoPlayCutscenes;
        save.musicVolume = musicVolume;
        save.soundVolume = soundVolume;
        save.WriteData();
    }
    private void OnDisable() => SaveData();







    public void SetThumbstickSense(float value)
    {
        thumbstickSens = (int)value;
        sensSliderText.text = value.ToString();
    }
    public void SetAutoPlayCutscenes(bool value)
    {
        autoPlayCutscenes = value;
        autoPlaySlash.gameObject.SetActive(value);
    }
    public void ToggleAutoPlayCutscenes() => SetAutoPlayCutscenes(!autoPlayCutscenes);
    public void SetMusicVolume(float value)
    {
        musicVolume = value;
    }
    public void SetSoundVolume(float value)
    {
        soundVolume = value;
    }

}

public class SettingsSave
{
    public static SettingsSave instance;
    public static SettingsSave save
    {
        get
        {
            if (instance == null)
            {
                instance = new SettingsSave();
                instance.ReadData();
            }
            return instance;
        }
    }

    public int thumbstickSens = 3;

    public float GetThumbstickSensitivity()
    {
        return thumbstickSens switch
        {
            1 => 0.5f,
            2 => 0.75f,
            3 => 1f,
            4 => 1.25f,
            5 => 1.5f,
            _ => 1f
        };
    }

    public bool autoPlayCutscenes;
    public float musicVolume = 1;
    public float soundVolume = 1;

    private string dataPath => Application.persistentDataPath + "/Saves/Settings.json";

    public void WriteData()
    {

        Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
        using StreamWriter save = File.CreateText(dataPath);
        save.WriteLine(JsonUtility.ToJson(this, true));
    }

    public void ReadData()
    {
        Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
        if (!File.Exists(dataPath)) return;
        using StreamReader load = File.OpenText(dataPath);
        JsonUtility.FromJsonOverwrite(load.ReadToEnd(), this);
    }

}