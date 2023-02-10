using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;

    public static MapManager Map { get { return _instance; } }

    Dictionary<int,PNode> _list = new Dictionary<int,PNode>(); // 현재 맵의 PNode들을 바인딩
    Dictionary<int, SpriteRenderer> _srDic = new Dictionary<int, SpriteRenderer>(); // DoTween의 DoColor함수를 사용하기 위해 미리 바인딩
    PNode[,] _mapNode = new PNode[10,10];


    public PNode[,] MapNode { get { return _mapNode; } }
    public int MapMaxX { get { return _mapNode.GetLength(1); } }
    public int MapMaxY { get { return _mapNode.GetLength(0); } }
    public bool DoMove { get; private set; }
    public PathFinding Path{ private get; set; }
    public PNode NowNode { get; set; }
    public PNode EndNode { get; set; }
    public int NodeCount { get { return _list.Count; } }
    void Start()
    {
        InitFindingAlgorithm();
        Init();
        InitMap();        
    }

    public void Init()
    {
        if (_instance == null) //싱글톤
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            return;
    }

    public void InitMap() // 맵 초기화
    {
        int count = 0;               
        foreach (PNode node in gameObject.GetComponentsInChildren<PNode>(true))
        {
            node._nodeNum = count++;
            SpriteRenderer sr = node.GetComponent<SpriteRenderer>();
            node._arrPosY = node._pos.y / Path._width;
            node._arrPosX = node._pos.x / Path._width;
            if (node._wall == true)
                _mapNode[node._arrPosY,node._arrPosX]= node;

            _list.Add(node._nodeNum,node);
            _srDic.Add(node._nodeNum, sr);           
        }      

        NowNode = _list[0];
        _srDic[NowNode._nodeNum].DOColor(Color.blue, 1f).Play();        

    }

    public void InitFindingAlgorithm()
    {
        Path = new AStar();
    }

    public void RefreshWeight()
    {
        foreach (PNode node in _list.Values)
        {
            node.G = 0;
            node.H = 0;
        }
    }

    public IEnumerator Move(PNode start,PNode end)
    {        
        if (start == null || end == null)
            yield break;

        DoMove = true;
        
        var tween = _srDic[end._nodeNum].DOColor(Color.red, 1f).Play();

        List<PNode> list = Path.Finding(start, end);

        yield return tween.WaitForCompletion();
        
        for (int i = 0; i < list.Count; i++)
        {
            PNode node = list[i];

            tween = _srDic[node._nodeNum].DOColor(Color.green, 0.5f).From().Play();
            yield return tween.WaitForCompletion();
        }
        
        _srDic[start._nodeNum].DOColor(Color.white, 0.5f).Play();
        _srDic[end._nodeNum].DOColor(Color.blue, 0.5f).Play();

        NowNode = end;
        DoMove = false;

        RefreshWeight();
        yield break;
    }    
}
