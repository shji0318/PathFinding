using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Base : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> _dic = new Dictionary<Type, UnityEngine.Object[]>();


    public void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objs = new UnityEngine.Object[names.Length];
        
        for(int i = 0; i < names.Length; i++)
        {
            T component = gameObject.FindChild<T>(names[i], true);

            if (component != null)
                objs[i] = component;
        }
        _dic.Add(typeof(T), objs);

    }

    public T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objs;

        if (_dic.TryGetValue(typeof(T), out objs ) == false)
            return null;

        return objs[idx] as T;
    }

    public void AddUIEventHandler()
    {

    }
}
