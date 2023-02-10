using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JPS : PathFinding
{
    PriorityQueue<PNode> _openList = new PriorityQueue<PNode>();
    List<PNode> _closeList = new List<PNode>();

    int[,] _checkPoint = new int[,]
    {
        { 3,3,1,0}, // 진행 방향이 위일 때 {← ↖} {→ ↗} 체크
        { 0,0,2,1}, // 진행 방향이 오른쪽일 때 {↑ ↗} {↓ ↘} 체크
        { 1,1,3,2}, // 진행 방향이 아래일 때 {→ ↘} {← ↙} 체크
        { 2,2,0,3}, // 진행 방향이 왼쪽일 때{↓ ↙} {↑ ↖} 체크
    };
    public JPS (int width =5)
    {
        _width = width;
        base.Init();
    }

    public override List<PNode> Finding(PNode start, PNode end)
    {


        _openList.Enqueue(start);

        while(_openList.Count()>0)
        {
            PNode parent = _openList.Dequeue();

            for(int i = 0; i < 4; i++)
            {
                PNode jumpPoint = StraightSearch(parent,i);
                
                if(jumpPoint !=null)
                {                    
                    jumpPoint.H = SetH(jumpPoint, end);
                    


                }
            }
            


        }
        //현재 노드에서 8방향 탐색
        //각 방향 탐색에서 만약 코너를 발견했을 경우 시작지점위치를 openList에 저장하며 이 때 부모노드는 현재 노드
        //대각선 이동일때는
        //Straight 
        //상
        //우
        //하
        //좌 

        //Dialog
        //우상단
        //우하단
        //좌하단
        //좌상단


        return null;
    }

    public PNode StraightSearch(PNode current,int dir)
    {
        Vector2Int pos = current._pos;
        float g = current.G;
        PNode jumpPoint = null;

        while(true)
        {            
            PNode left = Util.FindOverlap<PNode>(pos + _dir[_checkPoint[dir, 0]]);
            PNode leftUp = Util.FindOverlap<PNode>(pos + _diag[_checkPoint[dir, 1]]);

            if(left != null && left._wall == true)
            {
                if(leftUp != null && leftUp._wall==false)
                {
                    jumpPoint = Util.FindOverlap<PNode>(pos);
                    jumpPoint._parentNode = current._nodeNum;
                    jumpPoint._dir = dir;
                    jumpPoint._isStraight = true;
                    jumpPoint.G = g;
                    break;
                }
            }

            PNode right = Util.FindOverlap<PNode>(pos + _dir[_checkPoint[dir, 2]]);
            PNode rightUp = Util.FindOverlap<PNode>(pos + _diag[_checkPoint[dir, 3]]);

            if (right != null && right._wall == true)
            {
                if (rightUp != null && rightUp._wall == false)
                {
                    jumpPoint = Util.FindOverlap<PNode>(pos);
                    jumpPoint._parentNode = current._nodeNum;
                    jumpPoint._dir = dir;
                    jumpPoint._isStraight = true;
                    jumpPoint.G = g;
                    break;
                }
            }

            pos += _dir[dir];
            g += _dirWeight;
        }

        return jumpPoint;
    }

    

    public int SetH(PNode now, PNode end)
    {
        int x = Math.Abs(end._pos.x - now._pos.x);
        int y = Math.Abs(end._pos.y - now._pos.y);

        int weight = (int)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)); // 피타고라스 정의는 원래 루트를 씌워져야하지만 int 값을 사용하고 있기에 좀 더 세부적인 계산을 위해 루트를 씌우지 않았음


        return weight;
    }

    // dir = 상: 0, 우: 1, 하: 2, 좌: 3









}
