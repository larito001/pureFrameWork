
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YOTO;

public class EnemyManager : SingletonMono<EnemyManager>
{

    private Dictionary<int,ZombieEntity> zombieEntities = new Dictionary<int,ZombieEntity>();

    private int num = 0;
    
    public void Init()
    {
        Complete();
    }

    public ZombieEntity GetEnemey()
    {
        foreach (var keyValuePair in zombieEntities)
        {
            return keyValuePair.Value;
        }
        return null;
    }


    public void Hurt(int id, float hurt)
    {
        if (zombieEntities.ContainsKey(id))
        {
            // YOTOFramework.poolMgr.GetObjectPool(ObjectPoolType.NormalZombieEntity).Set(zombieEntities[id]);
            zombieEntities[id].Hurt(hurt);
            
        }
    }

    public void RemoveZombie(int id)
    {
        if (zombieEntities.ContainsKey(id))
        {
            zombieEntities.Remove(id);
        }
    }
    private void Complete()
    {
        zombieEntities.Clear();
        var poss= GameObject.Find("EnemyOrgPos");
        var posTransforms= poss.GetComponentsInChildren<Transform>();
        for (var i = 0; i < posTransforms.Length; i++)
        {
           var pos = posTransforms[i].position;
            YOTOFramework.timeMgr.LoopCall(() =>
            {
                ZombieEntity zombieEntity=  ZombieEntity.pool.GetItem(Vector3.zero);
                zombieEntity.InstanceGObj();
                // 设置位置，假设僵尸在地面上（y轴为0）
                zombieEntity.Location=pos;

                zombieEntities.Add(zombieEntity._entityID, zombieEntity);
            },1,10);
        }

  
       
    }

    public void SetTarget(Transform target)
    {
        foreach (var z in zombieEntities.Values)
        {
            z.SetTarget(target);
        }
    }
    
}
