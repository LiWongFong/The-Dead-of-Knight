using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public class Dash : MonoBehaviour
{
    public float dashDistance = 10f;
    public float momentum = 5f;

    private Rigidbody2D body;
    private bool clicked = false;
    private bool reset = true;
    private Vector2 worldPosition;
    private Vector2 launch;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
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
        worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (clicked) 
        {
            clicked = false;
            launch = worldPosition - transform.position.AsVector2();
            launch.Normalize();
            body.MovePosition(transform.position.AsVector2()+(launch*dashDistance));
            body.velocity = (launch*momentum);
        }

        if (body.velocity.y == 0.0)
        {
            reset = true;
            body.velocity = Vector2.zero;
        }
    }
}
