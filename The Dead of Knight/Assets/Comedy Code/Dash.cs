using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    public float dashDistance = 10f;
    public float momentum = 5f;

    private Rigidbody2D body;
    private bool clicked = false;
    private Vector3 worldPosition;
    private Vector2 launch;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            clicked = true; 
            Debug.Log("Pressed left click.");
        }
    }

    private void FixedUpdate()
    {
        worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (clicked) 
        {
            clicked = false;
            launch = (worldPosition - transform.position);
            launch.Normalize();
            body.MovePosition(launch*dashDistance);
            body.velocity = (launch*momentum);
        }
    }
}
