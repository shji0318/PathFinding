using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PathFinding // ���� PathFinding �˰����� ��ü�� ���� ���� ��� Ȯ���� ����Ͽ� �θ� �߻� Ŭ������ ����
{
    public int _width; // Ÿ�� ũ�Ⱑ ��� ���� �������� �ʱ� ������ ���� �����ϰ� �ϱ� ����
    // �� �� �� ��
    public Vector2Int[] _dir;
    // �� �� �� ��
    public Vector2Int[] _diag;

    public int _dirWeight;
    public int _diagWeight;

    public void Init()
    {
        // �������� width�� ���� ����ġ �ʱ�ȭ
        _dirWeight = _width;
        _diagWeight = (int)(_width * 1.4); // �밢���� ������ �� ���� ���̰� ���� ���� �ﰢ���� �밢���� ���̴� [�� �� * ��Ʈ2(�� 1.4)]�� ���� ����

        //���� ��� ���� �����¿�, �밢�� �� �̵� ��ǥ
        _dir = new Vector2Int[] 
        {
            new Vector2Int(0, _width),          //��
            new Vector2Int( _width, 0 ),        //��
            new Vector2Int( 0, _width * -1 ),   //��
            new Vector2Int( _width * -1, 0 )    //��
        }; //{x,y}
        _diag = new Vector2Int[] 
        { 
            new Vector2Int( _width, _width ),               //��
            new Vector2Int( _width, _width * -1 ),          //��
            new Vector2Int( _width * -1, _width * -1 ),     //��
            new Vector2Int( _width * -1, _width )           //��
        }; // {x,y}
    }

    

    abstract public List<PNode> Finding(PNode start, PNode end);

}
