using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class Editor 
{
#if UNITY_EDITOR
    [MenuItem("Map/CollisionMap")]
    public static void MapCollisionInfo()
    {
        GameObject go = Resources.Load<GameObject>("Prefabs/MapRoot");

        int maxX =0;
        int maxY =0;
        int width = 0;
        foreach (PNode node in go.transform.GetComponentsInChildren<PNode>())
        {
            if (node._pos.x > maxX)
                maxX = node._pos.x;

            if (node._pos.y > maxY)
                maxY = node._pos.y;
            
        }
    }
#endif
}
