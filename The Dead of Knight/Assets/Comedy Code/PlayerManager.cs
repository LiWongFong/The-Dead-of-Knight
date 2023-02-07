using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Player;

    public float dashDelta = 4f;
    public float momentum = 6f;

    private Rigidbody2D body;
    private int layerMask;
    private bool clicked = false;
    private bool jump = false;
    private bool reset = true;
    private float startTime;
    private float dashDistance;

    private Animator anim;
    private float prevX = 0f;

    private TrailRenderer trail;

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
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        trail = GetComponent<TrailRenderer>();
        layerMask = 1 << gameObject.layer;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTime = Time.time;
            clicked = true;
        }

        if (((clicked && Time.time - startTime >= 1.0f) || Input.GetMouseButtonUp(0)) && reset)
        {      
            dashDistance = (Time.time - startTime) * dashDelta;
            if (dashDistance > 1.0f*dashDelta) {dashDistance = 1.0f*dashDelta;}
            jump = true; 
            reset = false;
            clicked = false;
            Debug.Log("Pressed left click.");
        }

        if (!reset) {startTime = Time.time;}
    }

    private void FixedUpdate()
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (jump) 
        {
            jump = false;
            StartCoroutine(dLine());
            Vector2 launch = (worldPosition - transform.position.AsVector2());
            launch.Normalize();
            RaycastHit2D hit = Physics2D.Raycast(transform.position.AsVector2(), launch, dashDistance, ~layerMask);
            Vector2 blinkEnd;
            if (hit.collider == null)
            {
                blinkEnd = transform.position.AsVector2()+(launch*dashDistance);
            } else if (hit.collider.tag == "Trans")
            {
                blinkEnd = transform.position.AsVector2()+(launch*dashDistance);
                hit.collider.gameObject.GetComponent<Trans>().Trigger();
            } else
            {
                blinkEnd = transform.position.AsVector2()+(launch*hit.distance);
            }
            body.MovePosition(blinkEnd);
            body.velocity = (launch*momentum);
        }

        if (body.velocity.y == 0f) {reset = true;}

        if (transform.position.x - prevX > 0f) {anim.SetFloat("AniHorizontal", 1);}
        else if (transform.position.x - prevX < 0f) {anim.SetFloat("AniHorizontal", -1);}
        prevX = transform.position.x;
    }

    IEnumerator dLine()
    {
        trail.emitting = true;
        yield return null;
        trail.emitting = false;
    }

    public float getVelocity()
    {
        return body.velocity.y;
    }
}
