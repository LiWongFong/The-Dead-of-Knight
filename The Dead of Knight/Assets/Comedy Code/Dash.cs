using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public class Dash : MonoBehaviour
{
    public float dashDistance = 10f;
    public float momentum = 5f;

    private Rigidbody2D body;
    private int layerMask;
    private bool clicked = false;
    private bool reset = true;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        layerMask = 1 << gameObject.layer;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && reset)
        {
            clicked = true; 
            reset = false;
            Debug.Log("Pressed left click.");
        }
    }

    private void FixedUpdate()
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (clicked) 
        {
            clicked = false;
            Vector2 launch = worldPosition - transform.position.AsVector2();
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
