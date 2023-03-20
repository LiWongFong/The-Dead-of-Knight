using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class SetSortingLayer : MonoBehaviour
{
    public string SortingLayerName;

    private void Awake() {
        GetComponent<Renderer>().sortingLayerName = SortingLayerName;
        Debug.Log(GetComponent<Renderer>().sortingLayerName);
    }
}
