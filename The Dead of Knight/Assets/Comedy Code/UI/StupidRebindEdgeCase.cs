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

    public void StupidEdgecase()
    {
        var b = action.action.bindings[id];
        var ID = InputActionRebindingExtensions.GetBindingIndex(action.action, b.groups, b.path);
        CM.Rebind(action,text,id,ID);
    }

    public void EvenWorseEdgeCase()
    {
        CM.Rebind(action,text,id,ID);
    }

    [ContextMenu("go fck huiorahtjnbsdkfgjhskdbgdfuiygsbxjfckl")]
    public void test() {
        print(action.action.bindings[id]);
        var b = action.action.bindings[id];
        print(InputActionRebindingExtensions.GetBindingIndex(action.action, b.groups, b.path));
    }
}
