using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public int ID;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !PortalManager.pManager._teleporting)
        {
            PortalManager.pManager._teleporting = true;
            Rigidbody2D body = other.gameObject.GetComponent<Rigidbody2D>();
            PortalManager.pManager.tele(ID,body.velocity);
        }
    }
}
