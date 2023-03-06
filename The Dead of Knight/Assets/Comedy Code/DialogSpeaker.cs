using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSpeaker : MonoBehaviour
{
    [SerializeField]
    private TextAsset Dialog;

    private bool _stay = false;
    
    private string[] _lines;

    private void Start()
    {
        _lines = Dialog.text.Split(
        new string[] { "\r\n", "\r", "\n" },
        StringSplitOptions.None
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _stay = true;
            StartCoroutine(waitCheck());
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player")
        {
            _stay = false;
        } 
    }

    IEnumerator waitCheck()
    {
        yield return new WaitForSeconds(1);
        if (_stay) {Debug.Log(_lines[0]);}
    }
}
