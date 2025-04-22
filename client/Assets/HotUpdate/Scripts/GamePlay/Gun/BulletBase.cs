using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : PoolBaseGameObject
{
    public ParticleSystem particle;

    private void Start()
    {
        particle.Stop();
        particle.Play();
    }

    public override void ResetAll()
    {
        particle.Stop();
    }
}
