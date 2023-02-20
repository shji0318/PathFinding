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

    // JPS ���
    public bool _isStraight;
    public int _dir;
    public int _diag;

    [field : SerializeField]
    public float F { get { return G + H; } } // G+H ��ü ����ġ
    [field: SerializeField]
    public float G { get; set; } // ���� ��� ���� ���� ��� ���� ����ġ
    [field: SerializeField]
    public float H { get; set; } // ���� ��� ���� ���� ��� ���� ����ġ

    public void Start()
    {
        _pos.x = (int)transform.position.x;
        _pos.y = (int)transform.position.y;

        if (gameObject.layer == LayerMask.NameToLayer("Wall"))
            _wall = true;
    }    
}
