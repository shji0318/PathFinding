using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T> where T : PNode// Ascending
{
    int _count=0;
    
    List<T> _list = new List<T>();

    public void Enqueue(T obj)
    {
        _list.Add(obj);
        _count++;

        int now = _count-1;
        int next;
        
        while(now>0)
        {
            next = (now - 1) / 2; // 부모 노드 인덱스

            if (_list[now].F.CompareTo(_list[next].F) > 0)
                break;

            T temp = _list[now];
            _list[now] = _list[next];
            _list[next] = temp;

            now = next;
        }
    }

    public T Dequeue ()
    {   
        T pop = _list[0];        
        _list[0] = _list[_count-1];
        _list.RemoveAt(_count-1);
        _count--;

        #region 갱신
        int now = 0;
        int left;
        int right;       
        while (true)
        {
            left = now * 2 + 1;
            right = now * 2 + 2;

            int next = now;

            if (left <= _count-1 && _list[next].F.CompareTo(_list[left].F) >0)
                next = left;
            if (right <= _count-1 && _list[next].F.CompareTo(_list[right].F) >0)
                next = right;

            if (now == next)
                break;

            T temp = _list[now];
            _list[now] = _list[next];
            _list[next] = temp;

            now = next;

        }
        #endregion

        return pop;
    }

    public void Remove(T element)
    {
        int idx = _list.FindIndex(x => x._nodeNum == element._nodeNum);
        _list[idx] = _list[_count-1];
        _list.RemoveAt(_count - 1);
        _count--;

        #region 갱신
        int now = idx;
        int left;
        int right;
        while (true)
        {
            left = now * 2 + 1;
            right = now * 2 + 2;

            int next = now;

            if (left <= _count - 1 && _list[next].F.CompareTo(_list[left].F) > 0)
                next = left;
            if (right <= _count - 1 && _list[next].F.CompareTo(_list[right].F) > 0)
                next = right;

            if (now == next)
                break;

            T temp = _list[now];
            _list[now] = _list[next];
            _list[next] = temp;

            now = next;

        }
        #endregion
    }

    public void Remove(int idx) // 오버로딩 
    {
        _list[idx] = _list[_count - 1];
        _list.RemoveAt(_count - 1);
        _count--;

        #region 갱신
        int now = idx;
        int left;
        int right;
        while (true)
        {
            left = now * 2 + 1;
            right = now * 2 + 2;

            int next = now;

            if (left <= _count - 1 && _list[next].F.CompareTo(_list[left].F) > 0)
                next = left;
            if (right <= _count - 1 && _list[next].F.CompareTo(_list[right].F) > 0)
                next = right;

            if (now == next)
                break;

            T temp = _list[now];
            _list[now] = _list[next];
            _list[next] = temp;

            now = next;

        }
        #endregion
    }

    public int Find(T element, out int num)
    {
        int idx = _list.FindIndex(x => x._nodeNum == element._nodeNum);

        return num=idx ;
    }

    public T GetElement(int idx)
    {
        return _list[idx] as T;
    }

    public int Count()
    {
        return _count;
    }

    public void Clear()
    {
        _list.Clear();
    }

    

}
