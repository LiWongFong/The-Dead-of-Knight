using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager dManager;

            
    private SaveData _save = null;

    private static string DataFilePath;


    private void Awake() {
        if (dManager ==  null)
        {
            dManager = this;
            DontDestroyOnLoad(this);
        } else if (dManager != this)
        {
            Destroy(gameObject);
        }    

        DataFilePath = Path.Combine(Application.persistentDataPath, "GameData.json");
    }

    public SaveData SaveFile {get => _save;}
    public string Level {get => _save.Level; set => _save.Level = value;}
    public Vector2 Position {get => _save.Position; set => _save.Position = value;}
    public Vector2 Velocity {get => _save.Velocity; set => _save.Velocity = value;}

    private void Start() {
        if (_save == null) {_save = new SaveData("W11",new Vector2(-91.28f,-9.21f),new Vector2(0,0));}
        print(_save.toString());
    }

    public void Save()
    {
        // This creates a new StreamWriter to write to a specific file path
        using (StreamWriter writer = new StreamWriter(DataFilePath))
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
        using (StreamReader reader = new StreamReader(DataFilePath))
        {
            // We read in the file as a string
            string dataToLoad = reader.ReadToEnd();

            // Here we convert the JSON formatted string into an actual Object in memory
            _save = JsonUtility.FromJson<SaveData>(dataToLoad);
        }
    }


}
