using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Wet");
        PlayerManager.Player.MaxChargeTime *= 2.5f;
        PlayerManager.Player.setGravity(1);
    }

    private void OnTriggerExit(Collider other) {
        Debug.Log("Dry");
        PlayerManager.Player.MaxChargeTime /= 2.5f;
        PlayerManager.Player.setGravity(5);
    }
}
