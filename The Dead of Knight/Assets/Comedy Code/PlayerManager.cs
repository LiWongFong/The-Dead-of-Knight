using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Player;

    public float MaxChargeTime = 1f;
    public float MaxDashDistance = 4f;
    public float VerticalMomentum = 12f;
    public float HorizontalMomentum = 12f;

    [SerializeField]
    private InputActionReference Jump, Pause, Joy, Position;

    [HideInInspector]
    public Vector2 StoredMomentum; 
    
    private int _layermask;
    private bool _clicked = false;
    private bool _jump = false;
    private bool _reset = true;
    private bool _falling = false;
    private float _startTime;
    private float _dashDistance;
    private bool _stuck = false;
    private Vector2 _direction;


    private Rigidbody2D _body;

    private Animator _anim;

    private TrailRenderer _trail;

    private Indicator _indi;

    private GameObject _sword;

    private PlayerInput _input;

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

    private void OnEnable() {
        //_input.SwitchCurrentActionMap("Gameplay");
        //_body.constraints = RigidbodyConstraints2D.FreezeRotation;

        Jump.action.performed += OnClick;
        Jump.action.canceled += OnRelease;
    }

    private void OnDisable() {
        Jump.action.performed -= OnClick;
        Jump.action.canceled -= OnRelease;
    }

    // Start is called before the first frame update
    private void Start()
    {
        _anim = GetComponent<Animator>();
        _body = GetComponent<Rigidbody2D>();
        _trail = GetComponent<TrailRenderer>();
        _indi = GameObject.Find("Indicator").GetComponent<Indicator>();
        _sword = transform.GetChild(1).gameObject;
        _input = GetComponent<PlayerInput>();
        int playerLayer = 1 << gameObject.layer;
        int defaultLayer = 1 << 2;
        int edgecase = 1 << 6;
        _layermask = defaultLayer ^ playerLayer ^ edgecase;
    }

    private void OnClick(InputAction.CallbackContext ctx)
    {
        _startTime = Time.time;
        _clicked = true;
    }

    private void OnRelease(InputAction.CallbackContext ctx) {preJump();}

    private void preJump()
    {
        if (_reset && _clicked) 
        {
            _dashDistance = (Time.time - _startTime) * (MaxDashDistance/MaxChargeTime);
            if (_dashDistance > MaxDashDistance) {_dashDistance = MaxDashDistance;}
            _jump = true; 
            _reset = false;
            _clicked = false;

            _anim.ResetTrigger("Charging");
            _anim.SetTrigger("Dash");
            _anim.ResetTrigger("Ground");

            _sword.SetActive(false);
            _sword.GetComponent<Animator>().ResetTrigger("Shine");
            _sword.GetComponent<Animator>().SetTrigger("Shine end");

            Debug.Log("Pressed left click.");
        }
    }

    private void Update()
    {
        if (_input.currentControlScheme == "KB&M")
        {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Position.action.ReadValue<Vector2>());
        _direction = (worldPosition - transform.position.AsVector2());
        } else 
        {
            _direction = Joy.action.ReadValue<Vector2>();
        }
        _direction.Normalize();
        Vector3 direct3 = _direction;
        _indi.MoveToClickPoint(transform.position + (direct3 * 1.0f));

        if (Time.time - _startTime >= MaxChargeTime) {preJump();}

        if (_clicked && _reset)
        {
        _anim.SetTrigger("Charging");

        _sword.SetActive(true);
        _sword.GetComponent<Animator>().ResetTrigger("Shine end");
        _sword.GetComponent<Animator>().SetTrigger("Shine");
        }

        _sword.transform.up = -1*(_direction).Rotate(45);
 
        if (_clicked) {_anim.SetFloat("Facing",direct3.x);}

        if (!_reset) {_startTime = Time.time;}
    }

    private void FixedUpdate()
    {
        if (_jump) 
        {
            _jump = false;
            StartCoroutine(falling());
            StartCoroutine(dLine());
            Vector2 launch = _direction;
            launch.Normalize();
            RaycastHit2D hit = Physics2D.Raycast(transform.position.AsVector2(), launch, _dashDistance, ~_layermask);
            Vector2 blinkEnd;
            if (hit.collider == null)
            {
                blinkEnd = transform.position.AsVector2()+(launch*_dashDistance);
            } else
            {
                switch (hit.collider.tag)
                {
                    case "Trans":
                        blinkEnd = transform.position.AsVector2()+(launch*hit.distance);
                        StartCoroutine(trans(launch, _dashDistance - hit.distance));
                        _stuck = true;
                        break;
                    case "Wall":
                        blinkEnd = transform.position.AsVector2()+(launch*hit.distance);
                        _stuck = true;
                        StartCoroutine(stick(launch));
                        break;
                    default:
                        blinkEnd = transform.position.AsVector2()+(launch*hit.distance);
                        break;
                }
            }
            
            _body.MovePosition(blinkEnd);
            _body.velocity = (launch*new Vector2(HorizontalMomentum,VerticalMomentum));
        }

        if (_body.velocity.y == 0f && !_stuck)
        {
            _reset = true;
            _falling = false;
            _anim.SetTrigger("Ground");
            _anim.ResetTrigger("Dash");
            //Debug.Log("Reset");
            _body.velocity = new Vector2(0,0);
        }
    }

    IEnumerator stick(Vector2 launch)
    {
        yield return null;
        Debug.Log("Stuck");
        _body.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(0.5f);
        _body.constraints = RigidbodyConstraints2D.FreezeRotation;
        Vector2 newLaunch = launch * new Vector2(-1,1);
        RaycastHit2D rehit = Physics2D.Raycast(transform.position.AsVector2(), newLaunch, _dashDistance, ~_layermask);
        Vector2 bounceEnd;
        if (rehit.collider == null)
        {
            bounceEnd = transform.position.AsVector2()+(launch*_dashDistance);
        } else
        {
            switch (rehit.collider.tag)
            {
                case "Trans":
                    bounceEnd = transform.position.AsVector2()+(launch*rehit.distance);
                    StartCoroutine(trans(launch, _dashDistance - rehit.distance));
                    _stuck = true;
                    break;
                default:
                    bounceEnd = transform.position.AsVector2()+(launch*rehit.distance);
                    break;
            }
        }
        _body.MovePosition(bounceEnd);
        _body.velocity = launch*new Vector2(HorizontalMomentum,VerticalMomentum);
        yield return null;
        _stuck = false;
    }

    IEnumerator trans(Vector2 launch, float _distance)
    {
        yield return null;
        _body.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(0.5f);
        _body.constraints = RigidbodyConstraints2D.FreezeRotation;
        _body.MovePosition(transform.position.AsVector2()+(launch*_distance));
        _body.velocity = (launch*new Vector2(HorizontalMomentum,VerticalMomentum));
        yield return null;
        _stuck = false;
    }

    IEnumerator frozen(Vector2 v)
    {
        yield return new WaitForSeconds(0.5f);
        _body.constraints = RigidbodyConstraints2D.FreezeRotation;
        _body.velocity = v;
        yield return null;
        _stuck = false;
    }

    IEnumerator dLine()
    {
        _trail.emitting = true;
        yield return null;
        _trail.emitting = false;
    }

    IEnumerator falling()
    {
        yield return null;
        yield return null;
        _falling = true;
    }

    public void freeze()
    {
        _stuck = true;
        var v = _body.velocity;
        _body.constraints = RigidbodyConstraints2D.FreezeAll;
        StartCoroutine(frozen(v));
    }

    public float GetVelocityY()
    {
        return _body.velocity.y;
    }

    public float GetVelocityX()
    {
        return _body.velocity.x;
    }

    public void SetGravity(float gravity)
    {
        _body.gravityScale = gravity;
    }

    public bool IsFalling()
    {
        return _falling;
    }
    private void OnPause()
    {
        StoredMomentum = _body.velocity;
        _body.constraints = RigidbodyConstraints2D.FreezeAll;
        GameObject.Find("Pause Menu").GetComponent<PauseMenu>().Pause();
        //Menu is empty but need to be changed for the stupid fucking edge case
        _input.SwitchCurrentActionMap("Menu");
        Debug.Log(_input.currentActionMap);
        this.enabled = false;
    }

}