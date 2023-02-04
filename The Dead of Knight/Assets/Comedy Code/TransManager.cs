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
            DontDestroyOnLoad(this);
        } else if (tManager != this)
        {
            Destroy(gameObject);
        }    
    }

    public void Load(int nextScene)
    {
        if (!SceneManager.GetSceneByBuildIndex(nextScene).isLoaded)
        {
            SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
        }
    }

    public void Load(string nextScene)
    {
        if (!SceneManager.GetSceneByName(nextScene).isLoaded)
        {
            SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
        }
    }

    public void Unload(int prevScene)
    {
        if (SceneManager.GetSceneByBuildIndex(prevScene).isLoaded)
        {
            SceneManager.UnloadSceneAsync(prevScene);
        }
    }

    public void Unload(string prevScene)
    {
        if (SceneManager.GetSceneByName(prevScene).isLoaded)
        {
            SceneManager.UnloadSceneAsync(prevScene);
        }
    }
}
