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

    public void Unload(int scene)
    {
        if (SceneManager.GetSceneByBuildIndex(scene).isLoaded)
        {
            SceneManager.UnloadSceneAsync(scene);
        }
    }

    public void Unload(string scene)
    {
        if (SceneManager.GetSceneByName(scene).isLoaded)
        {
            SceneManager.UnloadSceneAsync(scene);
        }
    }
}
