using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieColliderCtrl : MonoBehaviour
{
    private BoxCollider collider;
    public int entityId;
    public ZombieEntity _zombieEntity;

    public void Init(ZombieEntity zombieEntity)
    {
        _zombieEntity = zombieEntity;

    }
    public void Run()
    {
        if(collider != null)
        collider.enabled = true;
    }
    public void Stop()
    {
        collider.enabled = false;
    }
    private void Start()
    {
        collider = gameObject.GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _zombieEntity.TriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        _zombieEntity.TriggerExit(other);
    }
}
