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
    }
}

