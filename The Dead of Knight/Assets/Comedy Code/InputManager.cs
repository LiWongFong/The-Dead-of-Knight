using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager iManager;

    private Rigidbody2D _body;
    private PlayerInput _input;

    private Vector2 _storedMomentum;

    private void Awake() {
        if (iManager ==  null)
        {
            iManager = this;
            DontDestroyOnLoad(this);
        } else if (iManager != this)
        {
            Destroy(gameObject);
        }    
    }

    private void Start() {
        _body = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
    }



    public void Unpause()
    {
        _input.SwitchCurrentActionMap("Gameplay");
        PlayerManager.Player.enabled = true;
        _body.constraints = RigidbodyConstraints2D.FreezeRotation;
        _body.velocity = _storedMomentum;
        GameObject.Find("Menu").GetComponent<Menu>().UnPause();
    }
}
