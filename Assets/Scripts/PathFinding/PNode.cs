using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class PNode : MonoBehaviour
{
    public int _nodeNum;
    public bool _wall;
    public Vector2Int _pos;    
    public int _parentNode;

    // JPS 사용
    public bool _isStraight;
    public int _dir;
    public int _diag;

    
    public float F { get { return G + H; } } // G+H 전체 가중치
    public float G { get; set; } // 시작 노드 부터 현재 노드 까지 가중치
    public float H { get; set; } // 현재 노드 부터 도착 노드 까지 가중치

    public void Start()
    {
        _pos.x = (int)transform.position.x;
        _pos.y = (int)transform.position.y;

        if (gameObject.layer == LayerMask.NameToLayer("Wall"))
            _wall = true;
    }    
}
