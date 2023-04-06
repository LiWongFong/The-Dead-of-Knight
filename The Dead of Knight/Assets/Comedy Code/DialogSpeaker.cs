using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogSpeaker : MonoBehaviour
{
    [SerializeField]
    private TextAsset Dialog;

    [SerializeField]
    private TextMeshProUGUI _tmp;

    private bool _stay = false;
    
    private string[] _lines;
    private string display;
    private int _line = 0;
    private bool _speaking = false;

    private void Start()
    {
        _lines = Dialog.text.Split(
        new string[] { "\r\n", "\r", "\n" },
        StringSplitOptions.None
        );
    }

    private void Update() {
        GetComponent<Animator>().SetBool("Talking", _speaking);
        transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("Talking", _speaking);
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
        if (_stay && !_speaking) {speak();}
    }

    private void speak()
    {
        StartCoroutine(type(_lines[_line]));
        _line++;
        //make animations restart or smth i dont care
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    IEnumerator type(string text)
    {
        _speaking = true;
        for (int i = 0; i < text.Length; i++)
        {
            display = text.Substring(0,i);
            _tmp.text = display;
            yield return new WaitForSeconds(0.1f);
        }
        _speaking = false;
        endBehavior();
        yield return new WaitForSeconds(1.5f);
        _tmp.text = "";
    }

    private void endBehavior()
    {
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}
