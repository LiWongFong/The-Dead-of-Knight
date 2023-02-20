using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSortingLayer : MonoBehaviour
{
    public string SortingLayerName;

    private void Awake() {
        GetComponent<Renderer>().sortingLayerName = SortingLayerName;
        Debug.Log(GetComponent<Renderer>().sortingLayerName);

        foreach (var layer in SortingLayer.layers)
        {
            Debug.Log(layer.name);
        }
    }
}
