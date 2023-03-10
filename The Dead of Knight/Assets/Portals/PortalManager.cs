using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public class PortalManager : MonoBehaviour
{
    public static PortalManager pManager;

    [SerializeField]
    private List<GameObject> Portals = new List<GameObject>();

    [SerializeField]
    private bool Mirror = false;

    [SerializeField]
    private Rigidbody2D PlayerBody;

    public bool _teleporting = false;

    private void Awake() {
        if (pManager ==  null)
        {
            pManager = this;
            DontDestroyOnLoad(this);
        } else if (pManager != this)
        {
            Destroy(gameObject);
        }    
    }

    private void Start() {
        //PlayerBody = PlayerManager.Player.GetComponent<Rigidbody2D>();
    }

    public void tele(int ID, Vector2 bodyVel)
    {
        GameObject enter = Portals[ID];
        int t = ID == 0 ? 1 : 0 ;
        GameObject exit = Portals[t];

        float Angle = Vector2.SignedAngle(enter.transform.up,exit.transform.up);
        Debug.Log(Angle);

        Debug.Log(bodyVel);
        bodyVel = bodyVel.Rotate(Angle);
        Debug.Log(bodyVel);
        bodyVel *= -1;
        
        if (Mirror)
        {
            float reflectAngle = Vector2.SignedAngle(bodyVel,exit.transform.up);
            bodyVel = bodyVel.Rotate(reflectAngle*2);
        }
        Debug.Log(bodyVel);

        PlayerBody.MovePosition(exit.transform.position.AsVector2());
        PlayerBody.velocity = bodyVel;

        StartCoroutine(refresh());
    }

    IEnumerator refresh()
    {
        yield return new WaitForSeconds(0.5f);
        _teleporting = false;
    }

}
