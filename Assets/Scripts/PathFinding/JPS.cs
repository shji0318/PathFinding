using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JPS : PathFinding
{    
    #region ������⺰ ��� üũ
    int[,] _checkPoint = new int[,]
    {
        { 3,3,1,0}, // ���� ������ ���� �� {�� ��} {�� ��} üũ
        { 0,0,2,1}, // ���� ������ �������� �� {�� ��} {�� ��} üũ
        { 1,1,3,2}, // ���� ������ �Ʒ��� �� {�� ��} {�� ��} üũ
        { 2,2,0,3}  // ���� ������ ������ ��{�� ��} {�� ��} üũ
    };

    int[,] _checkPointDiag = new int[,]
    {
        { 3,3,2,1}, // ���� ������ ������ �� {�� ��} {�� ��} üũ
        { 0,0,3,2}, // ���� ������ ���ϴ��� �� {�� ��} {�� ��} üũ
        { 1,1,0,3}, // ���� ������ ���ϴ��� �� {�� ��} {�� ��} üũ
        { 2,2,1,0}  // ���� ������ �»���� �� {�� ��} {�� ��} üũ
    };

    int[,] _secondSearch = new int[,]
    {
        { 0,1}, // ���� ������ ������ �� �� �� ����Ž��
        { 1,2}, // ���� ������ ���ϴ��� �� �� �� ����Ž��
        { 2,3}, // ���� ������ ���ϴ��� �� �� �� ����Ž��
        { 3,0}  // ���� ������ �»���� �� �� �� ����Ž��
    };
    //����ġ�� = �θ�� ���� �̵������� �־��� �� �ڽ��� ���ʺκ��� �θ��� �� �˻簡 �Ϸ�ƴٰ� �����Ͽ� Ž������ ����
    int[,] _straightPruning = new int[,]
    {
        { 0,1,3,0,3}, // ���� ������ ������ �� �� �� �� �� �� ���⸸ üũ
        { 0,1,2,0,1}, // ���� ������ �������� �� �� �� �� �� �� ���⸸ üũ
        { 1,2,3,1,2}, // ���� ������ �Ʒ����� �� �� �� ��  �� �� ���⸸ üũ
        { 0,2,3,2,3}  // ���� ������ ������ �� �� �� �� �� �� ���⸸ üũ
    };

    int[,] _diagPruning = new int[,]
    {
        { 0,1,0,1,3}, // ���� ������ ������ �� �� �� �� �� �� ���⸸ üũ
        { 1,2,1,0,2}, // ���� ������ ���ϴ��� �� �� �� �� �� �� ���⸸ üũ
        { 2,3,2,1,3}, // ���� ������ ���ϴ��� �� �� �� �� �� �� ���⸸ üũ
        { 3,0,3,0,2}  // ���� ������ �»���� �� �� �� �� �� �� ���⸸ üũ
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

        #region ó�� start ���� 8���� ���� Ž��
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
        
        // ���� ���� ���ʹ� ���� ���⿡ ���� ����ġ�⸦ ���� �ʿ���ϴ� ������ Ž��
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
            pos += _dir[dir]; // _dir[dir] �������� ��ĭ �����Ͽ� ��ġ
            g += _dirWeight; // ����ġ ����

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
            pos += _diag[diag]; // _dir[dir] �������� ��ĭ �����Ͽ� ��ġ
            g += _diagWeight; // ����ġ ����

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
            
            //���� Ž�� 
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
