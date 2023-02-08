using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas : MonoBehaviour
{
    public static Canvas canvas_;

    private void Awake() {
        if (canvas_ ==  null)
        {
            canvas_ = this;
            DontDestroyOnLoad(this);
        } else if (canvas_ != this)
        {
            Destroy(gameObject);
        }    
    }
}
