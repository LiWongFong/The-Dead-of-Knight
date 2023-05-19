using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hideCursor : MonoBehaviour
{
    private void Awake() {
        #if UNITY_STANDALONE
            Cursor.visible = false;
        #endif
    }
}
