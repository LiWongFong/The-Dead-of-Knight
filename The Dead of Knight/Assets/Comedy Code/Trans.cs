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
    private bool _left = false;
    
    private void OnTriggerEnter2D(Collider2D _other) 
    {
        if (_other.tag == "Player") 
        {
            Trigger();
            Debug.Log("Trigger");
            _falling = PlayerManager.Player.getVelocityY() <= 0f;
            _left = PlayerManager.Player.getVelocityX() <= 0f;
            if (PlayerManager.Player.isFalling()) {PlayerManager.Player.freeze();}
        }
    }

    private void OnTriggerExit2D(Collider2D _other)
    {
        if (_other.tag == "Player") 
        {
            if (!Horizontal && (PlayerManager.Player.getVelocityY() <= 0f && !_falling))
            {
                Trigger();
                Debug.Log("Stupid edge case");
            }

            if (Horizontal && (PlayerManager.Player.getVelocityX() <= 0f ^ _left))
            {
                Trigger();
                Debug.Log("Stupid edge case");
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
        //yield return new WaitForSeconds(0.006f);
        yield return null;
        TransManager.tManager.Unload(unload);
    }
}
