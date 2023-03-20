using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class FuckingStupidUIEdgecase : MonoBehaviour
{
    public static FuckingStupidUIEdgecase EVID;

    public GameObject _default;

    private InputSystemUIInputModule _UIinput;
    private PlayerInput _input;
    private EventSystem _event;
    private GameObject _lastSelected = null;

    private void Awake() {
        EVID = this;
    }

    private void Start() {
        _UIinput = GetComponent<InputSystemUIInputModule>();
        _input = PlayerManager.Player.gameObject.GetComponent<PlayerInput>();
        _event = GetComponent<EventSystem>();
    }

    private void Update() {
        if (_input.currentControlScheme == "KB&M")
        {
            _UIinput.deselectOnBackgroundClick = true;
            if (_event.currentSelectedGameObject != null) {_lastSelected = _event.currentSelectedGameObject;}
            _event.SetSelectedGameObject(null, new BaseEventData(_event));
        } else 
        {
            _UIinput.deselectOnBackgroundClick = false;
            if (_event.currentSelectedGameObject == null)
            {
                if (_lastSelected == null) {_lastSelected = _default;}
                _event.SetSelectedGameObject(_lastSelected, new BaseEventData(_event));
            }
        }
    }
}
