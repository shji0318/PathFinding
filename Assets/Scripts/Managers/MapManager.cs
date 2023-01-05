using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;

    public static MapManager Map { get { return _instance; } }

    List<PNode> _list = new List<PNode>();
    Dictionary<int, SpriteRenderer> _srDic = new Dictionary<int, SpriteRenderer>();


    public bool DoMove { get; private set; }
    public PathFinding Path{ private get; set; }
    public PNode NowNode { get; set; }
    public PNode EndNode { get; set; }
    public int NodeCount { get { return _list.Count; } }
    void Start()
    {
        Init();
        InitMap();
        InitFindingAlgorithm();
    }

    public void Init()
    {
        if (_instance == null) //ΩÃ±€≈Ê
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            return;
    }

    public void InitMap()
    {
        int count = 0;
        foreach (PNode node in gameObject.GetComponentsInChildren<PNode>())
        {
            node._nodeNum = count++;
            SpriteRenderer sr = node.GetComponent<SpriteRenderer>();

            _list.Add(node);
            _srDic.Add(node._nodeNum, sr);           
        }

        NowNode = _list[3];
        _srDic[NowNode._nodeNum].DOColor(Color.blue, 1f).Play();        

    }

    public void InitFindingAlgorithm()
    {
        Path = new AStar();
    }

    public void RefreshWeight()
    {
        foreach (PNode node in _list)
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

            tween = _srDic[node._nodeNum].DOColor(Color.green, 0.2f).From().Play();
            yield return tween.WaitForCompletion();
        }
        
        _srDic[start._nodeNum].DOColor(Color.white, 0.5f).Play();
        _srDic[end._nodeNum].DOColor(Color.blue, 0.5f).Play();

        NowNode = end;
        DoMove = false;

        RefreshWeight();
        yield return null;
    }

    
}
