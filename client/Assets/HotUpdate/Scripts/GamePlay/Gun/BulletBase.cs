using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : PoolBaseGameObject
{
    public ParticleSystem particle;
    public TrailRenderer trail;

  
    private void Start()
    {
        
    }

    public override void ResetAll()
    {
        trail.time = 0;
    }

    public override void OnStart()
    {


    }
}
