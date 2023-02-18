using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static T FindOverlap<T>(Vector2Int pos) where T : UnityEngine.Object
    {
        Collider2D co = Physics2D.OverlapPoint(pos);
        
        if (co == null)
            return null;

        T component = co.GetComponent<T>();

        if (component == null)
            return null;

        return component;
    }

    public static T FindChild<T>(this GameObject go, string name, bool recursive) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        foreach( T component in go.GetComponentsInChildren<T>(recursive))
        {
            if (component.name == name)
                return component;
        }

        return null;
    }
}
