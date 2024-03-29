using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public class Meatpie : DialogSpeaker
{
    [SerializeField]
    private float _swimSpeed;
    [SerializeField]
    private Transform[] children;
    [SerializeField]
    private AudioClip[] Noises;

    private Vector2 _start;
    private Vector2 _end;
    private Vector2 _target;
    private bool facingLeft = true;
    private bool turning = false;
    private float turnTime = 0.8333f;
    private float _timer = 0f;
    private bool saidIntro = false;

    private Animator _anim;
    private AudioSource _audio;

    private void Awake() {
        foreach (var child in children)
        {
            child.SetParent(null, true);
        }

        _start = children[0].position.AsVector2();
        _end = children[1].position.AsVector2();

        _target = _end;
        _target.y += (Random.Range(-100,100)/200f);

        _anim = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
    }

    private void FixedUpdate() {
        if (!turning && !_speaking)
        {
            transform.position = Vector2.MoveTowards(transform.position.AsVector2(), _target, _swimSpeed);
            if (transform.position.AsVector2().x == _target.x)
            {
                _target = _target.x == _end.x ? _start : _end;
                _target.y += (Random.Range(-100,100)/200f);

                if (facingLeft) {_anim.SetTrigger("Turn Right");}
                else {_anim.SetTrigger("Turn Left");}
                facingLeft = !facingLeft;

                turning = true;
            }
        } else if (turning && !_speaking)
        {
            if (_timer >= turnTime)
            {
                turning = false;
                _timer = 0;
                _anim.ResetTrigger("Turn Right");
                _anim.ResetTrigger("Turn Left");
            }
            _timer += Time.deltaTime;
        }
    }

    protected override void speak()
    {
        if (!saidIntro)
        {
            StartCoroutine(type(_lines,0,3));
            saidIntro = true;
        } else
        {
            switch (Random.Range(1,4))
                {
                    case 1:
                        StartCoroutine(type(_lines,4,1));
                        break;
                    case 2:
                        StartCoroutine(type(_lines,6,1));
                        break;
                    case 3:
                        StartCoroutine(type(_lines,8,1));
                        break;
                    case 4:
                        StartCoroutine(type(_lines,10,1));
                        break;
                    default:
                        break;
                }
        }
    }

    protected IEnumerator type(string[] text, int startingLine, int lineAmount, float timeToClear = 1.5F)
    {
        _speaking = true;

        for (int i = 0; i < 10; i++)
        {
            transform.position = Vector2.MoveTowards(transform.position.AsVector2(), _target, 0.08f);
            yield return new WaitForSeconds(0.833f/10f);
        }

        var dir = PlayerManager.Player.transform.position - transform.position;
        if (dir.x > 0 && facingLeft)
        {
            _anim.SetTrigger("Turn Right");
            yield return new WaitForSeconds(0.833f);
            facingLeft = !facingLeft;
        } else if (dir.x < 0 && !facingLeft)
        {
            _anim.SetTrigger("Turn Left");
            yield return new WaitForSeconds(0.833f);
            facingLeft = !facingLeft;
        }
        _anim.ResetTrigger("Turn Right");
        _anim.ResetTrigger("Turn Left");
        
        var facing = dir.x/Mathf.Abs(dir.x);
        facing++;
        facing /= 2;
        _anim.SetFloat("Facing Left", facing);
        _anim.SetBool("Speaking", true);

        //Speaking part
        for (int i = 0; i < lineAmount; i++)
        {
            for (int j = 0; j < text[startingLine+i].Length+1; j++)
            {
                display = text[startingLine+i].Substring(0,j);
                _tmp.text = display;

                _audio.PlayOneShot(Noises[Random.Range(0,4)]);
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.5f);
        }

        _anim.SetBool("Speaking", false);

        var resetDir = _target - transform.position.AsVector2();
        if (resetDir.x > 0 && facingLeft)
        {
            _anim.SetTrigger("Turn Right");
            yield return new WaitForSeconds(0.833f);
            facingLeft = !facingLeft;
        } else if (resetDir.x < 0 && !facingLeft)
        {
            _anim.SetTrigger("Turn Left");
            yield return new WaitForSeconds(0.833f);
            facingLeft = !facingLeft;
        }
        _anim.ResetTrigger("Turn Right");
        _anim.ResetTrigger("Turn Left");

        _speaking = false;
        endBehavior();
        yield return new WaitForSeconds(timeToClear);
        _tmp.text = "";
    }

    protected override void endBehavior() {}
}
