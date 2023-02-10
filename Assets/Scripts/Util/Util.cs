using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static T FindOverlap<T>(Vector2Int pos) where T : UnityEngine.Object
    {
        T component = Physics2D.OverlapPoint(pos).GetComponent<T>();

        if (component == null)
            return null;

        return component;
    }
}
