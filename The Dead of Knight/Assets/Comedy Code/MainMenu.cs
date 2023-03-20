using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    private void Start() {
        if (new SaveData("W11",new Vector2(-91.28f,-9.21f),new Vector2(0,0)) == DataManager.dManager.SaveFile)
        {
            GameObject.Find("Continue").SetActive(false);
            print("no perisoue save");
        }
        print(DataManager.dManager.SaveFile.toString());
    }

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
        print(new SaveData("W11",new Vector2(-91.28f,-9.21f),new Vector2(0,0)).toString());
        print(DataManager.dManager.SaveFile.toString());
        print(new SaveData("W11",new Vector2(-91.28f,-9.21f),new Vector2(0,0)) == DataManager.dManager.SaveFile);
        if (new SaveData("W11",new Vector2(-91.28f,-9.21f),new Vector2(0,0)) == DataManager.dManager.SaveFile)
        {
            SceneManager.LoadScene(DataManager.dManager.Level);
            StartCoroutine(LoadEternals());
            SceneManager.UnloadSceneAsync("Main Menu");  
        } 
        else
        {
            
        }
    }

    public void Continue()
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
