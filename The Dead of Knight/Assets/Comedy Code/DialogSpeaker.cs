using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class DialogSpeaker : MonoBehaviour
{
    [SerializeField]
    protected TextAsset Dialog;

    [SerializeField]
    protected TextMeshProUGUI _tmp;

    protected bool _stay = false;
    
    protected string[] _lines;
    protected string display;
    protected int _line = 0;
    protected bool _speaking = false;

    protected void Start()
    {
        _lines = Dialog.text.Split(
        new string[] { "\r\n", "\r", "\n" },
        StringSplitOptions.None
        );
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _stay = true;
            StartCoroutine(waitCheck());
        }
    }

    protected void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player")
        {
            _stay = false;
        } 
    }

    protected IEnumerator waitCheck()
    {
        yield return new WaitForSeconds(1);
        if (_stay && !_speaking) {speak();}
    }

    protected virtual void speak()
    {
        StartCoroutine(type(_lines[_line]));
    }

    protected virtual IEnumerator type(string text, float timeToClear = 1.5f)
    {
        _speaking = true;
        for (int i = 0; i < text.Length+1; i++)
        {
            display = text.Substring(0,i);
            _tmp.text = display;
            yield return new WaitForSeconds(0.1f);
        }
        _speaking = false;
        endBehavior();
        yield return new WaitForSeconds(timeToClear);
        _tmp.text = "";
    }

    protected abstract void endBehavior();
}
