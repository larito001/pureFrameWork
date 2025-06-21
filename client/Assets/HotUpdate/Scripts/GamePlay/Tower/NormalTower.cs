
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTower :  ObjectBase, PoolItem<TowerBaseEntity>
{
    public static DataObjPool<NormalTower, TowerBaseEntity> pool =
        new DataObjPool<NormalTower, TowerBaseEntity>("NormalTower", 20);

    private float timer = 3;

    protected override void YOTOOnload()
    {
     
    }

    public override void YOTOStart()
    {
     
    }
    private Queue<BulletEntity> bullets = new Queue<BulletEntity>();
    public override void YOTOUpdate(float deltaTime)
    {
        if (timer >1f)
        {
            var e = EnemyManager.Instance.GetEnemey();
            if (e != null && e.zombieBase != null)
            {
                var dir = e.zombieBase.transform.position - this.Location;
                var bullet = NormalTowerBulletEntity.pool.GetItem(null);
                bullet.InstanceGObj();
                bullet.FireFromTo(this.Location + new Vector3(0, 1, 0), dir);
                bullets.Enqueue(bullet);
                Debug.Log("位置：" + this.Location);
            }
            timer = 0;
        }

        timer += deltaTime;
        
        if (bullets.Count > 0)
        {
            var current = bullets.Peek();
            long currentTime = System.DateTime.Now.Ticks / 10000;  // 当前时间戳，单位为毫秒
            long elapsedTime = currentTime - current.GetStartTime();  // 时间差，单位为毫秒

            if (elapsedTime > 2000)  // 如果超过 5 秒（5000 毫秒）
            {
                bullets.Dequeue();
                current.Remove();
            }
        }
    
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
        SetPrefabBundlePath("");
    }
}
