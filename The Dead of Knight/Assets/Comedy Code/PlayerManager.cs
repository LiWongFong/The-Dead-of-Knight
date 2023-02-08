using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Player;

    public float DashDelta = 4f;
    public float Momentum = 6f;

    private Rigidbody2D _body;
    private int _layermask;
    private bool _clicked = false;
    private bool _jump = false;
    private bool _reset = true;
    private float _startTime;
    private float _dashDistance;

    private Animator _anim;
    private float _prevX = 0f;

    private TrailRenderer _trail;

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
        _layermask = 1 << gameObject.layer;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startTime = Time.time;
            _clicked = true;
        }

        if (((_clicked && Time.time - _startTime >= 1.0f) || Input.GetMouseButtonUp(0)) && _reset)
        {      
            _dashDistance = (Time.time - _startTime) * DashDelta;
            if (_dashDistance > 1.0f*DashDelta) {_dashDistance = 1.0f*DashDelta;}
            _jump = true; 
            _reset = false;
            _clicked = false;
            Debug.Log("Pressed left click.");
        }

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

        if (transform.position.x - _prevX > 0f) {_anim.SetFloat("AniHorizontal", 1);}
        else if (transform.position.x - _prevX < 0f) {_anim.SetFloat("AniHorizontal", -1);}
        _prevX = transform.position.x;
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
