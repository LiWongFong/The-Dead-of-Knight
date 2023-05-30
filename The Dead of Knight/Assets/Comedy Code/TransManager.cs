using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransManager : MonoBehaviour
{
    public static TransManager tManager;

    private void Awake()
    {
        if (tManager ==  null)
        {
            tManager = this;
        } else if (tManager != this)
        {
            Destroy(gameObject);
        }    
    }

    public void Load(int _nextScene)
    {
        if (!SceneManager.GetSceneByBuildIndex(_nextScene).isLoaded)
        {
            SceneManager.LoadScene(_nextScene, LoadSceneMode.Additive);
        }
    }

    public void Load(string _nextScene)
    {
        if (!SceneManager.GetSceneByName(_nextScene).isLoaded)
        {
            SceneManager.LoadScene(_nextScene, LoadSceneMode.Additive);
        }
    }

    public void Unload(int _prevScene)
    {
        if (SceneManager.GetSceneByBuildIndex(_prevScene).isLoaded)
        {
            SceneManager.UnloadSceneAsync(_prevScene);
        }
    }

    public void Unload(string _prevScene)
    {
        if (SceneManager.GetSceneByName(_prevScene).isLoaded)
        {
            Debug.Log("Unload");
            SceneManager.UnloadSceneAsync(_prevScene);
        }
    }
}
