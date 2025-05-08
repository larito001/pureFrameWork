
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YOTO;

public class EnemyManager : SingletonMono<EnemyManager>
{
    private List<string> zombies = new List<string>()
    {
        "Assets/HotUpdate/prefabs/Zombies/NormalZombie.prefab",
    };
    private Dictionary<int,ZombieEntity> zombieEntities = new Dictionary<int,ZombieEntity>();

    private int num = 0;
    
    public void Init()
    {
        zombieEntities.Clear();
        num=zombies.Count;
        for (var i = 0; i < zombies.Count; i++)
        {
            YOTOFramework.resMgr.LoadGameObject(zombies[i],
                Vector3.zero, Quaternion.identity, (obj, pos, rot) =>
                {
                    YOTOFramework.poolMgr.GetGameObjectPool(GameObjectPoolType.NormalZombie).SetPrefab(obj.GetComponent<ZombieAnimatorCtrl>());

                    PopList();
                });
        }

       
    }

    private void PopList()
    {
        num--;
        if (num == 0)
        {
            Complete();
        }
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
        YOTOFramework.timeMgr.LoopCall(() =>
        {
            ZombieEntity z = YOTOFramework.poolMgr.GetObjectPool(ObjectPoolType.NormalZombieEntity).Get<ZombieEntity>();
            // 设置位置，假设僵尸在地面上（y轴为0）
            z.SetPosition(new Vector3(-29, 0f, -50));

            zombieEntities.Add(z._entityID, z);
        },1);
  
       
    }

    public void SetTarget(Transform target)
    {
        foreach (var z in zombieEntities.Values)
        {
            z.SetTarget(target);
        }
    }
    
}
