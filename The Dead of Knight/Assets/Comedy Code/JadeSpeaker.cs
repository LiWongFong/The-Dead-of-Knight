using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JadeSpeaker : DialogSpeaker
{
    [SerializeField]
    private AudioClip[] Noises;

    private bool saidIntro = false;
    private bool solvedRiddle = false;
    private int timesTalkedTo = 3;

    private System.Random rnd = new System.Random();
    private AudioSource _audio;

    private void Awake() {
        _audio = GetComponent<AudioSource>();
    }

    protected void Update() {
        GetComponent<Animator>().SetBool("Talking", _speaking);
    }

    protected override void speak()
    {
        if (!saidIntro)
        {
            print(saidIntro);
            StartCoroutine(type(_lines,0,6));
            saidIntro = true;
        } else if (timesTalkedTo == 3)
        {
            if (solvedRiddle)
            {
                StartCoroutine(type(_lines,25,2));
            } else
            {
                StartCoroutine(type(_lines,7,2));
            }
            timesTalkedTo = 0;
        } else
        {
            switch (rnd.Next(1,4))
            {
                case 1:
                    StartCoroutine(type(_lines,10,4));
                    break;
                case 2:
                    StartCoroutine(type(_lines,15,4));
                    break;
                case 3:
                    StartCoroutine(type(_lines,20,4));
                    break;
                default:
                    break;
            }
            timesTalkedTo++;
        }
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

                _audio.PlayOneShot(Noises[rnd.Next(0,7)]);
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.5f);
        }
        _speaking = false;
        endBehavior();
        yield return new WaitForSeconds(1.5f);
        _tmp.text = "";
    }

    protected override void endBehavior() {}
}
