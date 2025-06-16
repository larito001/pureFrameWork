using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase :MonoBehaviour
{
    public ParticleSystem particle;
    public TrailRenderer trail;

    private BulletEntity bulletEntity;
    public void Init(BulletEntity entity)
    {
        bulletEntity=entity;
        trail.time = 0;
    }
    private void OnTriggerEnter(Collider other)
    { 

        if (bulletEntity != null)
        {
            bulletEntity.TriggerEnter(other);
        }
    
       
    }
}
