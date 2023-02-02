using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Trans : MonoBehaviour
{
    public string levelName;
    public Dash d;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            d.store();
            PlayerManager.pManager.top = (tag == "Top");
            SceneManager.LoadScene(levelName, LoadSceneMode.Single);
        }
    }
}
