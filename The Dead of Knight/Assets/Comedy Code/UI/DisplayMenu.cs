using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ExtensionMethods;
using TMPro;

public class DisplayMenu : Menu
{
    [SerializeField]
    private GameObject _settings;

    private FullScreenMode[] _mode = {FullScreenMode.FullScreenWindow, FullScreenMode.MaximizedWindow, FullScreenMode.Windowed};

    protected override void OnEnable() {
        if (_event == null) {_event = FuckingStupidUIEdgecase.EVID.GetComponent<EventSystem>();}
        StartCoroutine(MoveSelected());

        GameObject.Find("Toggle").GetComponent<Toggle>().SetIsOnWithoutNotify(Application.runInBackground);
        GameObject.Find("Dropdown").GetComponent<TMP_Dropdown>().SetValueWithoutNotify(PlayerPrefs.GetInt("FullScreenMode", 2));
    }

    public void Return()
    {
        _settings.GetComponent<SettingsMenu>().enabled = true;
        this.gameObject.SetActive(false);
    }

    public void RunInBackground(bool isRIB)
    {
        Application.runInBackground = isRIB;
        PlayerPrefs.SetInt("RunInBackground", Extensions.boolToInt(Application.runInBackground));
        Debug.Log("RIB Triggered");
        Debug.Log(Application.runInBackground);
    }

    public void dropdown(int i)
    {
        Screen.fullScreenMode = _mode[i];
        PlayerPrefs.SetInt("FullScreenMode", i);
        Debug.Log("Dropdown: " + _mode[i]);
    }
}
