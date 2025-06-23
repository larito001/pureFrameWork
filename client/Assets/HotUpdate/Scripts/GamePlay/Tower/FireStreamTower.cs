using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStreamTower : ObjectBase, PoolItem<TowerBaseEntity>
{
    public static DataObjPool<FireStreamTower, TowerBaseEntity> pool =
        new DataObjPool<FireStreamTower, TowerBaseEntity>("FireStreamTower", 20);

    private float timer = 3;

    protected override void YOTOOnload()
    {
     
    }

    public override void YOTOStart()
    {
     
    }
    public override void YOTOUpdate(float deltaTime)
    {
        if (timer >5f)
        {
            var e = EnemyManager.Instance.GetEnemey();
            if (e != null && e.zombieBase != null)
            {
                var dir = e.zombieBase.transform.position - this.Location;
                var bullet = FireStreamBullet.pool.GetItem(null);
                bullet.InstanceGObj();
                bullet.FireFromTo(this.Location , dir);
                Debug.Log("位置：" + this.Location);
            }
            timer = 0;
        }

        timer += deltaTime;
    }

    public override void YOTONetUpdate()
    {
    }

    public override void YOTOFixedUpdate(float deltaTime)
    {
    }

    public override void YOTOOnHide()
    {
    }

    protected override void AfterInstanceGObj()
    {
        
    }

    public void AfterIntoObjectPool()
    {
    }

    public void SetData(TowerBaseEntity serverData)
    {
        SetInVision(true);
        SetPrefabBundlePath("Assets/HotUpdate/prefabs/Tower/PreFabs/Sci-Fi_Turret_2_Red.prefab");
    }
}