using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveSpeaker : DialogSpeaker
{
    private bool saidIntro = false;
    private bool sleeping = false;

    private int[] protoLinesLeft = {1,2,3};
    private List<int> LinesLeft; 

    private void Awake() {
        LinesLeft = new List<int>(protoLinesLeft);
    }

    protected void Update() {
        GetComponent<Animator>().SetBool("Talking", _speaking);
    }

    protected override void speak()
    {
        if (!sleeping)
        {
            if (!saidIntro)
            {
                StartCoroutine(type(_lines,0,5));
                saidIntro = true;
            } else if (LinesLeft.Count != 0)
            {
                print(LinesLeft.Count);
                switch (LinesLeft[Random.Range(0,LinesLeft.Count-1)])
                {
                    case 1:
                        StartCoroutine(type(_lines,6,3));
                        LinesLeft.Remove(1);
                        break;
                    case 2:
                        StartCoroutine(type(_lines,10,3));
                        LinesLeft.Remove(2);
                        break;
                    case 3:
                        StartCoroutine(type(_lines,14,3));
                        LinesLeft.Remove(3);
                        break;
                    default:
                        break;
                }
            } else
            {
                StartCoroutine(type(_lines,18,3));
                sleeping = true;
            }
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
