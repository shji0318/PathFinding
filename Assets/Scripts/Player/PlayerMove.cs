using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

public class PlayerMove : MonoBehaviour
{
    MouseInput _mouse = MouseInput.None;
    void Update()
    {
        ChangeInput();
        MouseEvent();
    }

    public void ChangeInput()
    {
        _mouse = MouseInput.None;

        if (Input.GetMouseButtonDown(0))
            _mouse = MouseInput.LeftClick;
        else if (Input.GetMouseButtonDown(1))
            _mouse = MouseInput.RightClick;
    }
    public void MouseEvent()
    {
        switch(_mouse)
        {
            case MouseInput.LeftClick:
                Move();
                break;
            case MouseInput.RightClick:
                ChangeWall();
                break;
            case MouseInput.None:
                break;
        }
    }
    public void Move()
    {
        if (_mouse != MouseInput.LeftClick)
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

    public void ChangeWall()
    {
        if (_mouse != MouseInput.RightClick)
            return;

        Collider2D co = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition));

        if (co == null)
        {
            Debug.Log("해당 지점에 노드가 없습니다 :(");
            return;
        }
            

        PNode node = co.gameObject.GetComponent<PNode>();

        node._wall = !node._wall;

        if (node._wall == false)
            MapManager.Map.SRDic[node._nodeNum].color = Color.white;
        else
            MapManager.Map.SRDic[node._nodeNum].color = Color.black;
    }
}
