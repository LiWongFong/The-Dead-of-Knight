using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _dim, _core;

    public void Pause()
    {
        _dim.SetActive(true);
        _core.SetActive(true);
    }

    public void UnPause()
    {
        _dim.SetActive(false);
        _core.SetActive(false);

        PlayerManager.Player.gameObject.GetComponent<PlayerInput>().SwitchCurrentActionMap("Gameplay");
        PlayerManager.Player.enabled = true;
        PlayerManager.Player.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        PlayerManager.Player.gameObject.GetComponent<Rigidbody2D>().velocity = PlayerManager.Player.StoredMomentum;
    }
}
