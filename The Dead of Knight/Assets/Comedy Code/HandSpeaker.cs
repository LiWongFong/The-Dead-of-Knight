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
        StartCoroutine(type(_lines,0,2));
        //make animations restart or smth i dont care
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    protected IEnumerator type(string[] text, int startingLine, int lineAmount)
    {
        print(startingLine);
        _speaking = true;
        for (int i = 0; i < lineAmount; i++)
        {
            for (int j = 0; j < text[startingLine+i].Length+1; j++)
            {
                display = text[startingLine+i].Substring(0,j);
                _tmp.text = display;
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.5f);
        }
        _speaking = false;
        endBehavior();
        yield return new WaitForSeconds(1.5f);
        _tmp.text = "";
    }

    protected override void endBehavior()
    {
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
    
}
