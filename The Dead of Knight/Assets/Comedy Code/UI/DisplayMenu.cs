using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ExtensionMethods;

public class DisplayMenu : Menu
{
    [SerializeField]
    private GameObject _settings;

    protected override void OnEnable() {
        if (_event == null) {_event = FuckingStupidUIEdgecase.EVID.GetComponent<EventSystem>();}
        StartCoroutine(MoveSelected());

        GameObject.Find("Toggle").GetComponent<Toggle>().SetIsOnWithoutNotify(Application.runInBackground);
    }

    public void Return()
    {
        _settings.GetComponent<SettingsMenu>().enabled = true;
        this.gameObject.SetActive(false);
    }

    public void RunInBackground()
    {
        Application.runInBackground ^= true;
        PlayerPrefs.SetInt("RunInBackground", Extensions.boolToInt(Application.runInBackground));
        Debug.Log("RIB Triggered");
        Debug.Log(Application.runInBackground);
    }

    public void dropdown(int i)
    {
        Debug.Log("Dropdown: " + i);
    }
}
