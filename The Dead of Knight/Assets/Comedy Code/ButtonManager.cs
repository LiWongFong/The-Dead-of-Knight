using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void Quit()
    {
        #if UNITY_STANDALONE
            Application.Quit();
        #endif
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void NewGame()
    {
        SceneManager.LoadScene(DataManager.dManager.Level);
        StartCoroutine(LoadEternals());
        SceneManager.UnloadSceneAsync("Main Menu");
    }

    IEnumerator LoadEternals()
    {
        SceneManager.LoadScene("Eternals", LoadSceneMode.Additive);
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Eternals"));
        //add save later
        //x:-91.28 y:-9.21
        //yield return null;
        //PlayerManager.Player.transform.position = new Vector3(-91.28f,-9.21f,0);
    }
}
