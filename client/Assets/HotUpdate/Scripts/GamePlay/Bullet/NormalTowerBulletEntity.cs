using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YOTO;

public class NormalTowerBulletEntity : BulletEntity
{
    public static  DataObjPool<NormalTowerBulletEntity,Transform> pool=new DataObjPool<NormalTowerBulletEntity, Transform>("NormalTowerBulletEntity", 20);
    public override void FireFromTo(Vector3 pos, Vector3 dir)
    {
        if(!bulletBase) return ;
        bulletBase.trail.enabled = false;
        // 设置子弹位置：角色前方 + Y轴偏移（比如 +1.0f 高一点）
        bulletBase.transform.position = pos;
        bulletBase.trail.Clear();
        bulletBase.trail.time = 0.1f; 
        // 设置子弹朝向
        bulletBase.transform.rotation = Quaternion.LookRotation(dir);
        bulletBase.trail.enabled = true;
        // 添加刚体
        Rigidbody temp;
        if (!bulletBase.gameObject.TryGetComponent<Rigidbody>(out  temp))
        {
            temp =bulletBase.gameObject.AddComponent<Rigidbody>();
            temp.useGravity = false;
        }
        temp.velocity = dir.normalized * 50f;
    }

    //主动移除
    public override void Remove()
    {
        Debug.Log("回收");
        base.Remove();
        pool.RecoverItem(this);
    }


    public override void TriggerEnter(Collider other)
    {
        
    }

    public override void SetBulletData(Transform parent)
    {
        SetInVision(true);
        SetPrefabBundlePath("Assets/HotUpdate/prefabs/Bullet/Bullet.prefab");
    }

    public override void BulletAfterIntoObjectPool()
    {
 
    }

}
