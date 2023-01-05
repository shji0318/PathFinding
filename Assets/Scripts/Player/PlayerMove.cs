using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{   

    void Update()
    {
        Move();
    }

    public void Move()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        if (MapManager.Map.DoMove)
            return;

        Collider2D co = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition));

        if (co == null)
            return;

        PNode node = co.gameObject.GetComponent<PNode>();        

        if(node == null || node._wall==true || node == MapManager.Map.NowNode)
        {
            Debug.Log("이동할 수 없는 노드입니다. :(");
            return;
        }

        MapManager.Map.EndNode = node;

        StartCoroutine(MapManager.Map.Move(MapManager.Map.NowNode, MapManager.Map.EndNode));
        
    }
}
