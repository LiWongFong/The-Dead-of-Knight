using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ControlMenu : Menu
{
    [SerializeField]
    private GameObject _settings;

    private InputActionRebindingExtensions.RebindingOperation rebindOperation;

    public void Return()
    {
        _settings.GetComponent<SettingsMenu>().enabled = true;
        this.gameObject.SetActive(false);
    }

    public void Rebind(InputActionReference action, TMP_Text text)
    {
        text.text = "Waiting";

        rebindOperation = action.action.PerformInteractiveRebinding()
            .WithControlsExcluding("&lt;Pointer&gt;/position")
            .WithControlsExcluding("&lt;Pointer&gt;/delta")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => rebindComplete(action, text))
            .Start();
            
    }

    private void rebindComplete(InputActionReference action, TMP_Text text)
    {
        rebindOperation.Dispose();

        text.text = InputControlPath.ToHumanReadableString(action.action.bindings[0].effectivePath,InputControlPath.HumanReadableStringOptions.OmitDevice);
    }
}
