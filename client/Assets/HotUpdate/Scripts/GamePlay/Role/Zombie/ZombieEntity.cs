using System.Collections;
using System.Collections.Generic;
using ProjectDawn.Navigation;
using ProjectDawn.Navigation.Hybrid;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using YOTO;

public class ZombieEntity : ObjectBase, PoolItem<Vector3>
{
    public static DataObjPool<ZombieEntity, Vector3> pool = new DataObjPool<ZombieEntity, Vector3>("ZombieEntity", 50);
    public ZombieAnimatorCtrl zombieBase;
    public ZombieNavCtrl zombieNav;
    public float HP = 100;
    private Transform target;

    public void SetTarget(Transform t)
    {
        this.target = t;
    }

    protected override void AfterInstanceGObj()
    {
        zombieBase = objTrans.GetComponent<ZombieAnimatorCtrl>();
        zombieBase.gameObject.SetActive(true);
        zombieNav = zombieBase.GetComponent<ZombieNavCtrl>();
        zombieNav.Init(this);
        zombieBase.GetComponent<ZombieColliderCtrl>().entityId = this._entityID;
        zombieBase.EnemyRun();
        zombieBase.GetComponent<ZombieColliderCtrl>().Run();
        HP = 100;
        if (this.target)
        {
            SetTarget(target);
        }

        zombieBase.GetComponent<AgentCrowdPathingAuthoring>().Group =
            GameObject.Find("Crowd Group").GetComponent<CrowdGroupAuthoring>();
    }

    public void Hurt(float hurt)
    {
        HP -= hurt;
        if (HP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        zombieNav.Stop();
        YOTOFramework.timeMgr.DelayCall(() =>
        {
            Remove();
            EnemyManager.Instance.RemoveZombie(_entityID);
        },2.2f);
        zombieBase.EnemyDie();
        zombieBase.GetComponent<ZombieColliderCtrl>().Stop();


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
        if (HP > 0 && zombieNav && target)
        {
            timer += deltaTime;
            if (timer > 1)
            {
                timer = 0;
                zombieNav.SetTarget(target.position);
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

    public override void SetPosition(Vector3 pos)
    {
        if (zombieBase)
        {
            zombieBase.transform.position = pos;
        }
    }

    public override void SetRotation(Quaternion rot)
    {
    }
    //如果继承，注意重写
    public  void Remove()
    {
        RecoverObject();
        pool.RecoverItem(this);
    }
    public void AfterIntoObjectPool()
    {
    }

    public void SetData(Vector3 serverData)
    {
        SetInVision(true);
        SetPrefabBundlePath("Assets/HotUpdate/prefabs/Zombies/NormalZombie.prefab");
    }
}