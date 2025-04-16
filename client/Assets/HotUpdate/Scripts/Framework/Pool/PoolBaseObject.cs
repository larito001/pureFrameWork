using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class PoolBaseObject
{
    public bool  __isRecycle { get; set; }
    public bool isRecycle { get { return __isRecycle; } private set { } }
    public abstract void ResetAll();

}
public abstract class PoolBaseGameObject : MonoBehaviour
{
    /// <summary>
    /// 注意，请不要显式修改
    /// </summary>
    public bool __isRecycle { get; set; }
    /// <summary>
    ///读取状态，需要在使用之前，检查是否已经被回收了，如果没被回收，就放心调用。
    ///防止前面已经回收了但又吊了一次的情况发生
    /// </summary>
    public bool isRecycle { get { return  __isRecycle; } private set { } }
    /// <summary>
    /// 请注意，此方法会在放入、取出、收缩、清空时调用，尽量保证不会报错；被内部拦截后会终止整个对象池。
    /// </summary>
    public abstract void ResetAll();
}