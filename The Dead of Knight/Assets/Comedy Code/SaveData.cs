using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string Level;
    public Vector2 Position;
    public Vector2 Velocity;

    public SaveData() {}

    public SaveData(string _level, Vector2 _position, Vector2 _velocity)
    {
        Level = _level;
        Position = _position;
        Velocity = _velocity;
    }

    public string toString()
    {
        string full = "";
        full += "Level: "+Level+"\n";
        full += "Position: "+Position+"\n";
        full += "Velocity: "+Velocity+"\n";
        return full;
    }
}
