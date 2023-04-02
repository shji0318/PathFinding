using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AStar : PathFinding
{   
    
    public AStar(int width = 5) // ���� ������� Ÿ���� �ʺ� ���� default�� �س���, ���� ���� 
    {   
        _width = width;
        base.Init();        
    }
    
    // �� �� �� �� dir
    // �� �� �� �� diag
    public override List<PNode> Finding(PNode start, PNode end)
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        _isFind = false;
        bool[] visited = new bool[MapManager.Map.NodeCount];
        int[] weights = new int[MapManager.Map.NodeCount];
        PriorityQueue<PNode> openList = new PriorityQueue<PNode>();
        Dictionary<int, PNode> closeList = new Dictionary<int, PNode>();

        openList.Enqueue(start);
        visited[start._nodeNum] = true; // closeList�� ���ٸ� ��湮�� �ʿ䰡 ������ �н�
        
        while(openList.Count()>0)
        {
            PNode now = openList.Dequeue();
            closeList.Add(now._nodeNum, now);
            visited[now._nodeNum] = true; 
            

            if (now == end)
            {
                _isFind = true;
                break;
            }
                
                 

            PNode[] dirNode = new PNode[4]; // �밢���� ���� ���θ� ���ϱ� ���� ����
            
            
            // �����¿� ���⿡ ��带 OverlapCircle�� ���� ã�� �ݺ���
            /* OverlapCircleAll�� ã�� ���� ������ �밢���� �ִ� ���� ���� ��� ���̿� ��尡 ���̰ų� ����ִٸ� 
               ���� �հ� ������ �� �ֱ� ������ ���ڿ������⿡ �̵����� ���ϰ� �ϱ� ���ؼ� �̷��� �����*/
            for(int i = 0; i < 4;i++) 
            {
                PNode next = FindNode(now, _dir[i]);

                if (next == null)
                    continue;

                if (visited[next._nodeNum])
                    continue;

                int idx;
                if(openList.Find(next,out idx)>-1)
                {
                    PNode n = openList.GetElement(idx);

                    if (n.F <= now.G+_dirWeight + SetH(next, end))
                        continue;

                    openList.Remove(idx);
                }
                
                                
                next.G = now.G+_dirWeight;
                next.H = SetH(next, end);
                next._parentNode = now._nodeNum;
                dirNode[i] = next;
                visited[next._nodeNum] = true;                
                openList.Enqueue(next);
            }
            // �밢�� ���⿡ ��带 ã�� �ݺ���
            for(int i = 0; i< 4; i++)
            {
                if (dirNode[i % 4] == null || dirNode[(i + 1) % 4] == null) // �밢�� ��忡 �α� ��尡 ���ų� ���̶�� �̵� ���ϱ� ����
                    continue;                                               // EX) �� ���� �̵��� �� ��,�� ���� ��尡 ���̰ų� �������� ��� �������� �Ǹ� ���ڿ������� ������ ���ϰ� ����

                PNode next = FindNode(now, _diag[i]);

                if (next == null)
                    continue;

                if (visited[next._nodeNum])
                    continue;

                int idx;
                if (openList.Find(next, out idx) > -1) // ����ġ �� ���� �켱���� ť �ȿ� ������ ��尡 ������ ��� ����ġ�� �� ���ٸ� ��ü�ϴ� �۾��� �����ϱ� ���� �ش� ��带 ����
                {                                      // �׷��� �ʴٸ� ���� ����ġ�� �� ���� ������ ���� �߰��� �� �ʿ� ��� continue
                    PNode n = openList.GetElement(idx);

                    if (n.F <= now.G+_diagWeight + SetH(next, end))
                        continue;

                    openList.Remove(idx);
                }

                next.G = now.G+_diagWeight;
                next.H = SetH(next, end);
                next._parentNode = now._nodeNum;
                visited[next._nodeNum] = true;                
                openList.Enqueue(next);             
            }
        }

        sw.Stop();

        Debug.Log($"A* �˰��� �ҿ� �ð� : {sw.ElapsedMilliseconds}ms");

        if (_isFind == false)
            return null;

        List<PNode> list = new List<PNode>();
        PNode node = end;
        list.Add(node);
        while(true)
        {
            if (node == start)
                break;

            node = closeList[node._parentNode];
            list.Add(node);
        }

        list.Reverse();
       
       
        return list;
    } 

    
    public int SetH (PNode now, PNode end)
    {
        int x = Math.Abs(end._pos.x - now._pos.x);
        int y = Math.Abs(end._pos.y - now._pos.y);

        int weight = (int)Math.Sqrt(Math.Pow(x,2) + Math.Pow(y,2)); // ��Ÿ��� ���Ǵ� ���� ��Ʈ�� �������������� int ���� ����ϰ� �ֱ⿡ �� �� �������� ����� ���� ��Ʈ�� ������ �ʾ���
                                                           

        return weight;
    }

    public PNode FindNode (PNode now, Vector2Int next)
    {
        Collider2D co = Physics2D.OverlapPoint(now._pos + next);

        if (co == null)
            return null;

        PNode node = co.GetComponent<PNode>();

        if (node == null || node._wall == true)
            return null;

        return node;
    }
}
