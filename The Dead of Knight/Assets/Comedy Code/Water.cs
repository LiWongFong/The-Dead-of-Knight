using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerManager.Player.MaxChargeTime *= 2.5f;
        PlayerManager.Player.setGravity(1);
    }

    private void OnTriggerExit2D(Collider2D other) {
        PlayerManager.Player.MaxChargeTime /= 2.5f;
        PlayerManager.Player.setGravity(5);
    }
}
