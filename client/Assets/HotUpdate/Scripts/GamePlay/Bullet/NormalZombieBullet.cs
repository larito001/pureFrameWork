using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalZombieBullet : BulletEntity
{
     public static  DataObjPool<NormalZombieBullet,Transform> pool=new DataObjPool<NormalZombieBullet, Transform>("NormalZombieBullet", 20);
    private int needFireCount = 0;
    private Vector3 lastPos;
    private Vector3 lastDir;
    private Transform parent;
    private Vector3 firePos;
    public override void FireFromTo(Vector3 pos, Vector3 dir)
    {
        firePos = pos;
        destroyTime = 200;
        SetStartTime();
        if (!bulletBase)
        {
            lastPos = pos;
            lastDir +=dir;
            needFireCount++;
            return;
        } 
        // 设置子弹位置：角色前方 + Y轴偏移（比如 +1.0f 高一点）
        bulletBase.transform.position = pos;
        // 设置子弹朝向
        bulletBase.transform.rotation = Quaternion.LookRotation(dir);
        bulletBase.transform.SetParent(parent);
    }

    protected override void AfterInstanceGObj()
    {
        base.AfterInstanceGObj();
        if (needFireCount!=0)
        {
            FireFromTo(lastPos,lastDir);
            lastPos = new Vector3();
            lastDir = new Vector3();
            needFireCount = 0;
        }
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
        PlayerAnimatorCtrl player;
        if (other.TryGetComponent(out player))
        {
            player.characterBase.Hurt(2);
        }
       
    }

    public override void TriggerExit(Collider other)
    {

    }

    public override void SetBulletData(Transform parent)
    {
        this.parent = parent;
        SetInVision(true);
        SetPrefabBundlePath("Assets/HotUpdate/prefabs/Bullet/MeleeBullet.prefab");
    }

    public override void BulletAfterIntoObjectPool()
    {
 
    }
}
