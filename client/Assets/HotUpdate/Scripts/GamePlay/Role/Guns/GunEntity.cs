using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using YOTO;

public class GunEntity : BaseEntity
{

    private PlayerEntity player;
    private HandRoot handRoot;
    private TwoBoneIKConstraint leftHand;
    private ParticleSystem fire;
    public float fireRate = 0.2f; // 每发子弹的时间间隔
    private float fireTimer = 0f;

    public bool canFire = true; // 是否允许发射
    private bool isFiring = false; // 当前是否处于开火状态

    public Transform firePos
    {
        get;
        private set;
    }
    private Queue<BulletEntity> bullets = new Queue<BulletEntity>();
    
    public GunEntity(TwoBoneIKConstraint leftHand ,HandRoot handRoot)
    {
        this.leftHand = leftHand;
        this.handRoot = handRoot;
    }
    private GameObject gun;
    protected override void YOTOOnload()
    {
   
        // VFXManager.Instance.Init();

       
        YOTOFramework.resMgr.LoadGameObject("Assets/HotUpdate/prefabs/Role/Gun/SM_Wep_AssaultRifle_02.prefab", Vector3.zero,Quaternion.identity, (obj,pos,rot) =>
        {
            gun = UnityEngine.Object.Instantiate(obj);
            gun.transform.SetParent(handRoot.transform);
            var LeftHandTarget = gun.transform.Find("LeftHandTarget");
           
            leftHand.data.target = LeftHandTarget;
            gun.transform.localPosition = Vector3.zero;
            gun.transform.localRotation = Quaternion.identity;
            player.builder.Build();
            firePos = gun.transform.Find("FirePos");
            YOTOFramework.resMgr.LoadGameObject("Assets/HotUpdate/prefabs/Bullet/Fire.prefab", Vector3.zero,Quaternion.identity, (obj,pos,rot) =>
            {
                GameObject temp = UnityEngine.Object.Instantiate(obj, Vector3.zero,Quaternion.identity);
                fire= temp.GetComponentInChildren<ParticleSystem>();
                temp.transform.parent = firePos;
                temp.transform.localPosition = Vector3.zero;
                temp.transform.localRotation = Quaternion.identity;
                fire.Stop();
            });
        });
    }

    public void Init(PlayerEntity entity)
    {
        player = entity;

    }

    public void TryShot()
    {
        if (!canFire) return;

        if (fireTimer >= fireRate)
        {
            fireTimer = 0f;
            player.animatorCtrl.TryShoot();
            SpawnBullet();
        }
    }
    // 子弹生成接口（你来实现）
    protected virtual void SpawnBullet()
    {
       
        fire.Stop();
        fire.Play();
        // 获取子弹对象
        var  bullet = NormalGunBullet.pool.GetItem(Vector3.zero);
        bullet.InstanceGObj();
        bullet.FireFromTo( firePos.position,player.character.transform.forward);
        bullets.Enqueue(bullet);
       EnemyManager.Instance.SetTarget(this.player.character.transform);
      
    }
    public override void YOTOStart()
    {
  
    }

    public override void YOTOUpdate(float deltaTime)
    {

        if (bullets.Count > 0)
        {
            var current = bullets.Peek();
            long currentTime = System.DateTime.Now.Ticks / 10000;  // 当前时间戳，单位为毫秒
            long elapsedTime = currentTime - current.GetStartTime();  // 时间差，单位为毫秒

            if (elapsedTime > 3000)  // 如果超过 5 秒（5000 毫秒）
            {
                bullets.Dequeue();
                current.Remove();

            }
        }
        // 计时器增加
        if (fireTimer < fireRate)
        {
            fireTimer += Time.deltaTime;
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

    }

    public override void SetRotation(Quaternion rot)
    {
 
    }
    
}
