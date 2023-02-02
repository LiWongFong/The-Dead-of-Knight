using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public class Dash : MonoBehaviour
{
    public float dashDelta = 4f;
    public float momentum = 6f;

    private Rigidbody2D body;
    private int layerMask;
    private bool clicked = false;
    private bool jump = false;
    private bool reset = true;
    private float startTime;
    private float dashDistance;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
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
            Vector2 launch = (worldPosition - transform.position.AsVector2());
            launch.Normalize();
            RaycastHit2D hit = Physics2D.Raycast(transform.position.AsVector2(), launch, dashDistance, ~layerMask);
            if (hit.collider == null)
            {
                body.MovePosition(transform.position.AsVector2()+(launch*dashDistance));
                body.velocity = (launch*momentum);
            } else
            {
                body.velocity = (launch*10);
            }
        }

        if (body.velocity.y == 0.0)
        {
            reset = true;
            body.velocity = Vector2.zero;
        }
    }
}
