using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class EnemyRangeTrigger : MonoBehaviour
{
    public float Range { get;private set; }
    public int Id { get;private set; }
    private void Awake()
    {
       var rig= GetComponent<Rigidbody>();
       rig.useGravity = false;
       var box= GetComponent<BoxCollider>();
       box.isTrigger = true;
    }

    public void Init(int id)
    {
        this.Id = id;
    }
    public void SetRange(float range)
    {
        this.Range = range;
        var box= GetComponent<BoxCollider>();
        box.size = new Vector3(this.Range, 4, this.Range);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerAnimatorCtrl player;
        if (other.TryGetComponent<PlayerAnimatorCtrl>(out player))
        {
            EnemyManager.Instance.TriggerEnmeies(Id,other.transform);
        }
      
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerAnimatorCtrl player;
        if (other.TryGetComponent<PlayerAnimatorCtrl>(out player))
        {
            EnemyManager.Instance.ExitEnmeies(Id);
        }

    }
}
