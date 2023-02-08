using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trans : MonoBehaviour
{
    public string NextScene;
    public string PrevScene;
    public bool Horizontal = false;

    private bool _falling = false;
    
    private void OnTriggerEnter2D(Collider2D _other) 
    {
        if (_other.tag == "Player") 
        {
            Trigger();
            if (PlayerManager.Player.getVelocity() < 0f) {_falling = true;}
            else {_falling = false;}
        }
    }

    private void OnTriggerExit2D(Collider2D _other)
    {
        if (_other.tag == "Player" && !Horizontal) 
        {
            if (PlayerManager.Player.getVelocity() < 0f && !_falling)
            {
                Trigger();
                Debug.Log("Trigger");
            }
            Debug.Log("Exit");
        }
    }

    public void Trigger()
    {
        if (!SceneManager.GetSceneByName(NextScene).isLoaded)
        {
            TransManager.tManager.Load(NextScene);
            StartCoroutine(UnloadScene(PrevScene));
        } else
        {
            TransManager.tManager.Load(PrevScene);
            StartCoroutine(UnloadScene(NextScene));
        }
    }

    IEnumerator UnloadScene(string unload)
    {
        yield return new WaitForSeconds(0.006f);
        TransManager.tManager.Unload(unload);
    }
}
