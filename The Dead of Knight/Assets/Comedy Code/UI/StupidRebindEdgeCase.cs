using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class StupidRebindEdgeCase : MonoBehaviour
{
    [SerializeField]
    private ControlMenu CM;
    [SerializeField]
    private InputActionReference action;
    [SerializeField]
    private TMP_Text text;
    [SerializeField]
    private int id;

    private void Start() {
        transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = InputControlPath.ToHumanReadableString(action.action.bindings[id].effectivePath,InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    public void StupidEdgecase()
    {
        var b = action.action.bindings[id];
        var ID = InputActionRebindingExtensions.GetBindingIndex(action.action, b.groups, b.path);
        CM.Rebind(action,text,id,ID);
    }

    [ContextMenu("go fck huiorahtjnbsdkfgjhskdbgdfuiygsbxjfckl")]
    public void test()
    {
        foreach (var item in action.action.bindings)
        {
            print(item);
        }
    }
}
