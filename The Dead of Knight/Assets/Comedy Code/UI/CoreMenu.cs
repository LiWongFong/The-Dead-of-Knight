using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ExtensionMethods;

public class CoreMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _default, _options;

    private GameObject _lastSelected = null;

    private EventSystem _event;

    private void OnEnable() {
        if (_event == null) {_event = FuckingStupidUIEdgecase.EVID.GetComponent<EventSystem>();}
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
    
    public void SaveNQuit()
    {
        int countLoaded = SceneManager.sceneCount;
        for (int i = 0; i < countLoaded; i++)
        {
            if (SceneManager.GetSceneAt(i).name.Substring(0,1) == "W")
            {
                DataManager.dManager.Level = SceneManager.GetSceneAt(i).name;
            }
        }

        DataManager.dManager.Position = PlayerManager.Player.transform.position.AsVector2();
        DataManager.dManager.Velocity = PlayerManager.Player.StoredMomentum;

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
