using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

[AddComponentMenu("UI/Custom Button", 31)]
public class CustomButton : Button
{
    public void textUpdate()
    {
        switch (currentSelectionState)
        {
            case SelectionState.Pressed:
                //move text down 12.6
                break;
            default:
                break;
        }
    }

    [ContextMenu("hfgkjhdbfgkhdxbgi")]
    public void test()
    {
        print(currentSelectionState);
    }
}
