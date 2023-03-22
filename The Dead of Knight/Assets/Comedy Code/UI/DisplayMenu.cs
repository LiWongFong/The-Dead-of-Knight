using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayMenu : Menu
{
    [SerializeField]
    private GameObject _settings;

    public void Return()
    {
        _settings.GetComponent<SettingsMenu>().enabled = true;
        this.gameObject.SetActive(false);
    }
}
