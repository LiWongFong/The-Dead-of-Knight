using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trans : MonoBehaviour
{
    public string nextScene;
    public string prevScene;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Center") 
        {
            if (!SceneManager.GetSceneByName(nextScene).isLoaded)
            {
                TransManager.tManager.Load(nextScene);
                StartCoroutine(UnloadScene(prevScene));
            } else
            {
                TransManager.tManager.Load(prevScene);
                StartCoroutine(UnloadScene(nextScene));
            }
        }
    }

    IEnumerator UnloadScene(string unload)
    {
        yield return new WaitForSeconds(0.005f);
        TransManager.tManager.Unload(unload);
    }
}
