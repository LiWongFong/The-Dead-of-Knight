using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    [SerializeField]
    protected GameObject _default;

    protected GameObject _lastSelected = null;

    protected EventSystem _event;


    protected virtual void OnEnable() {
        if (_event == null) {_event = FuckingStupidUIEdgecase.EVID.GetComponent<EventSystem>();}
        StartCoroutine(MoveSelected());
    }

    protected virtual void OnDisable() {
        _lastSelected = _event.currentSelectedGameObject;
    }

    public void Open(GameObject menu)
    {
        menu.SetActive(true);
        this.enabled = false;
    }

    protected IEnumerator MoveSelected()
    {
        yield return null;
        if (_lastSelected == null) {_lastSelected = _default;}
        _event.SetSelectedGameObject(_lastSelected, new BaseEventData(_event));
    }
}
