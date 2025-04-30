using System.Collections;
using System.Collections.Generic;
using ProjectDawn.Navigation.Hybrid;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class ZombieNavCtrl : MonoBehaviour
{
    private UnityEngine.Vector3 Target;
    private AgentAuthoring agent;
    private void Start()
    {
        agent= transform.GetComponent<AgentAuthoring>();

    }

    public void Stop()
    {
        agent.Stop();
    }
    public void SetTarget(UnityEngine.Vector3 Target)
    {
        Target = Target;
        var body = agent.EntityBody;
        body.Destination =Target;
        body.IsStopped = false;
        agent.EntityBody = body;
    }

}
