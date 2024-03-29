using System;
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
    public Vector2 Momentum = new Vector2(12f,12f);
    public float HobbleSpeed = 1f;

    [Header("Control Things")]
    [SerializeField]
    private InputActionReference Jump;
    [SerializeField]
    private InputActionReference Joy;
    [SerializeField]
    private InputActionReference Position;
    [SerializeField]
    private InputActionReference Hobble;

    [SerializeField]
    private AudioClip[] SFX;

    [HideInInspector]
    public Vector2 StoredMomentum; 
    
    private int _layermask;
    private bool _clicked = false;
    private bool _jump = false;
    private bool _reset = false;
    private bool _falling = false;
    private bool _hobble = true;
    private float _startTime;
    private float _dashDistance;
    private bool _stuck = false;
    private Vector2 _direction;
    private float _prevYVelocity = 0;
    private bool IsAnimationReset = true;

    private List<ContactPoint2D> _points = new List<ContactPoint2D>();
    private ContactFilter2D _filter= new ContactFilter2D().NoFilter();


    private Rigidbody2D _body;

    private Animator _anim;

    private TrailRenderer _trail;

    private Indicator _indi;

    private GameObject _sword;

    private PlayerInput _input;

    private AudioSource _audio;

    public Vector2 Velocity {get => _body.velocity; set => _body.velocity = value;}


    private void Awake() {
        if (Player ==  null)
        {
            Player = this;
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
        _audio = GetComponent<AudioSource>();

        int playerLayer = 1 << gameObject.layer;
        int defaultLayer = 1 << 2;
        int edgecase = 1 << 6;
        _layermask = defaultLayer ^ playerLayer ^ edgecase;

        _filter.useTriggers = false;

        _body.transform.position = DataManager.dManager.Position;
        _body.velocity = DataManager.dManager.Velocity;
        _anim.SetFloat("Facing", _body.velocity.x);

        _input.actions.LoadBindingOverridesFromJson(DataManager.dManager.Controls); 
    }

    private void OnClick(InputAction.CallbackContext ctx)
    {
        _startTime = Time.time;
        _clicked = true;
        print("Left Pressed");
    }

    private void OnRelease(InputAction.CallbackContext ctx) {
        print("Left let go");preJump();}

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
            IsAnimationReset = false;

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
            _hobble = false;
            StartCoroutine(falling());
            StartCoroutine(dLine());
            Vector2 launch = _direction;
            launch.Normalize();
            RaycastHit2D hit = Physics2D.Raycast(transform.position.AsVector2(), launch, _dashDistance, ~_layermask);
            Debug.DrawRay(transform.position, launch*_dashDistance, Color.magenta, 10f);
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
                        blinkEnd += (hit.normal * collisionFix(blinkEnd,hit.normal));
                        StartCoroutine(stick(launch,hit.normal));
                        break;
                    default:
                        blinkEnd = transform.position.AsVector2()+(launch*hit.distance);
                        blinkEnd += (hit.normal * collisionFix(blinkEnd,hit.normal));
                        break;
                }
            }
            
            print(_dashDistance);

            _body.MovePosition(blinkEnd);
            print("Moved to: "+blinkEnd);
            _body.velocity = (launch*Momentum);

            _audio.PlayOneShot(SFX[0]);
        }

        if (_body.velocity.y == 0f && _prevYVelocity == 0f && _points.Count == 1)
        {
            var distanceFromPointToCenter = transform.position.x - _points[0].point.x;
            float normalizedDistanceFromPointToCenter = distanceFromPointToCenter/MathF.Abs(distanceFromPointToCenter);
            _body.velocity += new Vector2(normalizedDistanceFromPointToCenter,0);
        }

        if (_body.velocity.y == 0f && _prevYVelocity == 0f && !_stuck && _points.Count >= 2 && IsAnimationReset)
        {
            if (!_reset && Hobble.action.ReadValue<float>() == 0) {_audio.PlayOneShot(SFX[1]);}
            _reset = true;
            _falling = false;
            _hobble = true;
            _anim.SetTrigger("Ground");
            _anim.ResetTrigger("Dash");
            //Debug.Log("Reset");
            _body.velocity = new Vector2(0,0);
            
        }

        if (!_clicked && _hobble && Hobble.action.ReadValue<float>() != 0)
        {
            _reset = false;
            _body.velocity = new Vector2(Hobble.action.ReadValue<float>(), 0) * HobbleSpeed;
        }

        if (!_body.IsTouching(_filter))
        {
            _hobble = false;
            _falling = true;
        }

        _prevYVelocity = _body.velocity.y;
    }

    private float collisionFix(Vector2 endPosition, Vector2 normal)
    {
        float mag = 0;
        
        Vector2 fullSize = GetComponent<BoxCollider2D>().size;
        float diagAngle = Mathf.Atan((fullSize.y/2)/(fullSize.x/2));

        float theta = Vector2.Angle(normal,new Vector2(1,0));
        if (theta > 90) {theta = 180 - theta;}

        if (theta > diagAngle)
        {
            //c = y/sin(theta)
            mag = (fullSize.y/2)/Mathf.Sin(theta);
        }
        else if (theta < diagAngle)
        {
            //c = x/cos(theta)
            mag = (fullSize.x/2)/Mathf.Cos(theta);
        }
        else
        {
            // x^2 + y^2 = c^2
            mag = Mathf.Sqrt(((fullSize.y/2)*(fullSize.y/2))+((fullSize.x/2)*(fullSize.x/2)));
        }

        return mag;
    }

    IEnumerator stick(Vector2 launch, Vector2 normal)
    {
        yield return new WaitForFixedUpdate();
        Debug.Log("Stuck");
        _body.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(0.5f);
        _body.constraints = RigidbodyConstraints2D.FreezeRotation;

        StartCoroutine(dLine());

        Vector2 antiNormal = normal.Rotate(90);
        float reflectAngle = Vector2.SignedAngle(launch,antiNormal);
        Vector2 newLaunch = launch.Rotate(reflectAngle*2);

        RaycastHit2D rehit = Physics2D.Raycast(transform.position.AsVector2(), newLaunch, _dashDistance/2, ~_layermask);
        Debug.DrawRay(transform.position, newLaunch*_dashDistance/2, Color.blue, 10f);
        print(_dashDistance/2);

        Vector2 bounceEnd;
        bool hitTransAfterWall = false;
        if (rehit.collider == null)
        {
            bounceEnd = transform.position.AsVector2()+(newLaunch*_dashDistance/2);
        } else
        {
            switch (rehit.collider.tag)
            {
                case "Trans":
                    bounceEnd = transform.position.AsVector2()+(newLaunch*rehit.distance);
                    StartCoroutine(trans(newLaunch, _dashDistance - rehit.distance));
                    hitTransAfterWall = true;
                    break;
                default:
                    bounceEnd = transform.position.AsVector2()+(newLaunch*rehit.distance);
                    bounceEnd += (rehit.normal * collisionFix(bounceEnd,rehit.normal));
                    break;
            }
        }

        _body.MovePosition(bounceEnd);
        print("Moved to: "+bounceEnd);
        _body.velocity = (newLaunch*Momentum);

        if (!hitTransAfterWall) {_stuck = false;}
    }

    IEnumerator trans(Vector2 launch, float _distance)
    {
        yield return new WaitForFixedUpdate();
        _body.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(0.5f);
        _body.constraints = RigidbodyConstraints2D.FreezeRotation;

        StartCoroutine(dLine());

        RaycastHit2D continuedJump = Physics2D.Raycast(transform.position.AsVector2(), launch, _distance, ~(_layermask^(1 << 7)));
        Vector2 endPosition;
        bool hitWallAfterTrans = false;
        
        print(continuedJump.collider);
        Debug.DrawRay(transform.position, launch*_distance, Color.green, 10f);

        if (continuedJump.collider == null)
        {
            endPosition = transform.position.AsVector2()+(launch*_distance);
        } else
        {
            switch (continuedJump.collider.tag)
            {
                
                case "Wall":
                    endPosition = transform.position.AsVector2()+(launch*continuedJump.distance);
                    StartCoroutine(stick(launch, continuedJump.normal));
                    hitWallAfterTrans = true;
                    endPosition += (continuedJump.normal * collisionFix(endPosition,continuedJump.normal));
                    break;  
                default:
                    endPosition = transform.position.AsVector2()+(launch*continuedJump.distance);
                    endPosition += (continuedJump.normal * collisionFix(endPosition,continuedJump.normal));
                    break;
            }
        }

        _body.MovePosition(endPosition);
        print("Moved to: "+endPosition);
        _body.velocity = (launch*Momentum);

        if (!hitWallAfterTrans) {_stuck = false;}
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

    public void AnimFinished() {IsAnimationReset = true;}
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

    private void OnDestroy()
    {
        DataManager.dManager.Controls = _input.actions.SaveBindingOverridesAsJson();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        other.GetContacts(_points);
        foreach (var point in _points)
        {
            var normal = point.normal*10;
            Debug.DrawRay(point.point, normal, Color.red, 1f);
        } 
    }

    [ContextMenu("jhgbakrthbkhsejdrkawreyhsgduawgerygiuk")]
    private void test()
    {
        print(_reset);
        print(_clicked);
        print(_points.Count);
        print(IsAnimationReset);
    }
}