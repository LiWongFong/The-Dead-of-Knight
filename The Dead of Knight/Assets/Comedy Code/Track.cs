using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public class Track : MonoBehaviour
{
    private Rigidbody2D body;
    private Vector2 worldPosition;
    private Vector2 launch;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        launch = worldPosition - transform.position.AsVector2();
    }
}
