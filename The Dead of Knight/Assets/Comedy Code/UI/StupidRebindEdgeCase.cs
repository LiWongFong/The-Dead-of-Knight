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

    public void StupidEdgecase()
    {
        CM.Rebind(action,text);
    }
}
