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
    private CrowdGroupAuthoring group;
    public bool isInit=false;
    public bool isDie { get; private set; }
    public void SetTarget(Transform t)
    {
        this.target = t;
    }

    protected override void AfterInstanceGObj()
    {
        objTrans.gameObject.SetActive(true);
        isDie = false;
        zombieBase = objTrans.GetComponent<ZombieAnimatorCtrl>();
        zombieBase.Init(this);
        zombieBase.gameObject.SetActive(true);
        zombieNav = zombieBase.GetComponent<ZombieNavCtrl>();
        zombieNav.Init(this);
        zombieBase.GetComponent<ZombieColliderCtrl>().entityId = this._entityID;
        zombieBase.EnemyRun();
        zombieBase.GetComponent<ZombieColliderCtrl>().Run();
        HP = 100;
        zombieBase.GetComponent<AgentCrowdPathingAuthoring>().Group =group;  

        isInit = true;
     
    }

    public void SetGroup(CrowdGroupAuthoring  group)  
    {
        this.group=group;
        if (zombieBase)
        {
            zombieBase.GetComponent<AgentCrowdPathingAuthoring>().Group =group;  
        }
     

    }

    public void Hurt(float hurt)
    {
      
        HP -= hurt;
        int rand = Random.Range(0, 2); // 0 或 1
        FlyTextMgr.Instance.AddText(hurt.ToString(), this.objTrans.position, (FlyTextType)rand);
        if (HP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        zombieNav.Stop();
        EnemyManager.Instance.RemoveZombie(_entityID);
        YOTOFramework.timeMgr.DelayCall(() =>
        {
            Remove();

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
        if (HP > 0 && zombieNav )
        {
            timer += deltaTime;
            if (timer > 1)
            {
                timer = 0;
                zombieNav.SetTarget(target);
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
    
    //如果继承，注意重写
    public override void Free()
    {
        base.Free();
        objTrans.gameObject.SetActive(false);
        RecoverObject();
        pool.RecoverItem(this);
    }
    
    public void AfterIntoObjectPool()
    {
        isInit = false;
    }

    public void SetData(Vector3 serverData)
    {
        SetInVision(true);
        SetPrefabBundlePath("Assets/HotUpdate/prefabs/Zombies/NormalZombie.prefab");
    }
}