using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AStar : PathFinding
{   
    
    public AStar(int width = 5) // 현재 사용중인 타일의 너비 값을 default로 해놓음, 변경 가능 
    {   
        _width = width;
        base.Init();        
    }
    
    // ↑ → ↓ ← dir
    // ↗ ↘ ↙ ↖ diag
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
        visited[start._nodeNum] = true; // closeList에 들어갔다면 재방문할 필요가 없으니 패스
        
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
                
                 

            PNode[] dirNode = new PNode[4]; // 대각선에 가능 여부를 구하기 위해 저장
            
            
            // 상하좌우 방향에 노드를 OverlapCircle을 통해 찾는 반복문
            /* OverlapCircleAll로 찾지 않은 이유는 대각선에 있는 노드와 현재 노드 사이에 노드가 벽이거나 비어있다면 
               벽을 뚫고 지나갈 수 있기 때문에 부자연스럽기에 이동하지 못하게 하기 위해서 이렇게 사용함*/
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
            // 대각선 방향에 노드를 찾는 반복문
            for(int i = 0; i< 4; i++)
            {
                if (dirNode[i % 4] == null || dirNode[(i + 1) % 4] == null) // 대각선 노드에 인근 노드가 없거나 벽이라면 이동 못하기 때문
                    continue;                                               // EX) ↗ 노드로 이동할 때 ↑,→ 방향 노드가 벽이거나 막혀있을 경우 지나가게 되면 부자연스럽기 때문에 못하게 막음

                PNode next = FindNode(now, _diag[i]);

                if (next == null)
                    continue;

                if (visited[next._nodeNum])
                    continue;

                int idx;
                if (openList.Find(next, out idx) > -1) // 가중치 비교 현재 우선순위 큐 안에 동일한 노드가 존재할 경우 가중치가 더 낮다면 교체하는 작업을 진행하기 위해 해당 노드를 제거
                {                                      // 그렇지 않다면 현재 가중치가 더 낮기 때문에 새로 추가해 줄 필요 없어서 continue
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

        Debug.Log($"A* 알고리즘 소요 시간 : {sw.ElapsedMilliseconds}ms");

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

        int weight = (int)Math.Sqrt(Math.Pow(x,2) + Math.Pow(y,2)); // 피타고라스 정의는 원래 루트를 씌워져야하지만 int 값을 사용하고 있기에 좀 더 세부적인 계산을 위해 루트를 씌우지 않았음
                                                           

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
