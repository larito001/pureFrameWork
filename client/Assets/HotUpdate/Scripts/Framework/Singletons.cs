using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T>where T : new()
{
   private static T instance;
   public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }

}
public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
        
                GameObject obj = new GameObject(typeof(T).ToString());
                Debug.Log("创建mono单例" + obj);
                DontDestroyOnLoad(obj);
                instance = obj.AddComponent<T>();


            }
            return instance;
        }
    }

    protected void OnDestroy()
    {
        instance = null;
    }
}