using System;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Burst;
using UnityEngine;

namespace ProjectDawn.Navigation
{
    public static class VectorUtils
    {
        /// <summary>
        /// 将 float3 转换为 Vector3
        /// </summary>
        public static Vector3 ToVector3(this float3 f)
        {
            return new Vector3(f.x, f.y, f.z);
        }

        /// <summary>
        /// 将 Vector3 转换为 float3
        /// </summary>
        public static float3 ToFloat3(this Vector3 v)
        {
            return new float3(v.x, v.y, v.z);
        }
    }
    /// <summary>
    /// System that steers agent towards destination.
    /// </summary>
    [BurstCompile]
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(AgentSteeringSystemGroup))]
    [Obsolete("AgentSteeringSystem is deprecated, please use AgentSeekingSystem!", false)]
    public partial struct AgentSteeringSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            new AgentSteeringJob().ScheduleParallel();
        }

        [BurstCompile]
        partial struct AgentSteeringJob : IJobEntity
        {
            public void Execute(ref AgentBody body, in AgentSteering steering, in LocalTransform transform)
            {
                if (body.IsStopped)
                    return;

                float3 towards =VectorUtils.ToFloat3(body.Destination ) - transform.Position;
                float distance = math.length(towards);
                float3 desiredDirection = distance > math.EPSILON ? towards / distance : float3.zero;
                body.Force = desiredDirection;
                body.RemainingDistance = distance;
            }
        }
    }
}
