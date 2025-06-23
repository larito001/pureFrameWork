using System;
using System.Collections;
using System.Collections.Generic;
using ProjectDawn.Navigation.Hybrid;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = System.Numerics.Vector3;

public class ZombieNavCtrl : MonoBehaviour
{
    private ZombieEntity zombieEntity;
    private UnityEngine.Vector3 Target;
    private AgentAuthoring agent;
    private bool canMove = false;
    private bool haveAim = false;

    private void Awake()
    {
        agent = transform.GetComponent<AgentAuthoring>();
    }
    private float atkTimer = 0;
    private void FixedUpdate()
    {
        if (!canMove) return;
        
        UnityEngine.Vector3 currentPos = transform.position;
        float distance = math.distance(currentPos, agent.EntityBody.Destination);
        if (agent.EntityBody.IsStopped&&haveAim&&distance <= (agent.GetStopDistance()+0.1f))
        {
            if (atkTimer >= 2.4f)
            {
                zombieEntity.zombieBase.EnemyAtk();
                atkTimer = 0;
            }
            atkTimer += Time.fixedDeltaTime;
          
        }else if (agent.EntityBody.IsStopped&&!haveAim&&distance <= (agent.GetStopDistance()+0.1f))
        {
            atkTimer = 2.4f;
            zombieEntity.zombieBase.EnemyIdel();
        }
        else if(!agent.EntityBody.IsStopped&&distance > (agent.GetStopDistance()+0.1f))
        {
            atkTimer = 2.4f;
            zombieEntity.zombieBase.EnemyRun();
        }
    }

    public void Stop()
    {
        agent.Stop();
        canMove = false;
        haveAim = false;
    }

    public void SetTarget(UnityEngine.Vector3 Target)
    {
        if (canMove && agent)
        {
            haveAim = true;
            this.Target = Target;
            var body = agent.EntityBody;
            body.Destination = Target;
            body.IsStopped = false;
            agent.EntityBody = body;
       
        }
    }

    public void Init(ZombieEntity zombieEntity)
    {
        this.zombieEntity = zombieEntity;
        canMove = true;
    }
}