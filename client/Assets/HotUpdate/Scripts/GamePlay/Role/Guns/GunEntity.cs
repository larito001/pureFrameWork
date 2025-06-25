using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using YOTO;

public class GunEntity : BaseEntity
{
    private PlayerEntity player;
    private HandRoot handRoot;
    private ParticleSystem fire;
    private Transform fireTrans;
    public float fireRate = 0.2f; // 每发子弹的时间间隔
    private float fireTimer = 0f;

    public bool canFire { get; private set; } // 是否允许发射
    private bool isFiring = false; // 当前是否处于开火状态

    public Transform firePos { get; private set; }
    // private Queue<BulletEntity> bullets = new Queue<BulletEntity>();

    public GunEntity(HandRoot handRoot)
    {
        this.handRoot = handRoot;
    }

    private GameObject gun;
    private LineRenderer laser;
    private Transform laserPoint;
    protected override void YOTOOnload()
    {
        // VFXManager.Instance.Init();


        YOTOFramework.resMgr.LoadGameObject("Assets/HotUpdate/prefabs/Role/Gun/SM_Wep_AssaultRifle_02.prefab",
            Vector3.zero, Quaternion.identity, (obj, pos, rot) =>
            {
                gun = UnityEngine.Object.Instantiate(obj);
                gun.transform.SetParent(handRoot.transform);
                canFire = true;
                // leftHand.data.target = LeftHandTarget;
                gun.transform.localPosition = Vector3.zero;
                gun.transform.localRotation = Quaternion.identity;
                firePos = gun.transform.Find("FirePos");
                YOTOFramework.resMgr.LoadGameObject("Assets/HotUpdate/prefabs/Laser/Prefab/Laser.prefab",
                    Vector3.zero, Quaternion.identity,
                    (obj, pos, rot) =>
                    {
                        laser = GameObject.Instantiate(obj).GetComponent<LineRenderer>();
                        laser.positionCount = 2;
                        laserPoint = laser.GetComponentInChildren<Transform>();
                        laserPoint.parent = null;
                    });
                YOTOFramework.resMgr.LoadGameObject("Assets/HotUpdate/prefabs/Bullet/Fire.prefab", Vector3.zero,
                    Quaternion.identity, (obj, pos, rot) =>
                    {
                        fireTrans = UnityEngine.Object.Instantiate(obj, Vector3.zero, Quaternion.identity).transform;
                        fire = fireTrans.GetComponentInChildren<ParticleSystem>();
                        fireTrans.transform.parent = firePos;
                        fireTrans.transform.localPosition = Vector3.zero;
                        fireTrans.transform.localRotation = Quaternion.identity;
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
        // fireTrans .LookAt(fire.transform.position);
        fireTrans.forward = player.character.transform.forward;
        fire.Play();
        // 获取子弹对象
        var bullet = NormalGunBullet.pool.GetItem(firePos);
        bullet.InstanceGObj();
        bullet.FireFromTo(firePos.position, player.character.transform.forward);
        // bullets.Enqueue(bullet);
        EnemyManager.Instance.SetTarget(this.player.character.transform);
    }

    public override void YOTOStart()
    {
    }

    public override void YOTOUpdate(float deltaTime)
    {
        // 计时器增加
        if (fireTimer < fireRate)
        {
            fireTimer += Time.deltaTime;
        }

        if (laser)
        {
            // 设置 LineRenderer 点的数量（2个点：起点和终点）
       
            if (player.isAimEnd&&player.animatorCtrl.currentWeapon==PlayerAnimatorCtrl.GUN_LAYER)
            {
                if (!laser.enabled)
                {
                    laser.enabled = true;
                }
    
                // 设置起点
                Vector3 start = fireTrans.position;
                Vector3 direction = player.character.transform.forward;

                RaycastHit hit;
                if (Physics.Raycast(start, direction, out hit, 100f))
                {
                    // 如果射线击中物体，设置laser终点为击中点
                    laser.SetPosition(0, start);
                    laser.SetPosition(1, hit.point);
                    laserPoint.position = hit.point;
                }
                else
                {
                    // 如果没有击中，设置终点为最大距离点
                    laser.SetPosition(0, start);
                    laser.SetPosition(1, start + direction * 100f);
                }
            }
            else
            {
                if (laser.enabled)
                {
                    laser.enabled = false;
                }
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

    public void Reload()
    {
        canFire = false;
    }

    public void ReloadEnd()
    {
        canFire = true;
    }

    public void UnuseWeapon()
    {
        canFire = false;
        gun.SetActive(false);
    }

    public void UseWeapon()
    {
        canFire = true;
        gun.SetActive(true);
        //播放动画
        player.animatorCtrl.UseGun();
    }
}