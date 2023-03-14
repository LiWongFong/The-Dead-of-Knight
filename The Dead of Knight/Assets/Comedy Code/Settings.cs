using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings Setting;

    public bool Controller = false;

    private void Awake() {
        if (Setting ==  null)
        {
            Setting = this;
            DontDestroyOnLoad(this);
        } else if (Setting != this)
        {
            Destroy(gameObject);
        }    
    }
}
