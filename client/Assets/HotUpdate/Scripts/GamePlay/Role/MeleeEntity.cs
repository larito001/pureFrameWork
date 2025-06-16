using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YOTO;

public class MeleeEntity : BaseEntity
{
    private PlayerEntity player;
    private HandRoot handRoot;
    private ParticleSystem fire;
    public float fireRate =1f; // 每发子弹的时间间隔
    private float fireTimer = 0f;

    public bool canFire = true; // 是否允许发射
    private bool isFiring = false; // 当前是否处于开火状态

    public Transform firePos { get; private set; }
    private Queue<BulletEntity> bullets = new Queue<BulletEntity>();

    public MeleeEntity(HandRoot handRoot)
    {
        this.handRoot = handRoot;
    }

    private GameObject melee;

    protected override void YOTOOnload()
    {
        // VFXManager.Instance.Init();


        YOTOFramework.resMgr.LoadGameObject("Assets/melee weapons/Prefabs/Axe4.prefab",
            Vector3.zero, Quaternion.identity, (obj, pos, rot) =>
            {
                melee = UnityEngine.Object.Instantiate(obj,handRoot.transform);

                // leftHand.data.target = LeftHandTarget;
                melee.transform.localPosition = Vector3.zero;
                player.builder.Build();
                firePos = melee.transform.Find("FirePos");
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
            player.animatorCtrl.TryUseMelee();
            SpawnBullet();
        }
    }

    // 子弹生成接口（你来实现）
    protected virtual void SpawnBullet()
    {
        if (fire != null)
        {
            fire.Stop();
            fire.Play();   
        }

        // 获取子弹对象
        var bullet = MeleeBullet.pool.GetItem(firePos);
        bullet.InstanceGObj();
        bullet.FireFromTo(firePos.position, player.character.transform.forward);
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
            long currentTime = System.DateTime.Now.Ticks / 10000; // 当前时间戳，单位为毫秒
            long elapsedTime = currentTime - current.GetStartTime(); // 时间差，单位为毫秒

            if (elapsedTime > 1000) // 如果超过 5 秒（5000 毫秒）
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
}