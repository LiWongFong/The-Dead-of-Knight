using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager pManager;

    public Vector2 velocity;
    public float x;
    public bool top;

    private void Awake()
    {
        if (pManager ==  null)
        {
            pManager = this;
            DontDestroyOnLoad(this);
        } else if (pManager != this)
        {
            Destroy(gameObject);
        }    
    }
}
