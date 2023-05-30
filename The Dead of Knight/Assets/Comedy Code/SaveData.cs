using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData : IEquatable<SaveData>
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

    public bool Equals(SaveData otherSave)
    {
        if (otherSave == null)
            return false;

        if (this.Level == otherSave.Level && this.Position == otherSave.Position && this.Velocity == otherSave.Velocity)
            return true;
        else
            return false;
    }

    // override object.Equals
    public override bool Equals(object obj)
    {
        //
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //
        
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        SaveData saveObj = obj as SaveData;
        if (saveObj == null)
            return false;
        else
            return Equals(saveObj);
    }
    
    // override object.GetHashCode
    public override int GetHashCode()
    {
        // TODO: write your implementation of GetHashCode() here
        throw new System.NotImplementedException();
    }

    public static bool operator == (SaveData save1, SaveData save2)
    {
        if (((object)save1) == null || ((object)save2) == null)
            return System.Object.Equals(save1, save2);

        return save1.Equals(save2);
    }

    public static bool operator != (SaveData save1, SaveData save2)
    {
        if (((object)save1) == null || ((object)save2) == null)
            return !System.Object.Equals(save1, save2);

        return !save1.Equals(save2);
    }
}
