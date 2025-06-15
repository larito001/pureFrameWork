using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YOTO;

public class TowerBaseEntity : ObjectBase, PoolItem<Vector3>
{
    public static DataObjPool<TowerBaseEntity, Vector3> pool =
        new DataObjPool<TowerBaseEntity, Vector3>("TowerBaseEntity", 20);

    public static TowerBaseEntity selectTower;
    private bool isStartShoot = false;
    public void StartFire()
    {
        isStartShoot = true;
    }
    protected override void YOTOOnload()
    {
    }

    public override void YOTOStart()
    {
    }

    private float timer = 0;

    public override void YOTOUpdate(float deltaTime)
    {
        if (!isStartShoot) return;
        if (timer > 0.2f)
        {
            var e = EnemyManager.Instance.GetEnemey();
            if (e != null&&e.zombieBase!=null)
            {
                var dir = e.zombieBase.transform.position - this.Location;
                var bullet = NormalGunBullet.pool.GetItem(Vector3.zero);
                bullet.InstanceGObj();
                bullet.FireFromTo(this.Location + new Vector3(0, 1, 0), dir);
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
      var towerBase=  ObjTrans.GetComponent<TowerBase>();
      towerBase.SetId(this._entityID);
    }

    public void AfterIntoObjectPool()
    {
        isStartShoot = false;
    }

    public void SetData(Vector3 serverData)
    {
        SetInVision(true);
        SetPrefabBundlePath("Assets/HotUpdate/prefabs/TowerBase/BaseOfTower.prefab");
    }

    public void OnEnter()
    {
        if (!isStartShoot)
        {
            YOTOFramework.uIMgr.Show(UIEnum.TowerCreateUI);
            selectTower = this; 
        }

    }

    public void OnExit()
    {
        YOTOFramework.uIMgr.Hide(UIEnum.TowerCreateUI);
        selectTower = null;
    }


}