using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSpeaker : DialogSpeaker
{
    protected void Update() {
        GetComponent<Animator>().SetBool("Talking", _speaking);
        transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("Talking", _speaking);
    }

    protected override void speak()
    {
        StartCoroutine(type(_lines[_line]));
        _line++;
        //make animations restart or smth i dont care
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    protected override void endBehavior()
    {
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
    
}
