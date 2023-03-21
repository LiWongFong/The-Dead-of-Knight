using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class WarningMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _default;

    private EventSystem _event;

    private GameObject _lastSelected = null;

    public void Continue()
    {
        DataManager.dManager.Overwrite();
        SceneManager.LoadScene(DataManager.dManager.Level);
        StartCoroutine(LoadEternals());
        SceneManager.UnloadSceneAsync("Main Menu");  
    }

    public void Exit()
    {
        GameObject.Find("Main Menu").GetComponent<MainMenu>().enabled = true;
        gameObject.SetActive(false);
    }

    private void OnEnable() {
        if (_event == null) {_event = GameObject.Find("EventSystem").GetComponent<EventSystem>();}
        StartCoroutine(MoveSelected());
    }

    private void OnDisable() {
        _lastSelected = _event.currentSelectedGameObject;
    }

    IEnumerator LoadEternals()
    {
        SceneManager.LoadScene("Eternals", LoadSceneMode.Additive);
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Eternals"));
    }

    private IEnumerator MoveSelected()
    {
        yield return null;
        if (_lastSelected == null) {_lastSelected = _default;}
        _event.SetSelectedGameObject(_lastSelected, new BaseEventData(_event));
    }
}
