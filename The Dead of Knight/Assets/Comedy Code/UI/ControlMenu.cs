using System;
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

    public void Rebind(InputActionReference action, TMP_Text text, int id, Int32 ID)
    {
        text.text = "Waiting";

        rebindOperation = action.action.PerformInteractiveRebinding()
            .WithControlsExcluding("&lt;Pointer&gt;/position")
            .WithControlsExcluding("&lt;Pointer&gt;/delta")
            .WithTargetBinding(ID)
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => rebindComplete(action, text, id))
            .Start();
            
    }

    private void rebindComplete(InputActionReference action, TMP_Text text, int id)
    {
        rebindOperation.Dispose();

        text.text = InputControlPath.ToHumanReadableString(action.action.bindings[id].effectivePath,InputControlPath.HumanReadableStringOptions.OmitDevice);
    }
}
