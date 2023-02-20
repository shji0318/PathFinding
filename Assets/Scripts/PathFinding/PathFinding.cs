using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PathFinding // 추후 PathFinding 알고리즘을 교체할 일이 있을 경우 확장을 고려하여 부모 추상 클래스를 만듦
{
    public bool _isFind = false;
    public int _width; // 타일 크기가 모든 게임 동일하지 않기 때문에 변경 가능하게 하기 위함
    // ↑ → ↓ ←
    public Vector2Int[] _dir;
    // ↗ ↘ ↙ ↖
    public Vector2Int[] _diag;

    public float _dirWeight;
    public float _diagWeight;

    public void Init()
    {
        // 재정의한 width에 따른 가중치 초기화
        _dirWeight = _width;
        _diagWeight = (float)(_width*1.4); // 대각선을 제외한 두 변의 길이가 같은 직각 삼각형의 대각선에 길이는 [한 변 * 루트2(약 1.4)]의 값을 갖음

        //현재 노드 기준 상하좌우, 대각선 별 이동 좌표
        _dir = new Vector2Int[] 
        {
            new Vector2Int(0, _width),          //↑
            new Vector2Int( _width, 0 ),        //→
            new Vector2Int( 0, _width * -1 ),   //↓
            new Vector2Int( _width * -1, 0 )    //←
        }; //{x,y}
        _diag = new Vector2Int[] 
        { 
            new Vector2Int( _width, _width ),               //↗
            new Vector2Int( _width, _width * -1 ),          //↘
            new Vector2Int( _width * -1, _width * -1 ),     //↙
            new Vector2Int( _width * -1, _width )           //↖
        }; // {x,y}
    }   

    abstract public List<PNode> Finding(PNode start, PNode end);
}
