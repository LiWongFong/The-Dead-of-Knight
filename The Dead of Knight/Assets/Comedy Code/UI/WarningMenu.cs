using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WarningMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _default;
    [SerializeField]
    private AudioSource _audio;

    private EventSystem _event;

    private GameObject _lastSelected = null;

    public void Continue()
    {
        DataManager.dManager.Overwrite();
        StartCoroutine(LoadGame());
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

            _audio.volume = 1-a;
            tempColor.a = a;
            img.color = tempColor;

            yield return new WaitForSeconds(0.023f);
        }

        #if UNITY_STANDALONE
            Cursor.visible = false;
        #endif
        #if UNITY_EDITOR
            Cursor.visible = true;
        #endif

        SceneManager.LoadScene(DataManager.dManager.Level);
        SceneManager.LoadScene("Eternals", LoadSceneMode.Additive);
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Eternals"));
        yield return null;
        SceneManager.UnloadSceneAsync("Main Menu");  
    }
}
