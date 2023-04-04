using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtensionMethods
{
    public static class Extensions
    {
        public static Vector2 AsVector2(this Vector3 _v)
        {
            return new Vector2(_v.x, _v.y);
        }

        public static Vector2 Rotate(this Vector2 v, float degrees) {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
            
            float tx = v.x;
            float ty = v.y;
            v.x = (cos * tx) - (sin * ty);
            v.y = (sin * tx) + (cos * ty);
            return v;
        }

        public static int boolToInt(bool val)
        {
            if (val)
                return 1;
            else
                return 0;
        }

        public static bool intToBool(int val)
        {
            if (val != 0)
                return true;
            else
                return false;
        }
    }
    
}

