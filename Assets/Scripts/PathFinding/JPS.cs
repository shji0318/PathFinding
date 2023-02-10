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
        { 3,3,1,0}, // ���� ������ ���� �� {�� ��} {�� ��} üũ
        { 0,0,2,1}, // ���� ������ �������� �� {�� ��} {�� ��} üũ
        { 1,1,3,2}, // ���� ������ �Ʒ��� �� {�� ��} {�� ��} üũ
        { 2,2,0,3}, // ���� ������ ������ ��{�� ��} {�� ��} üũ
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
        //���� ��忡�� 8���� Ž��
        //�� ���� Ž������ ���� �ڳʸ� �߰����� ��� ����������ġ�� openList�� �����ϸ� �� �� �θ���� ���� ���
        //�밢�� �̵��϶���
        //Straight 
        //��
        //��
        //��
        //�� 

        //Dialog
        //����
        //���ϴ�
        //���ϴ�
        //�»��


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

        int weight = (int)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)); // ��Ÿ��� ���Ǵ� ���� ��Ʈ�� �������������� int ���� ����ϰ� �ֱ⿡ �� �� �������� ����� ���� ��Ʈ�� ������ �ʾ���


        return weight;
    }

    // dir = ��: 0, ��: 1, ��: 2, ��: 3









}
