using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private GameObject _dim;

    [Space]

    [SerializeField]
    private InputActionReference Unpause;

    public void pause()
    {
        _dim.SetActive(true);
    }

    private void xcvbnmUnpause()
    {
        _dim.SetActive(false);
        PlayerManager.Player.enabled = true;
        //nut in my, i mean dont
    }
}
