using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trans : MonoBehaviour
{
    public string nextScene;
    public string prevScene;

    private bool falling = false;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Player") 
        {
            Trigger();
            if (PlayerManager.Player.getVelocity() < 0f) {falling = true;}
            else {falling = false;}
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player") 
        {
            if (PlayerManager.Player.getVelocity() < 0f && !falling)
            {
                Trigger();
                Debug.Log("Trigger");
            }
            Debug.Log("Exit");
        }
    }

    public void Trigger()
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

    IEnumerator UnloadScene(string unload)
    {
        yield return new WaitForSeconds(0.006f);
        TransManager.tManager.Unload(unload);
    }
}
