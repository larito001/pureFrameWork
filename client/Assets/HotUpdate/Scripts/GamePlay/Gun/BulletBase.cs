using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : PoolBaseGameObject
{
    public ParticleSystem particle;
    public TrailRenderer trail;
    public long startTime;
  
    private void Start()
    {
        
    }

    public override void ResetAll()
    {
        trail.time = 0;
    }

    public override void OnStart()
    {

        // 使用 DateTime.Now 获取当前时间，然后转换为毫秒
        startTime = System.DateTime.Now.Ticks / 10000; // 每 10000 Ticks 等于 1 毫秒
    }
}
