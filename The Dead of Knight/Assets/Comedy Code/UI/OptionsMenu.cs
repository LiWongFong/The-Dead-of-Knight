using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _default, _core;

    private GameObject _lastSelected = null;

    private EventSystem _event;

    private void OnEnable() {
        if (_event == null) {_event = FuckingStupidUIEdgecase.EVID.GetComponent<EventSystem>();}
        StartCoroutine(MoveSelected());
    }

    private void OnDisable() {
        _lastSelected = _event.currentSelectedGameObject;
    }

    public void Return()
    {
        _core.GetComponent<CoreMenu>().enabled = true;
        this.gameObject.SetActive(false);
    }
    
    private IEnumerator MoveSelected()
    {
        yield return null;
        if (_lastSelected == null) {_lastSelected = _default;}
        _event.SetSelectedGameObject(_lastSelected, new BaseEventData(_event));
    }
}
