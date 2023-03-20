using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager cManager;

    private void Awake() {
        if (cManager ==  null)
        {
            cManager = this;
        } else if (cManager != this)
        {
            Destroy(gameObject);
        }    
    }
}
