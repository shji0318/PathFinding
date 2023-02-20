using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapManager : MonoBehaviour
{
    public static MapManager _instance;

    public static MapManager Map { get { return _instance; } }

    Dictionary<int,PNode> _list = new Dictionary<int,PNode>(); // 현재 맵의 PNode들을 바인딩
    Dictionary<int, SpriteRenderer> _srDic = new Dictionary<int, SpriteRenderer>(); // DoTween의 DoColor함수를 사용하기 위해 미리 바인딩
           
    public Dictionary<int, PNode> List { get { return _list; }}
    public Dictionary<int, SpriteRenderer> SRDic { get { return _srDic; } }
    public bool DoMove { get; private set; }
    public bool AStar { get; set; } = true;
    public PathFinding Path{ private get; set; }
    public PNode NowNode { get; set; }
    public PNode EndNode { get; set; }
    public int NodeCount { get { return _list.Count; } }
    void Start()
    {
        GenerateMap();
    }


    public void GenerateMap()
    {
        InitFindingAlgorithm(new AStar());
        Init();
        InitMap();
    }

    public void Init()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    public void InitMap() // 맵 초기화
    {
                   
        foreach (PNode node in gameObject.GetComponentsInChildren<PNode>(true))
        {
            node._nodeNum = (node._pos.y / Path._width) * 10 + (node._pos.x / Path._width);
            node.gameObject.name = node._nodeNum.ToString();
            SpriteRenderer sr = node.GetComponent<SpriteRenderer>();                    

            _list.Add(node._nodeNum,node);
            _srDic.Add(node._nodeNum, sr);           
        }      

        NowNode = _list[0];
        _srDic[NowNode._nodeNum].DOColor(Color.blue, 1f).Play();        

    }

    public void InitFindingAlgorithm(PathFinding path)
    {
        Path = path;
        RefreshNode();        
    }

    public void RefreshNode()
    {
        foreach (PNode node in _list.Values)
        {
            node.G = 0;
            node.H = 0;
            node._isStraight = false;
            node._dir = 0;
            node._diag = 0;
            node._parentNode = 0;

            if(node._wall == false)
                SRDic[node._nodeNum].DOColor(Color.white, 0.5f).Play();

            _srDic[NowNode._nodeNum].DOColor(Color.blue, 1f).Play();
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

        if (list == null)
        {
            var tweenStart = _srDic[start._nodeNum].DOColor(Color.blue, 0.5f).Play();
            yield return tweenStart.WaitForCompletion();
            var tweenEnd = _srDic[end._nodeNum].DOColor(Color.white, 0.5f).Play();
            yield return tweenEnd.WaitForCompletion();

            RefreshNode();

            DoMove = false;
            Debug.Log("현재 이동할 수 있는 루트가 없습니다. 막다른길로만 이뤄져 있는지 확인해 주세요");
            yield break;
        }

        for (int i = 0; i < list.Count; i++)
        {
            PNode node = list[i];

            tween = _srDic[node._nodeNum].DOColor(Color.green, 0.5f).From().Play();
            yield return tween.WaitForCompletion();
        }
        
        _srDic[start._nodeNum].DOColor(Color.white, 0.5f).Play();
        _srDic[end._nodeNum].DOColor(Color.blue, 0.5f).Play();        

        NowNode = end;
        RefreshNode();

        DoMove = false;
        
        yield break;
    }    
}
