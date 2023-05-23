using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _default, _warning;

    private GameObject _lastSelected = null;
    private EventSystem _event = null;

    private void Start() {
        if (DataManager.dManager.SaveFile == null)
        {
            GameObject.Find("Continue").SetActive(false);
            print("no perisoue save");
        }
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
        if (DataManager.dManager.SaveFile == null)
        {
            DataManager.dManager.Overwrite();
            StartCoroutine(LoadGame());
        } 
        else
        {
            _warning.SetActive(true);
            this.enabled = false;
        }
    }

    public void Continue()
    {
        StartCoroutine(LoadGame());
    }

    private void OnEnable() {
        if (_event == null) {_event = GameObject.Find("EventSystem").GetComponent<EventSystem>();}
        StartCoroutine(MoveSelected());

        for (int i = 0; i < GetComponent<RectTransform>().childCount; i++)
        {
            //button.interactalbe = false  uses diabled color
            //button.enabled = false  does not use diabled color
            GetComponent<RectTransform>().GetChild(i).gameObject.GetComponent<Button>().enabled = true;
        }
    }

    private void OnDisable() {
        _lastSelected = _event.currentSelectedGameObject;

        //diable buttons
        for (int i = 0; i < GetComponent<RectTransform>().childCount; i++)
        {
            //button.interactalbe = false  uses diabled color
            //button.enabled = false  does not use diabled color
            GetComponent<RectTransform>().GetChild(i).gameObject.GetComponent<Button>().enabled = false;
        }
    }

    private IEnumerator MoveSelected()
    {
        yield return null;
        if (_lastSelected == null) {_lastSelected = _default;}
        _event.SetSelectedGameObject(_lastSelected, new BaseEventData(_event));
    }

    IEnumerator LoadGame()
    {
        var img = GameObject.Find("Black Screen").GetComponent<Image>();
        float a = 0;
        for (float i = 0; i < 0.69; i += 0.023f)
        {
            //0
            //30
            //0.03225806451
            var tempColor = img.color;
            a += 0.03225806451f;
            tempColor.a = a;
            img.color = tempColor;

            yield return new WaitForSeconds(0.023f);
        }

        SceneManager.LoadScene(DataManager.dManager.Level);
        SceneManager.LoadScene("Eternals", LoadSceneMode.Additive);
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Eternals"));
        yield return null;
        SceneManager.UnloadSceneAsync("Main Menu");  

        #if UNITY_STANDALONE
            Cursor.visible = false;
        #endif
        #if UNITY_EDITOR
            Cursor.visible = true;
        #endif
    }
}
