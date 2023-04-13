using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ExtensionMethods;

public enum Save
{
    Controls,
    Place
}

public class DataManager : MonoBehaviour
{
    public static DataManager dManager;

            
    private SaveData _save = null;

    private static string SaveFilePath;
    private static string ControlFilePath;

    private FullScreenMode[] _mode = {FullScreenMode.FullScreenWindow, FullScreenMode.MaximizedWindow, FullScreenMode.Windowed};


    private void Awake() {
        if (dManager ==  null)
        {
            dManager = this;
            DontDestroyOnLoad(this);
        } else if (dManager != this)
        {
            Destroy(gameObject);
        }    

        SaveFilePath = Path.Combine(Application.persistentDataPath, "Save.json");
        ControlFilePath = Path.Combine(Application.persistentDataPath, "Controls.json");
        Application.quitting += Quit;

        Application.runInBackground = Extensions.intToBool(PlayerPrefs.GetInt("RunInBackground", 0));
        Screen.fullScreenMode = _mode[PlayerPrefs.GetInt("FullScreenMode", 2)];
    }

    public SaveData SaveFile {get => _save;}
    public string Level {get => _save.Level; set => _save.Level = value;}
    public Vector2 Position {get => _save.Position; set => _save.Position = value;}
    public Vector2 Velocity {get => _save.Velocity; set => _save.Velocity = value;}

    private void Start() {
        try {Load();}
        catch (FileNotFoundException e) {print(e);}
    }

    public void Save()
    {
        // This creates a new StreamWriter to write to a specific file path
        using (StreamWriter writer = new StreamWriter(SaveFilePath))
        {
            // This will convert our Data object into a string of JSON
            string dataToWrite = JsonUtility.ToJson(_save);

            // This is where we actually write to the file
            writer.Write(dataToWrite);
        }
    }

    public void Load()
    {
        // This creates a StreamReader, which allows us to read the data from the specified file path
        using (StreamReader reader = new StreamReader(SaveFilePath))
        {
            // We read in the file as a string
            string dataToLoad = reader.ReadToEnd();

            // Here we convert the JSON formatted string into an actual Object in memory
            _save = JsonUtility.FromJson<SaveData>(dataToLoad);
        }
    }

    public void Overwrite()
    {
        _save = new SaveData("W11",new Vector2(-91.28f,-9.21f),new Vector2(0,0));
    }

    private void Quit() {
        Debug.Log("Exit called");
        var t = Time.time;
        Debug.Log(t);
        try
        {
            int countLoaded = SceneManager.sceneCount;
            for (int i = 0; i < countLoaded; i++)
            {
                if (SceneManager.GetSceneAt(i).name.Substring(0,1) == "W")
                {
                    _save.Level = SceneManager.GetSceneAt(i).name;
                }
            }

            _save.Position = PlayerManager.Player.transform.position.AsVector2();

            if (PlayerManager.Player.GetComponent<Rigidbody2D>().constraints == RigidbodyConstraints2D.FreezeRotation)
            {
                _save.Velocity = PlayerManager.Player.GetComponent<Rigidbody2D>().velocity;
            }
            else
            {
                _save.Velocity = PlayerManager.Player.StoredMomentum;
            }
        }
        catch (Exception e)
        {
            print(e);
        }
        if (_save != null) {Save();}
        Debug.Log(Time.time);
        Debug.Log(Time.time - t);

        //task manager kill. This is terrible but otherwise it will freeze
        if (!Application.isEditor) {System.Diagnostics.Process.GetCurrentProcess().Kill();}
    }
}