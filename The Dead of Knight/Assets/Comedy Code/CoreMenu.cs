using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class CoreMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _default, _options;

    private GameObject _lastSelected = null;

    private EventSystem _event;

    private void OnEnable() {
        if (_event == null) {_event = FuckingStupidUIEdgecase.EVID.GetComponent<EventSystem>();}
        StartCoroutine(MoveSelected());
    }

    private void OnDisable() {
        _lastSelected = _event.currentSelectedGameObject;
    }
    
    public void SaveNQuit()
    {
        //TODO save
        SceneManager.LoadScene("Main Menu");
    }

    public void Open()
    {
        _options.SetActive(true);
        this.enabled = false;
    }

    private IEnumerator MoveSelected()
    {
        yield return null;
        if (_lastSelected == null) {_lastSelected = _default;}
        _event.SetSelectedGameObject(_lastSelected, new BaseEventData(_event));
    }
}
