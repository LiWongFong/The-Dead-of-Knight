using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Player;

    public float MaxChargeTime = 1f;
    public float MaxDashDistance = 4f;
    public float Momentum = 6f;

    private Rigidbody2D _body;
    private int _layermask;
    private bool _clicked = false;
    private bool _jump = false;
    private bool _reset = true;
    private float _startTime;
    private float _dashDistance;

    private Animator _anim;

    private TrailRenderer _trail;

    private Indicator _indi;

    private GameObject _sword;

    private void Awake() {
        if (Player ==  null)
        {
            Player = this;
            DontDestroyOnLoad(this);
        } else if (Player != this)
        {
            Destroy(gameObject);
        }    
    }

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _body = GetComponent<Rigidbody2D>();
        _trail = GetComponent<TrailRenderer>();
        _indi = GameObject.Find("Image").GetComponent<Indicator>();
        _sword = transform.GetChild(0).gameObject;
        _layermask = 1 << gameObject.layer;
    }

    private void Update()
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var direction = (worldPosition - transform.position.AsVector2());
        direction.Normalize();
        Vector3 direct3 = direction;
        _indi.MoveToClickPoint(transform.position + (direct3 * 1.0f));

        if (Input.GetMouseButtonDown(0))
        {
            _startTime = Time.time;
            _clicked = true;
        }

        if (((_clicked && Time.time - _startTime >= MaxChargeTime) || Input.GetMouseButtonUp(0)) && _reset)
        {      
            _dashDistance = (Time.time - _startTime) * (MaxDashDistance/MaxChargeTime);
            if (_dashDistance > MaxDashDistance) {_dashDistance = MaxDashDistance;}
            _jump = true; 
            _reset = false;
            _clicked = false;
            _anim.ResetTrigger("Charging");
            _anim.SetTrigger("Dash");
            _sword.SetActive(false);
            _sword.GetComponent<Animator>().ResetTrigger("Shine");
            _sword.GetComponent<Animator>().SetTrigger("Shine end");
            Debug.Log("Pressed left click.");
        }

        if (_clicked && _reset)
        {
        _anim.SetTrigger("Charging");
        _sword.SetActive(true);
        _sword.GetComponent<Animator>().ResetTrigger("Shine end");
        _sword.GetComponent<Animator>().SetTrigger("Shine");
        }

        _sword.transform.up = -1*(worldPosition - _sword.transform.position.AsVector2()).Rotate(45);

        if (_clicked) {_anim.SetFloat("Facing",direct3.x);}

        if (!_reset) {_startTime = Time.time;}
    }

    private void FixedUpdate()
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (_jump) 
        {
            _jump = false;
            StartCoroutine(dLine());
            Vector2 launch = (worldPosition - transform.position.AsVector2());
            launch.Normalize();
            RaycastHit2D hit = Physics2D.Raycast(transform.position.AsVector2(), launch, _dashDistance, ~_layermask);
            Vector2 blinkEnd;
            if (hit.collider == null)
            {
                blinkEnd = transform.position.AsVector2()+(launch*_dashDistance);
            } else if (hit.collider.tag == "Trans")
            {
                blinkEnd = transform.position.AsVector2()+(launch*_dashDistance);
                hit.collider.gameObject.GetComponent<Trans>().Trigger();
            } else
            {
                blinkEnd = transform.position.AsVector2()+(launch*hit.distance);
            }
            _body.MovePosition(blinkEnd);
            _body.velocity = (launch*Momentum);
        }

        if (_body.velocity.y == 0f) {_reset = true;}
    }

    IEnumerator dLine()
    {
        _trail.emitting = true;
        yield return null;
        _trail.emitting = false;
    }

    public float getVelocity()
    {
        return _body.velocity.y;
    }
}
