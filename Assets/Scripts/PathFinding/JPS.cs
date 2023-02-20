using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JPS : PathFinding
{    
    #region 진행방향별 노드 체크
    int[,] _checkPoint = new int[,]
    {
        { 3,3,1,0}, // 진행 방향이 위일 때 {← ↖} {→ ↗} 체크
        { 0,0,2,1}, // 진행 방향이 오른쪽일 때 {↑ ↗} {↓ ↘} 체크
        { 1,1,3,2}, // 진행 방향이 아래일 때 {→ ↘} {← ↙} 체크
        { 2,2,0,3}  // 진행 방향이 왼쪽일 때{↓ ↙} {↑ ↖} 체크
    };

    int[,] _checkPointDiag = new int[,]
    {
        { 3,3,2,1}, // 진행 방향이 우상단일 때 {← ↖} {↓ ↘} 체크
        { 0,0,3,2}, // 진행 방향이 우하단일 때 {↑ ↗} {← ↙} 체크
        { 1,1,0,3}, // 진행 방향이 좌하단일 때 {→ ↘} {↑ ↖} 체크
        { 2,2,1,0}  // 진행 방향이 좌상단일 때 {↓ ↙} {→ ↗} 체크
    };

    int[,] _secondSearch = new int[,]
    {
        { 0,1}, // 진행 방향이 우상단일 때 ↑ → 보조탐색
        { 1,2}, // 진행 방향이 우하단일 때 → ↓ 보조탐색
        { 2,3}, // 진행 방향이 좌하단일 때 ↓ ← 보조탐색
        { 3,0}  // 진행 방향이 좌상단일 때 ← ↑ 보조탐색
    };
    //가지치기 = 부모로 부터 이동방향이 주어질 때 자신의 뒤쪽부분은 부모노드 때 검사가 완료됐다고 가정하여 탐사하지 않음
    int[,] _straightPruning = new int[,]
    {
        { 0,1,3,0,3}, // 진행 방향이 위쪽일 때 ← ↑ → ↖ ↗ 방향만 체크
        { 0,1,2,0,1}, // 진행 방향이 오른쪽일 때 ↑ → ↓ ↗ ↘ 방향만 체크
        { 1,2,3,1,2}, // 진행 방향이 아래쪽일 때 → ↓ ←  ↘ ↙ 방향만 체크
        { 0,2,3,2,3}  // 진행 방향이 왼쪽일 때 ↓ ← ↑ ↙ ↖ 방향만 체크
    };

    int[,] _diagPruning = new int[,]
    {
        { 0,1,0,1,3}, // 진행 방향이 우상단일 때 ↑ → ↗ ↘ ↖ 방향만 체크
        { 1,2,1,0,2}, // 진행 방향이 우하단일 때 → ↓ ↘ ↗ ↙ 방향만 체크
        { 2,3,2,1,3}, // 진행 방향이 좌하단일 때 ↓ ← ↙ ↘ ↖ 방향만 체크
        { 3,0,3,0,2}  // 진행 방향이 좌상단일 때 ← ↑ ↖ ↗ ↙ 방향만 체크
    };
    #endregion

    public JPS (int width =5)
    {
        _width = width;
        base.Init();
    }

    public override List<PNode> Finding(PNode start, PNode end)
    {
        PriorityQueue<PNode> openList = new PriorityQueue<PNode>();
        
        _isFind = false;

        #region 처음 start 노드는 8방향 전부 탐색
        for (int dir = 0; dir < 4; dir++)
        {
            if (_isFind == true)
                break;

            PNode jumpPoint = StraightSearch(start, dir, end);

            if (jumpPoint == null)
                continue;


            jumpPoint.H = SetH(jumpPoint, end);
            MapManager.Map.SRDic[jumpPoint._nodeNum].color = Color.gray;            
            openList.Enqueue(jumpPoint);
        }

        for (int diag = 0; diag < 4; diag++)
        {
            if (_isFind == true)
                break;

            PNode jumpPoint = DiagSearch(start, diag, end);

            if (jumpPoint == null)
                continue;


            jumpPoint.H = SetH(jumpPoint, end);
            MapManager.Map.SRDic[jumpPoint._nodeNum].color = Color.gray;
            openList.Enqueue(jumpPoint);
        }
        #endregion
        
        // 다음 노드들 부터는 진행 방향에 따른 가지치기를 통해 필요로하는 영역만 탐색
        while (openList.Count()>0)
        {
            if (_isFind == true)
                break;

            PNode parent = openList.Dequeue();

            if(parent._isStraight== true)
            {
                int dir = parent._dir;
                for(int i = 0; i < 3; i ++)
                {
                    if (_isFind == true)
                        break;

                    PNode jumpPoint = StraightSearch(parent, _straightPruning[dir,i], end);

                    if (jumpPoint == null)
                        continue;

                    jumpPoint.H = SetH(jumpPoint, end);
                    MapManager.Map.SRDic[jumpPoint._nodeNum].color = Color.gray;
                    openList.Enqueue(jumpPoint);
                }

                for(int i = 3; i <5; i++)
                {
                    if (_isFind == true)
                        break;

                    PNode jumpPoint = DiagSearch(parent, _straightPruning[dir, i], end);

                    if (jumpPoint == null)
                        continue;


                    jumpPoint.H = SetH(jumpPoint, end);
                    MapManager.Map.SRDic[jumpPoint._nodeNum].color = Color.gray;
                    openList.Enqueue(jumpPoint);
                }
            }
            else
            {
                int diag = parent._diag;

                for(int i = 0; i < 2; i++)
                {
                    if (_isFind == true)
                        break;

                    PNode jumpPoint = StraightSearch(parent, _diagPruning[diag, i], end);

                    if (jumpPoint == null)
                        continue;


                    jumpPoint.H = SetH(jumpPoint, end);
                    MapManager.Map.SRDic[jumpPoint._nodeNum].color = Color.gray;
                    openList.Enqueue(jumpPoint);
                }
                
                for(int i = 2; i<5;i++)
                {
                    if (_isFind == true)
                        break;

                    PNode jumpPoint = DiagSearch(parent, _diagPruning[diag, i], end);

                    if (jumpPoint == null)
                        continue;


                    jumpPoint.H = SetH(jumpPoint, end);
                    MapManager.Map.SRDic[jumpPoint._nodeNum].color = Color.gray;
                    openList.Enqueue(jumpPoint);
                }
            }       
        }

        if (_isFind == false)
            return null;

        List<PNode> list = new List<PNode>();
        Queue<PNode> q = new Queue<PNode>();
        q.Enqueue(end);
        while(q.Count>0)
        {
            PNode now = q.Dequeue();
            list.Add(now);
            if (now == start)
                break;

            PNode parent = MapManager.Map.List[now._parentNode];
            Vector2Int nextPos = now._isStraight == true ? _dir[(now._dir + 2) % 4] : _diag[(now._diag + 2) % 4];
            while (true)
            {                
                PNode bridge = Util.FindOverlap<PNode>(now._pos + nextPos);                

                if (bridge == parent)
                    break;

                list.Add(bridge);
                now = bridge;
            }
            q.Enqueue(parent);
        }
        
        list.Reverse();
        return list;
                
    }

    public PNode StraightSearch(PNode current,int dir,PNode end)
    {
        Vector2Int pos = current._pos;
        float g = 0;
        PNode jumpPoint = null;

        while(true)
        {
            pos += _dir[dir]; // _dir[dir] 방향으로 한칸 진행하여 서치
            g += _dirWeight; // 가중치 증가

            if (Util.FindOverlap<PNode>(pos) == null)
                break;

            if (Util.FindOverlap<PNode>(pos)._wall == true)
                break;

            if(Util.FindOverlap<PNode>(pos) == end)
            {
                _isFind = true;
                MapManager.Map.SRDic[current._nodeNum].color = Color.gray;                
                return SetJumpPoint(pos, current, true, dir, g);
            }

            PNode left = Util.FindOverlap<PNode>(pos + _dir[_checkPoint[dir, 0]]);
            PNode leftUp = Util.FindOverlap<PNode>(pos + _diag[_checkPoint[dir, 1]]);

            if(left != null && leftUp != null)
            {
                if( left._wall == true && leftUp._wall==false)
                    return SetJumpPoint(pos, current, true, dir, g);
            }

            PNode right = Util.FindOverlap<PNode>(pos + _dir[_checkPoint[dir, 2]]);
            PNode rightUp = Util.FindOverlap<PNode>(pos + _diag[_checkPoint[dir, 3]]);

            if (right != null && rightUp != null)
            {
                if ( right._wall == true && rightUp._wall == false)
                    return SetJumpPoint(pos, current, true, dir, g);                
            }            
        }

        return jumpPoint;
    }

    public PNode DiagSearch(PNode current, int diag, PNode end)
    {
        Vector2Int pos = current._pos;
        float g = 0;
        PNode jumpPoint = null;

        while (true)
        {
            pos += _diag[diag]; // _dir[dir] 방향으로 한칸 진행하여 서치
            g += _diagWeight; // 가중치 증가

            if (Util.FindOverlap<PNode>(pos) == null)
                break;

            if (Util.FindOverlap<PNode>(pos)._wall == true)
                break;

            if (Util.FindOverlap<PNode>(pos) == end)
            {
                _isFind = true;
                MapManager.Map.SRDic[current._nodeNum].color = Color.gray;
                return SetJumpPoint(pos, current, false, diag, g); ;
            }


            PNode left = Util.FindOverlap<PNode>(pos + _dir[_checkPointDiag[diag, 0]]);
            PNode leftUp = Util.FindOverlap<PNode>(pos + _diag[_checkPointDiag[diag, 1]]);

            if (left != null && leftUp != null)
            {
                if (left._wall == true && leftUp._wall == false)
                    return SetJumpPoint(pos, current, false, diag, g);              
            }

            PNode right = Util.FindOverlap<PNode>(pos + _dir[_checkPointDiag[diag, 2]]);
            PNode rightUp = Util.FindOverlap<PNode>(pos + _diag[_checkPointDiag[diag, 3]]);

            if (right != null && rightUp != null)
            {
                if (right._wall == true && rightUp._wall == false)
                    return SetJumpPoint(pos, current, false, diag, g);                
            }
            
            //보조 탐색 
            jumpPoint = StraightSearch(Util.FindOverlap<PNode>(pos), _secondSearch[diag, 0],end);            
            if (jumpPoint != null)
                return SetJumpPoint(pos, current, false, diag, g);           

            jumpPoint = StraightSearch(Util.FindOverlap<PNode>(pos), _secondSearch[diag, 1], end);
            if (jumpPoint != null) 
                return SetJumpPoint(pos,current,false,diag,g);

            
        }

        return jumpPoint;
    }

    public int SetH(PNode now, PNode end)
    {
        int x = Math.Abs(end._pos.x - now._pos.x);
        int y = Math.Abs(end._pos.y - now._pos.y);

        int weight = (int)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)); 


        return weight;
    }

    public PNode SetJumpPoint(Vector2Int pos, PNode current, bool straight, int dir, float g)
    {
        PNode jumpPoint = Util.FindOverlap<PNode>(pos);

        if (jumpPoint.H != 0 && jumpPoint.G < current.G + g)
            return null;

        if (straight == true)
        {
            jumpPoint._parentNode = current._nodeNum;
            jumpPoint._dir = dir;
            jumpPoint._isStraight = straight;
            jumpPoint.G = current.G+g;
        }
        else
        {
            jumpPoint._parentNode = current._nodeNum;
            jumpPoint._diag = dir;
            jumpPoint._isStraight = straight;
            jumpPoint.G = current.G+g;
        }

        return jumpPoint;
    }
}
