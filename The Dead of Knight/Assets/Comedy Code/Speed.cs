using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed : MonoBehaviour
{
    [SerializeField]
    private float _speedMultiplier;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player")
        {
            PlayerManager.Player.Velocity = _speedMultiplier * PlayerManager.Player.Velocity;
        }
    }
}
