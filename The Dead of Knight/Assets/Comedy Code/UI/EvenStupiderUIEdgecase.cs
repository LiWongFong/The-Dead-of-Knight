using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class EvenStupiderUIEdgecase : MonoBehaviour
{
    [SerializeField]
    public GameObject _default;

    private InputSystemUIInputModule _UIinput;
    private PlayerInput _input;
    private EventSystem _event;
    private GameObject _lastSelected = null;

    private void Start() {
        _UIinput = GetComponent<InputSystemUIInputModule>();
        _input = GetComponent<PlayerInput>();
        _event = GetComponent<EventSystem>();
    }

    private void Update() {
        if (_input.currentControlScheme == "Mouse")
        {
            _UIinput.deselectOnBackgroundClick = true;
            _event.SetSelectedGameObject(null, new BaseEventData(_event));
            if (_event.currentSelectedGameObject != null) {_lastSelected = _event.currentSelectedGameObject;}
        } else if (_input.currentControlScheme == "Gamepad")
        {
            _UIinput.deselectOnBackgroundClick = false;
            if (_event.currentSelectedGameObject == null)
            {
                if (_lastSelected == null) {_lastSelected = _default;}
                _event.SetSelectedGameObject(_lastSelected, new BaseEventData(_event));
            }
        } else if (_input.currentControlScheme == "Keyboard")
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
