using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    public ParticleSystem particle;
    [SerializeField]
    private TrailRenderer trail;

    public void SetTrailEnabled(bool enabled)
    {
        if (this.trail != null)
            trail.enabled = enabled;
    }
    public void SetBoxEnabled(bool enabled)
    {
        var box = this.GetComponent<BoxCollider>();
        if (box != null)
            box.enabled = enabled;
    }
    public void SetTrailTime(float time)
    {
        if (this.trail != null)
            trail.time = time;
    }

    public void ClearTrail()
    {
        if (this.trail != null)
            trail.Clear();
    }

    private BulletEntity bulletEntity;

    public void Init(BulletEntity entity)
    {
        bulletEntity = entity;
        if (trail != null)
            trail.time = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (bulletEntity != null)
        {
            bulletEntity.TriggerEnter(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (bulletEntity != null)
        {
            bulletEntity.TriggerExit(other);
        }
    }
}